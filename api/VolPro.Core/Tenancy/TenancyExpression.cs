using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VolPro.Core.Configuration;
using VolPro.Core.DBManager;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Core.UserManager;
using VolPro.Entity.DomainModels;
using VolPro.Entity.SystemModels;
using static OfficeOpenXml.ExcelErrorValue;

namespace VolPro.Core.Tenancy
{
    public static class TenancyExpression
    {
        /// <summary>
        /// 获取數據權限sql
        /// 調用方式：DBServerProvider.DbContext.Set<表>().CreateTenancyFilterSql();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string CreateTenancyFilterSql<T>(this IQueryable<T> query) where T : class
        {
            return query.CreateTenancyFilter<T>().ToQueryString();
        }
        /// <summary>
        /// 注意：數據庫表中必须包括appsettings.json配置文件UserIdField的字段才會進行數據隔離。如果表没有這些字段，請在上面單獨写過濾邏輯
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        ///  <param name="checkAdmin">是否對管理員帳號也做驗証</param>
        /// <returns></returns>
        public static IQueryable<T> CreateTenancyFilter<T>(this IQueryable<T> query) where T : class
        {
            //2024.07.04增加租户不分庫统一過濾處理
            // query = query.FilterTenancy();

            //if (AppSetting.TenancyField != null
            //    && typeof(T).GetProperty(AppSetting.TenancyField) != null
            //    && !string.IsNullOrEmpty(UserContext.Current.UserInfo.TenancyValue))
            //{
            //    query = query.Where(AppSetting.TenancyField.CreateExpression<T>(UserContext.Current.UserInfo.TenancyValue, LinqExpressionType.Equal));
            //}

            //是否用户表
            bool isUserTable = typeof(T) == typeof(Sys_User);

            //默認通過創建人id過濾數據
            string filterCreateId = null;
            //用户表通過user_id過濾數據
            if (isUserTable)
            {
                filterCreateId = "User_Id";
            }
            else
            {
                //获取表的創建人id字段，在配置appsettings文件中UserIdField值
                var properties = typeof(T).GetProperties();
                //使用創建人id過濾數據
                filterCreateId = properties.Where(x => x.Name == AppSetting.CreateMember.UserIdField).FirstOrDefault()?.Name;
                if (filterCreateId == null)
                {
                    filterCreateId = properties.Where(x => x.Name == "CreateId").FirstOrDefault()?.Name;
                }
            }


            string tableName = typeof(T).Name;

            //使用用户數據權限(用户管理界面配置的指定看到某些用户創建的數據庫)
            if (AppSetting.UserAuth)
            {
                int[] userIds = UserContext.Current.GetCurrentUserAuthUserIds(tableName);
                //設置查看指定用户的數據
                if (userIds != null && userIds.Length > 0)
                {
                    //没有配置創建人id的表不執行數據權限過濾
                    if (filterCreateId == null)
                    {
                        return query;
                    }
                    return query.Where(filterCreateId.CreateExpression<T>(userIds, LinqExpressionType.In));
                }
            }
            var roleIds = UserContext.Current.RoleIds;

            //2024.08.11增加菜單數據權限(优先级高级角色數據權限)
            List<int> authDataTypes = null;
            string authMenuData = null;
            var permission = UserContext.Current.GetPermissions(tableName.ToLower())??new Permissions() { };
            //自定義數據權限
            if (permission.CustomAuth != null)
            {
                return QueryCustom(query, permission.CustomAuth);
            }
            //没有配置創建人id的表不執行數據權限過濾
            if (filterCreateId == null)
            {
                return query;
            }
            authMenuData = permission.AuthMenuData;
            // string authMenuData = UserContext.Current.GetPermissions(tableName.ToLower())?.AuthMenuData;
            if (!string.IsNullOrEmpty(authMenuData))
            {
                authDataTypes = new List<int>() { authMenuData.GetInt() };
            }
            else
            {
                authDataTypes = RoleContext.GetRoles(x => roleIds.Contains(x.Id))
                   .Where(x => x.AuthData > 0)
                   .Select(s => s.AuthData).ToList();
            }

            if (!isUserTable)
            {
                if (authDataTypes.Count == 0)
                {
                    return query;
                }
            }
            //!!不要给超级管理員設置部门，否则可能會被组织權限共享顯示出来
            if (!isUserTable && (authDataTypes.Contains((int)AuthData.本组织及下數據) || authDataTypes.Contains((int)AuthData.本组织數據)))
            {
                var deptIds = UserContext.Current.DeptIds;
                var userDeptQuery = DBServerProvider.DbContext.Set<Sys_UserDepartment>().Where(x => x.Enable == 1);
                if (authDataTypes.Contains((int)AuthData.本组织及下數據))
                {
                    var childDeptIds = DepartmentContext.GetAllChildrenIds(deptIds);
                    userDeptQuery = userDeptQuery.Where(x => childDeptIds.Contains(x.DepartmentId));
                }
                else
                {
                    userDeptQuery = userDeptQuery.Where(x => deptIds.Contains(x.DepartmentId));
                }

                //分庫
                if (CheckDb<T>())
                {
                    var userIds = userDeptQuery.Select(s => s.UserId).Distinct();
                    query = query.QueryTenancyDynamicShareDBFilter<T>(filterCreateId, userIds);
                }
                else
                {
                    query = query.QueryTenancyFilter<T, Sys_UserDepartment>(filterCreateId, userDeptQuery, "UserId");
                }
                return query;
            }
            //如果角色没有配置數據權限，當前页面是isUserTable=true用户表時，默認顯示當前角色下的數據
            if (isUserTable || authDataTypes.Contains((int)AuthData.本角色以及下數據) || authDataTypes.Contains((int)AuthData.本角色數據))
            {
                var userRoleQuery = DBServerProvider.DbContext.Set<Sys_UserRole>().Where(x => x.Enable == 1 && x.RoleId > 1);
                if (isUserTable || authDataTypes.Contains((int)AuthData.本角色以及下數據))
                {
                    //获取所有子角色
                    var childRoleIds = RoleContext.GetAllChildrenIds(roleIds);
                    userRoleQuery = userRoleQuery.Where(x => childRoleIds.Contains(x.RoleId));
                }
                else
                {
                    userRoleQuery = userRoleQuery.Where(x => roleIds.Contains(x.RoleId));
                }
                //分庫
                if (CheckDb<T>())
                {
                    var userIds = userRoleQuery.Select(s => s.UserId).Distinct();
                    query = query.QueryTenancyDynamicShareDBFilter<T>(filterCreateId, userIds);
                }
                else
                {
                    query = query.QueryTenancyFilter<T, Sys_UserRole>(filterCreateId, userRoleQuery, "UserId");
                }
                return query;
            }
            if (authDataTypes.Contains((int)AuthData.仅自己數據))
            {
                return query.Where(filterCreateId.CreateExpression<T>(UserContext.Current.UserId, LinqExpressionType.Equal));
            }
            return query;
        }



        private static bool CheckDb<T>()
        {
            //是否使用分庫
            return AppSetting.UseDynamicShareDB || typeof(T).BaseType.Name != typeof(SysEntity).Name;
        }
        private static IQueryable<T1> QueryTenancyDynamicShareDBFilter<T1>(this IQueryable<T1> query, string createIdField, IQueryable<int> userIds)
        {
            return query.Where(createIdField.CreateExpression<T1>(userIds.Take(5000).ToArray(), LinqExpressionType.In));
        }
        private static IQueryable<T1> QueryTenancyFilter<T1, T2>(this IQueryable<T1> query, string createIdField, IQueryable<T2> subQuery, string userIdField)
        {
            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            // 創建参數表达式
            ParameterExpression x = Expression.Parameter(t1, "x");
            ParameterExpression c = Expression.Parameter(t2, "c");

            // 构建内部Any条件表达式：c => c.User_Id == x.CreateID

            MemberExpression t1Member = t1.GetProperty(createIdField).PropertyType == typeof(int)
                ? Expression.Property(x, createIdField)
                : Expression.Property(Expression.Property(x, createIdField), "value");

            MemberExpression t2Member = t2.GetProperty(userIdField).PropertyType == typeof(int)
                ? Expression.Property(c, userIdField)
                : Expression.Property(Expression.Property(c, userIdField), "value");

            Expression innerCondition = Expression.Equal(
                t1Member, t2Member
            // //Expression.Property(x, "User_Id")
            // Expression.Property(Expression.Property(x, createIdField), "value"),
            //// Expression.Property(x, "CreateID"),
            //Expression.Property(Expression.Property(c, userIdField), "value")
            );

            // 构建外部Where条件表达式：x => role.Any(c => c.Role_Id == x.User_Id)
            Expression outerCondition = Expression.Call(
                typeof(Queryable),
                "Any",
                new[] { t2 },
                subQuery.Expression,
                Expression.Lambda<Func<T2, bool>>(innerCondition, c)
            );

            // 构建最终的查詢表达式：user.Where(x => role.Any(c => c.Role_Id == x.User_Id))
            Expression<Func<T1, bool>> lambda = Expression.Lambda<Func<T1, bool>>(outerCondition, x);

            query = query.Where(lambda);

            return query;

        }

        /// <summary>
        /// 自定義數據權限
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static IQueryable<T> QueryCustom<T>(this IQueryable<T> query, List<Dictionary<string, string>> list)
        {
            var properties = typeof(T).GetProperties();
            //Expression<Func<T, bool>> orFilter = null;
            Expression<Func<T, bool>> expression = x => true;
            foreach (var item in list)
            {
                if (item.TryGetValue("field", out string field))
                {
                    if (string.IsNullOrEmpty(field))
                    {
                        continue;
                    }
                }
                if (!properties.Any(x => x.Name == field))
                {
                    Console.WriteLine($"表【{typeof(T).GetEntityTableName(false)}】不存在字段【{field}】");
                    continue;
                }
                if (item.TryGetValue("value", out string value))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                }
                value = value.Trim();
                item.TryGetValue("filterType", out string filterType);
                LinqExpressionType type = LinqExpressionType.Equal;
                switch (filterType)
                {
                    case "!=":
                        type = LinqExpressionType.NotEqual;
                        break;
                    case ">":
                        type = LinqExpressionType.GreaterThan;
                        break;
                    case ">=":
                        type = LinqExpressionType.ThanOrEqual;
                        break;
                    case "小于":
                    case "<":
                        type = LinqExpressionType.LessThan;
                        break;
                    case "<=":
                        type = LinqExpressionType.LessThanOrEqual;
                        break;
                    case "in":
                        type = LinqExpressionType.In;
                        break;
                    case "like":
                        type = LinqExpressionType.Like;
                        break;
                    default:
                        break;
                }
                // //值的類型，0自定義,1當前用户,2當前角色,3當前部门
                item.TryGetValue("valueType", out string valueType);
                if (!string.IsNullOrEmpty(valueType) && valueType == "1")
                {

                    switch (value)
                    {
                        //1當前用户
                        case "User_Id":
                            object userVal = UserContext.Current.UserId.ChangeType(properties.Where(x => x.Name == field).FirstOrDefault().PropertyType);
                            expression = expression.And(field.CreateExpression<T>(userVal, LinqExpressionType.Equal));
                            break;
                        //2當前角色
                        case "RoleIds":
                            expression = expression.And(field.CreateExpression<T>(UserContext.Current.RoleIds, LinqExpressionType.In));
                            break;
                        //3當前部门
                        case "DeptIds":
                            expression = expression.And(field.CreateExpression<T>(UserContext.Current.DeptIds, LinqExpressionType.In));
                            break;
                            ;
                        default:
                            //其他自定義字段(UserInfo里面定義的其他字段目前只能是int\string\guid,其他list字段还未支持)
                            var userInfo = UserContext.Current.UserInfo;
                            var property = typeof(UserInfo).GetProperty(value);
                            if (property == null)
                            {
                                Console.WriteLine($"UserInfo未找到字段:{value}");
                                break;
                            }
                            var userInfoVal = property.GetValue(userInfo);
                            if (userInfoVal == null)
                            {
                                break;
                            }
                            expression = expression.And(field.CreateExpression<T>(userInfoVal.ChangeType(properties.Where(x => x.Name == field).FirstOrDefault().PropertyType), type));
                            break;
                    }
                    continue;
                }
                if (type == LinqExpressionType.In || type == LinqExpressionType.NotEqual || value.Contains(','))
                {
                    if (type != LinqExpressionType.In)
                    {
                        type = LinqExpressionType.NotIn;
                    }
                    var values = value.Split(",").Where(s => s != "").Select(s => s.Trim());
                    if (values.Count() == 0)
                    {
                        continue;
                    }
                    expression = expression.And(field.CreateExpression<T>(values, type));
                    continue;
                }
                expression = expression.And(field.CreateExpression<T>(value.ChangeType(properties.Where(x => x.Name == field).FirstOrDefault().PropertyType), type));
            }
            return query.Where(expression);
        }

    }
}

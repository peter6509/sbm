using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VolPro.Core.Const;
using VolPro.Core.DBManager;
using VolPro.Core.Enums;
using VolPro.Core.ManageUser;
using VolPro.Core.UserManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.Tenancy
{
    public static class TenancyManager<T> where T : class
    {
        /// <summary>
        /// 數據隔離操作(2023.08.16优化增加了queryable参數可以写EF查詢，及增加了返回参數)
        /// 注意(必看)：數據庫表字段必须包括appsettings.json配置文件中的CreateMember->UserIdField創建人id字段才會進行數據隔離。
        /// 如果表没有這些字段，請在下面 switch (tableName)單獨写過濾邏輯
        /// </summary>
        /// <param name="tableName">數據庫表名</param>
        /// <returns></returns>
        public static (string sql, IQueryable<T> query) GetSearchQueryable(string multiTenancyString, string tableName, IQueryable<T> queryable)
        {

            //超级管理員不限制(這里可以根據tableName表名自己判断要不要限制超级管理員)
            if (UserContext.Current.IsSuperAdmin)
            {
                return (multiTenancyString, queryable);
            }

            switch (tableName)
            {
                //例如：指定用户表指定查詢条件

                //自定義用户表過濾仅顯示當前租户下的所有用户
                //case nameof(Sys_User):
                //    //  获取當前租户下用户的所有角色
                //    var roleIds = RoleContext.GetRoles(x => x.DbServiceId == UserContext.CurrentServiceId)
                //        //   超级管理員不限制角色
                //        .Where(x => UserContext.Current.IsSuperAdmin ? true : UserContext.Current.RoleIds.Contains(x.Id))
                //        .Select(s => s.Id).ToArray();
                //    // 获取用户的所有子角色
                //    roleIds = RoleContext.GetAllChildren(roleIds).Where(x=>x.DbServiceId== UserContext.CurrentServiceId).Select(s=>s.Id).ToArray();
                //    var roleQuery = DBServerProvider.DbContext.Set<Sys_UserRole>().Where(x => x.Enable == 1 && roleIds.Contains(x.RoleId));//&& x.RoleId > 1 
                //    queryable = (IQueryable<T>)(queryable as IQueryable<Sys_User>).Where(c => roleQuery.Any(uid => c.User_Id == uid.UserId));
                //    break;
                case nameof(Sys_Role):
                    break;
                case nameof(Sys_Group):
                    break;
               
               
                //case nameof(sbm_partner):
                //    break;
                //case nameof(sbm_bu):
                //    break;
                //case nameof(sbm_require_purchase):
                //    break;
                //case nameof(sbm_require_purchase_line):
                //    break;
                //case nameof(sbm_require_purchase_doc):
                //    break;
                case nameof(Sys_Department):
                    //   case nameof(Demo_Order):
                    ////例：訂單管理只看自己角色及子角色對應用户創建的數據
                    ////注：下面的Sys_UserRole表存的是每個角色對應有哪些用户,Sys_UserRole不包括超级管理員RoleId=1的用户

                    /*************************方式一：用EF查詢***********************************/
                    ////1、 获取當前登錄帳的角色及子角色
                    //var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleIds);
                    ////2、查詢這些角色對應的用户
                    //var userIdsQuery = DBServerProvider.DbContext.Set<Sys_UserRole>().Where(x => roleIds.Contains(x.RoleId) && x.Enable == 1).Select(s => s.UserId);

                    ////上面1、2的操作可以简化， RoleContext.GetCurrentAllChildUser()已經封装了获取當前登錄帳號角色下的所有用户id方法
                    ////userIdsQuery = RoleContext.GetCurrentAllChildUser();

                    ////3、進行子查詢
                    //queryable = (IQueryable<T>)(queryable as IQueryable<Demo_Order>).Where(c => userIdsQuery.Any(uid => uid == c.CreateID));

                    ////4、queryable.ToQueryString()可以查看實際生成的sql
                    ////Console.WriteLine(queryable.ToQueryString());
                    ///

                    /*************************方式二：写原生sql查詢***********************************/
                    //multiTenancyString = $" select * from {tableName}  where CreateID in (select UserId from  Sys_UserRole where RoleId in ({string.Join(",", roleIds)}))";


                    /********注：上面的两种方式，如果熟悉表达式语法，尽量采用第一种方式；也可以采用第二种方式写原生sql,數據過濾的規則由自己定義********/

                    //*************************方式三：写原生sql查詢，某些表只能查看自己的數據***********************************/
                    // multiTenancyString += $" select * from {tableName} where CreateID='{UserContext.Current.UserId}'";


                    break;
                default:
                    //1、其他表默認執行數據隔離,隔離方式與角色管理页面的[數據權限]：
                    //2、注：角色設置數據權限后就會進行數據隔離，如果不需要隔離的數據，见上面switch (tableName)说明
                    //3、隔離方式：本组织(部门)及下數據、本组织(部门)數據、本角色以及下數據、本角色數據、仅自己數據


                    //统一執行數據隔離
                    // 注意(必看)：數據庫表字段必须包括appsettings.json配置文件中的CreateMember->UserIdField創建人id字段才會進行數據隔離。
                    // 如果表没有這些字段，請在上面 switch (tableName)單獨写過濾邏輯
                    queryable = queryable.CreateTenancyFilter<T>();
                    break;
            }
            return (multiTenancyString, queryable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">數據庫表名</param>
        /// <param name="ids">當前操作的所有id</param>
        /// <param name="tableKey">主鍵字段</param>
        /// <returns></returns>
        public static string GetMultiTenancySql(string tableName, string ids, string tableKey)
        {
            //使用方法同上
            string multiTenancyString;
            switch (tableName)
            {
                default:
                    multiTenancyString = $"select count(*) FROM {tableName} " +
                       $" where CreateID='{UserContext.Current.UserId}'" +
                       $" and  {tableKey} in ({ids}) ";
                    break;
            }
            return multiTenancyString;
        }
    }



}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VolPro.Core.CacheManager;
using VolPro.Core.DBManager;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Core.UserManager;
using VolPro.Entity;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.ManageUser
{
    public class UserContext
    {
        /// <summary>
        /// 為了尽量减少redis或Memory讀取,保証執行效率,將UserContext注入到DI，
        /// 每個UserContext的屬性至多讀取一次redis或Memory缓存从而提高查詢效率
        /// </summary>
        public static UserContext Current
        {
            get
            {
                return Context.RequestServices.GetService(typeof(UserContext)) as UserContext;
            }
        }

        private static Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return Utilities.HttpContext.Current;
            }
        }
        private static ICacheService CacheService
        {
            get { return GetService<ICacheService>(); }
        }

        private static T GetService<T>() where T : class
        {
            return AutofacContainerModule.GetService<T>();
        }

        public UserInfo UserInfo
        {
            get
            {
                if (_userInfo != null)
                {
                    return _userInfo;
                }
                return GetUserInfo(UserId);
            }
        }

        private UserInfo _userInfo { get; set; }

        /// <summary>
        /// 角色ID為1的默認為超级管理員
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return IsRoleIdSuperAdmin(this.RoleIds); }
        }
        /// <summary>
        /// 角色ID為1的默認為超级管理員
        /// </summary>
        public static bool IsRoleIdSuperAdmin(int[] roleIds)
        {
            return roleIds.Contains(1);
        }

        public static bool IsRoleIdSuperAdmin(int roleId)
        {
            return roleId == 1;
        }

        public UserInfo GetUserInfo(int userId)
        {
            if (_userInfo != null) return _userInfo;
            if (userId <= 0)
            {
                _userInfo = new UserInfo() { RoleIds = new int[] { } };
                return _userInfo;
            }
            string key = userId.GetUserIdKey();
            _userInfo = CacheService.Get<UserInfo>(key);
            if (_userInfo != null && _userInfo.User_Id > 0) return _userInfo;

            _userInfo = DBServerProvider.DbContext.Set<Sys_User>()
                .Where(x => x.User_Id == userId).Select(s => new
                {
                    User_Id = userId,
                    Role_Id = s.Role_Id ?? 0,
                    Token = s.Token,
                    UserName = s.UserName,
                    UserTrueName = s.UserTrueName,
                    Enable = 1,
                    s.RoleIds,
                    s.DeptIds
                    //s.PostId
                }).ToList().Select(s => new UserInfo()
                {
                    User_Id = userId,
                    Role_Id = s.Role_Id,
                    Token = s.Token,
                    UserName = s.UserName,
                    UserTrueName = s.UserTrueName,
                    Enable = 1,
                    RoleIds = s.Role_Id == 1 ? new int[] { 1 } : (string.IsNullOrEmpty(s.RoleIds) ? new int[] { } : s.RoleIds.Split(",").Select(x => x.GetInt()).ToArray()),
                    DeptIds = string.IsNullOrEmpty(s.DeptIds) ? new List<Guid>() : s.DeptIds.Split(",").Select(x => (Guid)x.GetGuid()).ToList()
                    //PostIds = string.IsNullOrEmpty(s.PostId) ? new List<Guid>() : s.PostId.Split(",").Select(x => (Guid)x.GetGuid()).ToList(),
                    //TenancyValue = null //用户租户字段請在此處返回實现
                }).FirstOrDefault();

            if (_userInfo != null && _userInfo.User_Id > 0)
            {
                CacheService.AddObject(key, _userInfo);
            }
            return _userInfo ?? new UserInfo() { RoleIds = new int[] { } };
        }

        /// <summary>
        /// 获取角色權限時通過安全字典锁定的角色id
        /// </summary>
        private static ConcurrentDictionary<string, object> objKeyValue = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 角色權限的版本號
        /// </summary>
        private static readonly Dictionary<int, string> rolePermissionsVersion = new Dictionary<int, string>();

        /// <summary>
        /// 每個角色ID對應的菜單權限（已做静態化處理）
        /// 每次获取權限時用當前服務器的版本號與redis/memory缓存的版本比较,如果不同會重新刷新缓存
        /// </summary>
        private static readonly Dictionary<int, List<Permissions>> rolePermissions = new Dictionary<int, List<Permissions>>();


        private static ConcurrentDictionary<int, object> objUserValue = new ConcurrentDictionary<int, object>();

        private static readonly Dictionary<int, string> userAuthVersion = new Dictionary<int, string>();

        private static readonly Dictionary<int, List<UserAuthData>> userAuthPermissions = new Dictionary<int, List<UserAuthData>>();

        /// <summary>
        /// 获取用户所有的菜單權限
        /// </summary>

        public List<Permissions> Permissions
        {
            get
            {
                return GetPermissions(RoleIds);
            }
        }

        /// <summary>
        /// 菜單按钮变更時，同時刷新權限缓存2022.05.23
        /// </summary>
        /// <param name="menuId"></param>
        public void RefreshWithMenuActionChange(int menuId)
        {
            foreach (var roleId in rolePermissions.Where(c => c.Value.Any(x => x.Menu_Id == menuId)).Select(s => s.Key))
            {
                if (rolePermissionsVersion.ContainsKey(roleId))
                {
                    CacheService.Add(roleId.GetRoleIdKey(), DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                }
            }

        }

        /// <summary>
        /// 获取單個表的權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Permissions GetPermissions(string tableName)
        {
            return GetPermissions(RoleIds).Where(x => x.TableName == tableName).FirstOrDefault();
        }
        /// <summary>
        /// 2022.03.26
        /// 菜單類型1:移動端，0:PC端
        /// </summary>
        public static int MenuType
        {
            get
            {
                return Context.Request.Headers.ContainsKey("uapp") ? 1 : 0;
            }
        }
        /// <summary>
        /// 自定条件查詢權限
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Permissions GetPermissions(Func<Permissions, bool> func)
        {
            // 2022.03.26增移動端加菜單類型判断
            return GetPermissions(RoleIds).Where(func).Where(x => x.MenuType == MenuType).FirstOrDefault();
        }

        private List<Permissions> ActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    var menuAuthArr = x.MenuAuth.DeserializeObject<List<Sys_Actions>>();
                    x.UserAuthArr = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.Split(",").Where(c => menuAuthArr.Any(m => m.Value == c)).ToArray();

                }
                catch { }
                finally
                {
                    if (x.UserAuthArr == null)
                    {
                        x.UserAuthArr = new string[0];
                    }
                }
            });
            return permissions;
        }
        private List<Permissions> MenuActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    x.UserAuthArr = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.DeserializeObject<List<Sys_Actions>>().Select(s => s.Value).ToArray();
                }
                catch { }
                finally
                {
                    if (x.UserAuthArr == null)
                    {
                        x.UserAuthArr = new string[0];
                    }
                }
            });
            return permissions;
        }
        /// <summary>
        /// 用户菜單權限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<Permissions> GetPermissions(int[] roleIds)
        {
            if (IsRoleIdSuperAdmin(roleIds))
            {
                //2020.12.27增加菜單界面上不顯示，但可以分配權限
                var permissions = DBServerProvider.DbContext.Set<Sys_Menu>()
                    .Where(x => x.Enable == 1 || x.Enable == 2)
                    .Select(a => new Permissions
                    {
                        Menu_Id = a.Menu_Id,
                        ParentId = a.ParentId,
                        //2020.05.06增加默認將表名转換成小写，權限驗証時不再转換
                        TableName = (a.TableName ?? "").ToLower(),
                        //MenuAuth = a.Auth,
                        UserAuth = a.Auth,
                        // 2022.03.26增移動端加菜單類型
                        MenuType = a.MenuType ?? 0
                    }).ToList();
                return MenuActionToArray(permissions);
            }
            ICacheService cacheService = CacheService;
            for (int i = 0; i < roleIds.Length; i++)
            {
                int roleId = roleIds[i];

                string roleKey = roleId.GetRoleIdKey();

                //角色有缓存，並且當前服務器的角色版本號與redis/memory缓存角色的版本號相同直接返回静態對象角色權限
                string currnetVeriosn = "";
                if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                    && currnetVeriosn == cacheService.Get(roleKey))
                {
                    continue;
                    //return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
                }

                //锁定每個角色，通過安全字典减少锁粒度，否则多個同時角色获取缓存會导致阻塞
                object objId = objKeyValue.GetOrAdd(roleId.ToString(), new object());
                //锁定每個角色
                lock (objId)
                {
                    if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                        && currnetVeriosn == cacheService.Get(roleKey))
                    {
                        continue;
                        //  return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
                    }

                    //没有redis/memory缓存角色的版本號或與當前服務器的角色版本號不同時，刷新缓存
                    var dbContext = DBServerProvider.DbContext;
                    List<Permissions> _permissions = (from a in dbContext.Set<Sys_Menu>()
                                                      join b in dbContext.Set<Sys_RoleAuth>()
                                                      on a.Menu_Id equals b.Menu_Id
                                                      where b.Role_Id == roleId //&& a.ParentId > 0
                                                      && b.AuthValue != ""
                                                      orderby a.ParentId
                                                      select new Permissions
                                                      {
                                                          Menu_Id = a.Menu_Id,
                                                          ParentId = a.ParentId,
                                                          //2020.05.06增加默認將表名转換成小写，權限驗証時不再转換
                                                          TableName = (a.TableName ?? "").ToLower(),
                                                          MenuAuth = a.Auth,
                                                          UserAuth = b.AuthValue ?? "",
                                                          AuthMenuData = b.AuthMenuData,
                                                          // 2022.03.26增移動端加菜單類型
                                                          MenuType = a.MenuType ?? 0
                                                      }).ToList();
                    ActionToArray(_permissions);
                    string _version = cacheService.Get(roleKey);
                    //生成一個唯一版本號標識
                    if (_version == null)
                    {
                        _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                        //將版本號写入缓存
                        cacheService.Add(roleKey, _version);
                    }
                    //刷新當前服務器角色的權限
                    rolePermissions[roleId] = _permissions;

                    //写入當前服務器的角色最新版本號
                    rolePermissionsVersion[roleId] = _version;

                }
            }
            return rolePermissions.Where(x => roleIds.Contains(x.Key))
                 .SelectMany(s => s.Value)
                 .GroupBy(g => g.Menu_Id)
                 .Select(s => new Permissions()
                 {
                     Menu_Id = s.Key,
                     ParentId = s.Select(s1 => s1.ParentId).FirstOrDefault(),
                     TableName = s.Select(s1 => s1.TableName).FirstOrDefault(),
                     MenuType = s.Select(c => c.MenuType).FirstOrDefault(),
                     UserAuthArr = s.SelectMany(s1 => s1.UserAuthArr).ToArray(),
                     AuthMenuData=s.Select(s1=>s1.AuthMenuData).FirstOrDefault()
                 }).ToList();


        }

        /// <summary>
        /// 获取用户的表的能看到的數據用户id
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int[] GetCurrentUserAuthUserIds(string tableName)
        {
            return GetUserAuthData(UserId)?.Where(x => x.TableName == tableName.ToLower())?.FirstOrDefault()?.UserIds;
        }

        public List<UserAuthData> CurrentUserAuthUserIds
        {
            get
            {
                return GetUserAuthData(UserId);
            }
        }

        public void RemoveUserAuthData(int userId)
        {
            userAuthVersion.Remove(userId);
        }

        /// <summary>
        /// 用户指定可以看到的某些用户的數據權限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<UserAuthData> GetUserAuthData(int userId)
        {
            if (userId <= 0) return null;

            string currnetVeriosn = "";
            string authKey = $"uh:{userId}";
            ICacheService cacheService = CacheService;

            if (!(userAuthVersion.TryGetValue(userId, out currnetVeriosn)
                && currnetVeriosn == cacheService.Get(authKey)))
            {
                object objId = objUserValue.GetOrAdd(userId, new object());
                //锁定用户
                lock (objId)
                {
                    if (!((userAuthVersion.TryGetValue(userId, out currnetVeriosn)
                        && currnetVeriosn == cacheService.Get(authKey))))
                    {
                        var menuQuery = DBServerProvider.DbContext.Set<Sys_Menu>();
                        var authQuery = DBServerProvider.DbContext.Set<Sys_UserAuth>()
                                   .Where(x => x.UserId == userId && x.AuthUserIds != "")
                                   .Select(s => new
                                   {
                                       s.UserId,
                                       s.AuthUserIds,
                                       s.MenuId,
                                       TableName = menuQuery.Where(x => x.Menu_Id == s.MenuId).Select(m => m.TableName).FirstOrDefault()
                                   });

                        var authData = authQuery.ToList().Select(s => new UserAuthData()
                        {
                            UserIds = s.AuthUserIds.Split(",").Select(x => x.GetInt()).ToArray(),
                            Menu_Id = s.MenuId ?? 0,
                            TableName = s.TableName?.ToLower()
                        }).ToList();

                        string _version = cacheService.Get(authKey);
                        //生成一個唯一版本號標識
                        if (_version == null)
                        {
                            _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                            //將版本號写入缓存
                            cacheService.Add(authKey, _version);
                        }
                        //刷新當前服務器角色的權限
                        userAuthPermissions[userId] = authData;

                        //写入當前服務器的角色最新版本號
                        userAuthVersion[userId] = _version;

                        return authData;
                    }
                }
            }
            return userAuthPermissions[userId];
        }

        /// <summary>
        /// 判断是否有權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, string authName, int[] roleIds)
        {
            if (roleIds == null || roleIds.Length == 0)
            {
                return false;
            };
            tableName = tableName.ToLower();
            return GetPermissions(roleIds).Any(x => x.TableName == tableName && x.UserAuthArr.Contains(authName));
        }

        /// <summary>
        /// 判断是否有權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, ActionPermissionOptions actionPermission, int[] roleIds)
        {
            return ExistsPermissions(tableName, actionPermission.ToString(), roleIds);
        }
        public int UserId
        {
            get
            {
                return (Context.User.FindFirstValue(JwtRegisteredClaimNames.Jti)
                    ?? Context.User.FindFirstValue(ClaimTypes.NameIdentifier)).GetInt();
            }
        }

        public string UserName
        {
            get { return UserInfo.UserName; }
        }

        public string UserTrueName
        {
            get { return UserInfo.UserTrueName; }
        }

        public string Token
        {
            get { return UserInfo.Token; }
        }

        public int[] RoleIds
        {
            get { return UserInfo.RoleIds; }
        }


        public List<Guid> DeptIds
        {
            get { return UserInfo.DeptIds; }
        }

        /// <summary>
        /// 获取所有子角色包括自己
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllChildrenRoleIds()
        {
            return RoleContext.GetAllChildrenIds(RoleIds);
        }


        /// <summary>
        /// 获取所有子角色下的用户
        /// </summary>
        /// <param name="isCurrent">只获取當前角色下的用户(不包括子角色)</param>
        /// <returns></returns>
        public IQueryable<int> GetAllChildrenRoleIdUserIds(bool isCurrent)
        {
            var roleIds = isCurrent ? RoleIds.ToList() : GetAllChildrenRoleIds();
            return DBServerProvider.DbContext.Set<Sys_UserRole>().Where(x => x.Enable == 1 && roleIds.Contains(x.RoleId))
              .Select(s => s.UserId);
        }


        /// <summary>
        /// 获取所有子部门
        /// </summary>
        /// <returns></returns>
        public List<Guid> GetAllChildrenDeptIds()
        {
            return DepartmentContext.GetAllChildrenIds(DeptIds);
        }


        /// <summary>
        /// 获取所有子部门下的所有用户id
        /// </summary>
        /// <param name="isCurrent">只获取當前部门下的用户(不包括子部门)</param>
        /// <returns></returns>
        public IQueryable<int> GetAllChildrenDeptIdUserIds(bool isCurrent)
        {
            List<Guid> deptIds = isCurrent ? DeptIds : GetAllChildrenDeptIds();
            return DBServerProvider.DbContext.Set<Sys_UserDepartment>().Where(x => x.Enable == 1 && deptIds.Contains(x.DepartmentId))
                 .Select(s => s.UserId);
        }

        public void LogOut(int userId)
        {
            CacheService.Remove(userId.GetUserIdKey());
        }
        /// <summary>
        /// 當前选中的數據庫
        /// </summary>
        public static Guid CurrentServiceId
        {
            get
            {
                if (Context.Request.Headers.TryGetValue("serviceId", out StringValues value))
                {
                    var val = value.GetGuid() ?? Guid.NewGuid();
                    if (Current.IsSuperAdmin)
                    {
                        return val;
                    }
                    var roleIds = Current.RoleIds;
                    if (RoleContext.GetRoles(x => roleIds.Contains(x.Id)).Any(x => x.DbServiceId == val))
                    {
                        return val;
                    }
                }
                //2023.12.28优化移動端租户选择
                if (Current.RoleIds.Length > 0)
                {
                    return RoleContext.GetRoles(x => Current.RoleIds.Contains(x.Id)).Select(s => s.DbServiceId).FirstOrDefault();
                }
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 當前选中的部门庫
        /// </summary>
        public static Guid CurrentDeptId
        {
            get
            {
                if (Context.Request.Headers.TryGetValue("deptId", out StringValues value))
                {
                    var val = value.GetGuid() ?? Guid.NewGuid();
                    //if (Current.IsSuperAdmin)
                    //{
                    return val;
                    //}
                    //var roleIds = Current.RoleIds;
                    //if (RoleContext.GetRoles(x => roleIds.Contains(x.Id)).Any(x => x.DbServiceId == val))
                    //{
                    //    return val;
                    //}
                }
                return Guid.Empty;
            }
        }
        /// <summary>
        /// 获取用户所有數據庫
        /// </summary>
        public IEnumerable<Sys_DbService> UserDbService
        {
            get
            {
                if (IsSuperAdmin)
                {
                    return DbCache.GetDbInfo(x => true);
                }
                var roleIds = RoleIds;
                var dbIds = RoleContext.GetRoles(x => roleIds.Contains(x.Id)).Select(s => s.DbServiceId).ToList();
                return DbCache.GetDbInfo(x => dbIds.Contains(x.DbServiceId));//.Select(s => new { s.DbServiceName, s.DbServiceId });
            }
        }
        /// <summary>
        /// 获取租户下的所有用户
        /// </summary>
        /// <param name="dbServiceId"></param>
        /// <returns></returns>
        public IQueryable<int> GetQueryableTenancyUser(Guid dbServiceId)
        {
            var roleIds = RoleContext.GetRoles(x => x.DbServiceId == dbServiceId).Select(s => s.Id);
            return DBServerProvider.DbContext.Set<Sys_UserRole>().Where(x => x.Enable == 1 && x.RoleId > 1 && roleIds.Contains(x.RoleId)).
                Select(s => s.UserId);
        }
        /// <summary>
        /// 获取指定租户下的所有用户id
        /// </summary>
        /// <param name="dbServiceId"></param>
        /// <returns></returns>
        public List<int> GetTenancyUserId(Guid dbServiceId)
        {
            return GetQueryableTenancyUser(dbServiceId).ToList();
        }
        /// <summary>
        ///  获取當前租户下的所有用户id
        /// </summary>
        public List<int> GetCurrentTenancyUserId
        {
            get
            {
                return GetQueryableTenancyUser(CurrentServiceId).ToList();
            }
        }
    }

    public class UserAuthData
    {
        /// <summary>
        /// 當前用户指定的可以看到的用户數據
        /// </summary>
        public int[] UserIds { get; set; }
        public int Menu_Id { get; set; }

        public string TableName { get; set; }
        /// <summary>
        /// 2022.03.26
        /// 菜單類型1:移動端，0:PC端
        /// </summary>
        public int MenuType { get; set; }
    }
}

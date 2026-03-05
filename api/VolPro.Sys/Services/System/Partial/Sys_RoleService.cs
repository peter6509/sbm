using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VolPro.Core;
using VolPro.Core.DBManager;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Core.Services;
using VolPro.Core.UserManager;
using VolPro.Core.Utilities;
using VolPro.Entity;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IRepositories;

namespace VolPro.Sys.Services
{
    public partial class Sys_RoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_RoleRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Sys_RoleService(
            ISys_RoleRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
        }
        private WebResponseContent _responseContent = new WebResponseContent();
        public override PageGridData<Sys_Role> GetPageData(PageDataOptions pageData)
        {
            IsMultiTenancy = false;
            //角色Id=1默認為超级管理員角色，界面上不顯示此角色
            QueryRelativeExpression = (IQueryable<Sys_Role> queryable) =>
            {
                if (UserContext.Current.IsSuperAdmin)
                {
                    return queryable;
                }
                List<int> roleIds = GetAllChildrenRoleIdAndSelf();
                return queryable.Where(x => roleIds.Contains(x.Role_Id));
            };
            return base.GetPageData(pageData);
        }
        /// <summary>
        /// 编輯權限時，获取當前用户的所有菜單權限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserTreePermission()
        {
            return await GetUserTreePermission(UserContext.Current.RoleIds);
        }

        /// <summary>
        /// 编輯權限時，获取指定角色的所有菜單權限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetUserTreePermission(int[] roleIds)
        {
            WebResponseContent webResponse = new WebResponseContent();
            //if (!UserContext.IsRoleIdSuperAdmin(roleId) && !UserContext.Current.RoleId.Contains( roleId))
            //{
            //    //if (!(await GetAllChildrenAsync(UserContext.Current.RoleId)).Exists(x => x.Id == roleId))
            //    //{
            //    //    return webResponse.Error("没有權限获取此角色的權限信息");
            //    //}
            //}
            //获取用户權限
            List<Permissions> permissions = UserContext.Current.GetPermissions(roleIds);
            //權限用户權限查詢所有的菜單信息
            List<Sys_Menu> menus = await Task.Run(() => Sys_MenuService.Instance.GetUserMenuList(roleIds));
            //获取當前用户權限如:(Add,Search)對應的顯示文本信息如:Add：添加，Search:查詢
            var data = menus.Select(x => new UserPermissions
            {
                Id = x.Menu_Id,
                Pid = x.ParentId,
                Text = x.MenuName,
                IsApp = x.MenuType == 1,
                Actions = GetActions(x.Menu_Id, x.Actions, permissions, roleIds),
                AuthMenuData = permissions.Where(c => c.Menu_Id == x.Menu_Id).Select(s => s.AuthMenuData).FirstOrDefault()
            });
            return webResponse.OK(null, data);
        }

        private List<Sys_Actions> GetActions(int menuId, List<Sys_Actions> menuActions, List<Permissions> permissions, int[] roleIds)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleIds))
            {
                return menuActions;
            }

            return menuActions.Where(p => permissions
                 .Exists(w => menuId == w.Menu_Id
                 && w.UserAuthArr.Contains(p.Value)))
                  .ToList();
        }
        private List<RoleNodes> rolesChildren = new List<RoleNodes>();

        /// <summary>
        /// 编輯權限時获取當前用户下的所有角色與當前用户的菜單權限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentTreePermission()
        {
            _responseContent = await GetCurrentUserTreePermission();
            var roleIds = UserContext.Current.RoleIds;
            return _responseContent.OK(null, new
            {
                tree = _responseContent.Data,
                roles = await GetAllChildrenAsync(roleIds)
            });
        }

        private List<RoleNodes> roles = null;

        /// <summary>
        /// 获取當前角色下的所有角色包括自己的角色Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllChildrenRoleIdAndSelf()
        {
            var _roleIds = UserContext.Current.RoleIds;
            List<int> roleIds = GetAllChildren(_roleIds).Select(x => x.Id).ToList();
            roleIds.AddRange(_roleIds);
            return roleIds;
        }


        /// <summary>
        /// 获取當前角色下的所有角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<RoleNodes> GetAllChildren(int roleId)
        {
            roles = GetAllRoleQueryable().ToList();
            return GetAllChildrenNodes(new int[] { roleId });
        }

        public List<RoleNodes> GetAllChildren(int[] roleIds)
        {
            roles = GetAllRoleQueryable().ToList();
            return GetAllChildrenNodes(roleIds);
        }

        public async Task<List<RoleNodes>> GetAllChildrenAsync(int[] roleIds)
        {
            roles = await GetAllRoleQueryable().ToListAsync();
            return GetAllChildrenNodes(roleIds);
        }
        private IQueryable<RoleNodes> GetAllRoleQueryable()
        {
            return repository
                   .FindAsIQueryable(
                   x => x.Enable == 1 && x.Role_Id > 1)
                   .Select(
                   s => new RoleNodes()
                   {
                       Id = s.Role_Id,
                       ParentId = s.ParentId,
                       RoleName = s.RoleName
                   });
        }

        public async Task<List<int>> GetAllChildrenRoleIdAsync(int roleId)
        {
            return (await GetAllChildrenAsync(new int[] { roleId })).Select(x => x.Id).ToList();
        }


        public List<int> GetAllChildrenRoleId(int roleId)
        {
            return GetAllChildren(roleId).Select(x => x.Id).ToList();
        }

        public List<int> GetAllChildrenRoleId(int[] roleIds)
        {
            return GetAllChildrenNodes(roleIds).Select(x => x.Id).ToList();
        }

        private List<RoleNodes> GetAllChildrenNodes(int[] roleIds)
        {
            return RoleContext.GetAllChildren(roleIds);
        }


        /// <summary>
        /// 保存角色權限
        /// </summary>
        /// <param name="userPermissions"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SavePermission(List<UserPermissions> userPermissions, int roleId)
        {

            string message = "";
            try
            {
                UserInfo user = UserContext.Current.UserInfo;
                if (!(await GetAllChildrenAsync(user.RoleIds)).Exists(x => x.Id == roleId))
                    return _responseContent.Error("没有權限修改此角色的權限信息");
                //當前用户的權限
                List<Permissions> permissions = UserContext.Current.Permissions;

                List<int> originalMeunIds = new List<int>();
                //被分配角色的權限
                List<Sys_RoleAuth> roleAuths = await repository.FindAsync<Sys_RoleAuth>(x => x.Role_Id == roleId);
                List<Sys_RoleAuth> updateAuths = new List<Sys_RoleAuth>();
                foreach (UserPermissions x in userPermissions)
                {
                    Permissions per = permissions.Where(p => p.Menu_Id == x.Id).FirstOrDefault();
                    //不能分配超過當前用户的權限
                    if (per == null) continue;
                    //per.UserAuthArr.Contains(a.Value)校驗權限范围
                    string[] arr = x.Actions == null || x.Actions.Count == 0
                      ? new string[0]
                      : x.Actions.Where(a => per.UserAuthArr.Contains(a.Value))
                      .Select(s => s.Value).ToArray();

                    //如果當前權限没有分配過，設置Auth_Id默認為0，表示新增的權限
                    var auth = roleAuths.Where(r => r.Menu_Id == x.Id).Select(s => new { s.Auth_Id, s.AuthValue, s.Menu_Id, s.AuthMenuData }).FirstOrDefault();
                    string newAuthValue = string.Join(",", arr);
                    //權限没有發生变化则不處理
                    if (auth == null || auth.AuthValue != newAuthValue || auth.AuthMenuData != x.AuthMenuData)
                    {
                        updateAuths.Add(new Sys_RoleAuth()
                        {
                            Role_Id = roleId,
                            Menu_Id = x.Id,
                            AuthValue = string.Join(",", arr),
                            AuthMenuData = x.AuthMenuData,
                            Auth_Id = auth == null ? 0 : auth.Auth_Id,
                            ModifyDate = DateTime.Now,
                            Modifier = user.UserTrueName,
                            CreateDate = DateTime.Now,
                            Creator = user.UserTrueName
                        });
                    }
                    else
                    {
                        originalMeunIds.Add(auth.Menu_Id);
                    }

                }
                //更新權限
                repository.UpdateRange(updateAuths.Where(x => x.Auth_Id > 0), x => new
                {
                    x.Menu_Id,
                    x.AuthValue,
                    x.AuthMenuData,
                    x.Modifier,
                    x.ModifyDate
                });
                //新增的權限
                repository.AddRange(updateAuths.Where(x => x.Auth_Id <= 0));

                //获取權限取消的權限
                int[] authIds = roleAuths.Where(x => userPermissions.Select(u => u.Id)
                 .ToList().Contains(x.Menu_Id) || originalMeunIds.Contains(x.Menu_Id))
                .Select(s => s.Auth_Id)
                .ToArray();
                List<Sys_RoleAuth> delAuths = roleAuths.Where(x => x.AuthValue != "" && !authIds.Contains(x.Auth_Id)).ToList();
                delAuths.ForEach(x =>
                {
                    x.AuthValue = "";
                    x.AuthMenuData = "";
                });
                //將取消的權限設置為""
                repository.UpdateRange(delAuths, x => new
                {
                    x.Menu_Id,
                    x.AuthValue,
                    x.AuthMenuData,
                    x.Modifier,
                    x.ModifyDate
                });

                int addCount = updateAuths.Where(x => x.Auth_Id <= 0).Count();
                int updateCount = updateAuths.Where(x => x.Auth_Id > 0).Count();
                await repository.SaveChangesAsync();

                string _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                //標識缓存已更新
                base.CacheContext.Add(roleId.GetRoleIdKey(), _version);

                _responseContent.OK($"保存成功：新增加配菜單權限{addCount}条,更新菜單{updateCount}条,删除權限{delAuths.Count()}条");
            }
            catch (Exception ex)
            {
                message = "异常信息：" + ex.Message + ex.StackTrace + ex.InnerException + ",";
                Console.WriteLine(message);
            }
            finally
            {
                Logger.Info($"權限分配置:{message}{_responseContent.Message}");
            }

            return _responseContent;
        }

        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            AddOnExecuting = (Sys_Role role, object obj) =>
            {
                if (!UserContext.Current.IsSuperAdmin && role.ParentId > 0 && !RoleContext.GetAllChildrenIds(UserContext.Current.RoleIds).Contains(role.ParentId))
                {
                    return _responseContent.Error("不能添加此角色".Translator());
                }
                if (role.DbServiceId == null && !UserContext.IsRoleIdSuperAdmin(role.Role_Id))
                {
                    role.DbServiceId = UserContext.CurrentServiceId;
                }
                return ValidateRoleName(role, x => x.RoleName == role.RoleName);
            };
            return RemoveCache(base.Add(saveDataModel));
        }

        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            if (!UserContext.Current.IsSuperAdmin)
            {
                var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleIds);
                var _keys = keys.Select(s => s.GetInt());
                if (_keys.Any(x => !roleIds.Contains(x)))
                {
                    return _responseContent.Error("没有權限删除此角色");
                }
            }
            return RemoveCache(base.Del(keys, delList));
        }

        private WebResponseContent ValidateRoleName(Sys_Role role, Expression<Func<Sys_Role, bool>> predicate)
        {
            //if (repository.Exists(predicate)) 
            //{
            //    return _responseContent.Error("角色名【{$ts}】已存在,請設置其他角色名".TranslatorFormat(role.RoleName));
            //}
            return _responseContent.OK();
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {
            UpdateOnExecuting = (Sys_Role role, object obj1, object obj2, List<object> obj3) =>
            {
                if (role.Role_Id == 1)
                {
                    role.ParentId = 0;
                }
                //2020.05.07新增禁止选择上级角色為自己
                if (role.Role_Id == role.ParentId)
                {
                    return _responseContent.Error($"上级角色不能选择自己".Translator());
                }
                if (repository.Exists(x => x.Role_Id == role.ParentId && x.ParentId == role.Role_Id))
                {
                    return _responseContent.Error($"不能选择此上级角色，选择的上级角色與當前角色形成依赖关系".Translator());
                }
                if (!UserContext.Current.IsSuperAdmin)
                {
                    var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleIds);
                    if (role.ParentId > 0)
                    {
                        if (!roleIds.Contains(role.ParentId))
                        {
                            return _responseContent.Error($"不能选择此角色");
                        }
                    }
                    if (!roleIds.Contains(role.Role_Id))
                    {
                        return _responseContent.Error($"不能选择此角色");
                    }
                    return _responseContent.OK("");
                }
                return ValidateRoleName(role, x => x.RoleName == role.RoleName && x.Role_Id != role.Role_Id);
            };
            return RemoveCache(base.Update(saveModel));
        }
        private WebResponseContent RemoveCache(WebResponseContent webResponse)
        {
            if (webResponse.Status)
            {
                RoleContext.Refresh();
            }
            return webResponse;
        }
    }

    public class UserPermissions
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Text { get; set; }
        public bool IsApp { get; set; }
        public List<Sys_Actions> Actions { get; set; }

        /// <summary>
        ///菜單數據權限
        /// </summary>
        [Display(Name = "菜單數據權限")]
        public string AuthMenuData { get; set; }
    }
}

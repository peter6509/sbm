using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Core.Services;
using VolPro.Core.Utilities;
using VolPro.Entity;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IRepositories;

namespace VolPro.Sys.Services
{
    public partial class Sys_MenuService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_MenuRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Sys_MenuService(
            ISys_MenuRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
        }
        /// <summary>
        /// 菜單静態化處理，每次获取菜單時先比较菜單是否發生变化，如果發生变化从數據庫重新获取，否则直接获取_menus菜單
        /// </summary>
        private static List<Sys_Menu> _menus { get; set; }

        /// <summary>
        /// 从數據庫获取菜單時锁定的對象
        /// </summary>
        private static object _menuObj = new object();

        /// <summary>
        /// 當前服務器的菜單版本
        /// </summary>
        private static string _menuVersionn = "";

        private const string _menuCacheKey = "inernalMenu";

        /// <summary>
        /// 编輯修改菜單時,获取所有菜單
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetMenu()
        {
            //  DBServerProvider.SqlDapper.q
            return await repository.FindAsIQueryable(x => true)
                .OrderByDescending(a => a.OrderNo)
                .ThenByDescending(q => q.ParentId)
                .Select(a =>
            new
            {
                id = a.Menu_Id,
                parentId = a.ParentId,
                name = a.MenuName,
                a.Icon,
                a.MenuType,
                a.OrderNo
            }).ToListAsync();

        }

        private List<Sys_Menu> GetAllMenu()
        {
            //每次比较缓存是否更新過，如果更新则重新获取數據
            string _cacheVersion = CacheContext.Get(_menuCacheKey);
            if (_menuVersionn != "" && _menuVersionn == _cacheVersion)
            {
                return _menus ?? new List<Sys_Menu>();
            }
            lock (_menuObj)
            {
                if (_menuVersionn != "" && _menus != null && _menuVersionn == _cacheVersion) return _menus;
                //2020.12.27增加菜單界面上不顯示，但可以分配權限
                _menus = repository.FindAsIQueryable(x => x.Enable == 1 || x.Enable == 2)
                    .OrderByDescending(a => a.OrderNo)
                    .ThenByDescending(q => q.ParentId).ToList();

                _menus.ForEach(x =>
                {
                    // 2022.03.26增移動端加菜單類型
                    x.MenuType ??= 0;
                    if (!string.IsNullOrEmpty(x.Auth) && x.Auth.Length > 10)
                    {
                        try
                        {
                            x.Actions = x.Auth.DeserializeObject<List<Sys_Actions>>();
                        }
                        catch { }
                    }
                    if (x.Actions == null) x.Actions = new List<Sys_Actions>();
                });

                string cacheVersion = CacheContext.Get(_menuCacheKey);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    CacheContext.Add(_menuCacheKey, cacheVersion);
                }
                else
                {
                    _menuVersionn = cacheVersion;
                }
            }
            return _menus;
        }

        /// <summary>
        /// 获取當前用户有權限查看的菜單
        /// </summary>
        /// <returns></returns>
        public List<Sys_Menu> GetCurrentMenuList()
        {
            var roleIds = UserContext.Current.IsSuperAdmin ? new int[] { 1 } : UserContext.Current.RoleIds ?? new int[] { };
            return GetUserMenuList(roleIds);
        }


        public List<Sys_Menu> GetUserMenuList(int[] roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return GetAllMenu();
            }
            List<int> menuIds = UserContext.Current.GetPermissions(roleId).Select(x => x.Menu_Id).ToList();
            return GetAllMenu().Where(x => menuIds.Contains(x.Menu_Id)).ToList();
        }

        /// <summary>
        /// 获取當前用户所有菜單與權限
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetCurrentMenuActionList()
        {
            return await GetMenuActionList(UserContext.Current.RoleIds);
        }

        /// <summary>
        /// 根據角色ID获取菜單與權限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<object> GetMenuActionList(int[] roleIds)
        {
            //2020.12.27增加菜單界面上不顯示，但可以分配權限
            if (UserContext.IsRoleIdSuperAdmin(roleIds))
            {
                return await Task.Run(() => GetAllMenu()
                .Where(c => c.MenuType == UserContext.MenuType)
                .Select(x =>
                new
                {
                    id = x.Menu_Id,
                    name = x.MenuName,
                    url = x.Url,
                    parentId = x.ParentId,
                    icon = x.Icon,
                    x.Enable,
                    x.LinkType,
                    x.TableName, // 2022.03.26增移動端加菜單類型
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).ToList());
            }

            var menu = from a in UserContext.Current.Permissions
                       join b in GetAllMenu().Where(c => c.MenuType == UserContext.MenuType)
                       on a.Menu_Id equals b.Menu_Id
                       orderby b.OrderNo descending
                       select new
                       {
                           id = a.Menu_Id,
                           name = b.MenuName,
                           url = b.Url,
                           parentId = b.ParentId,
                           icon = b.Icon,
                           b.Enable,
                           b.LinkType,
                           b.TableName, // 2022.03.26增移動端加菜單類型
                           permission = a.UserAuthArr
                       };
            return menu.ToList();
        }

        /// <summary>
        /// 新建或编輯菜單
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Save(Sys_Menu menu)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (menu == null) return webResponse.Error("没有获取到提交的参數");
            if (menu.Menu_Id > 0 && menu.Menu_Id == menu.ParentId) return webResponse.Error("父级ID不能是當前菜單的ID");
            try
            {
                webResponse = menu.ValidationEntity(x => new { x.MenuName, x.TableName });
                if (!webResponse.Status) return webResponse;
                if (menu.TableName != "/" && menu.TableName != ".")
                {
                    // 2022.03.26增移動端加菜單類型判断
                    Sys_Menu sysMenu = await repository.FindAsyncFirst(x => x.TableName == menu.TableName);
                    if (sysMenu != null)
                    {
                        sysMenu.MenuType ??= 0;
                        if (sysMenu.MenuType == menu.MenuType)
                        {
                            if ((menu.Menu_Id > 0 && sysMenu.Menu_Id != menu.Menu_Id)
                            || menu.Menu_Id <= 0)
                            {
                                return webResponse.Error($"视图/表名【{menu.TableName}】已被其他菜單使用");
                            }
                        }
                    }
                }
                bool _changed = false;
                if (menu.Menu_Id <= 0)
                {
                    repository.Add(menu.SetCreateDefaultVal());
                }
                else
                {
                    //2020.05.07新增禁止选择上级角色為自己
                    if (menu.Menu_Id == menu.ParentId)
                    {
                        return webResponse.Error($"父级id不能為自己");
                    }
                    if (repository.Exists(x => x.ParentId == menu.Menu_Id && menu.ParentId == x.Menu_Id))
                    {
                        return webResponse.Error($"不能选择此父级id，选择的父级id與當前菜單形成依赖关系");
                    }

                    _changed = repository.FindAsIQueryable(c => c.Menu_Id == menu.Menu_Id).Select(s => s.Auth).FirstOrDefault() != menu.Auth;

                    repository.Update(menu.SetModifyDefaultVal(), p => new
                    {
                        p.ParentId,
                        p.MenuName,
                        p.Url,
                        p.Auth,
                        p.OrderNo,
                        p.Icon,
                        p.AuthData,
                        p.LinkType,
                        p.Enable,
                        p.MenuType,// 2022.03.26增移動端加菜單類型
                        p.TableName,
                        p.ModifyDate,
                        p.Modifier
                    });
                }
                await repository.SaveChangesAsync();

                CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                if (_changed)
                {
                    UserContext.Current.RefreshWithMenuActionChange(menu.Menu_Id);
                }
                _menus = null;
                webResponse.OK("保存成功", menu);
            }
            catch (Exception ex)
            {
                webResponse.Error(ex.Message+ex.StackTrace);
            }
            finally
            {
                Logger.Info($"表:{menu.TableName},菜單：{menu.MenuName},權限{menu.Auth},{(webResponse.Status ? "成功" : "失败")}{webResponse.Message}");
            }
            return webResponse;

        }

        public async Task<WebResponseContent> DelMenu(int menuId)
        {
            WebResponseContent webResponse = new WebResponseContent();

            if (await repository.ExistsAsync(x => x.ParentId == menuId))
            {
                return webResponse.Error("當前菜單存在子菜單,請先删除子菜單!");
            }
            repository.Delete(new Sys_Menu()
            {
                Menu_Id = menuId
            }, true);
            CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
            return webResponse.OK("删除成功");
        }
        /// <summary>
        /// 编輯菜單時，获取菜單信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<object> GetTreeItem(int menuId)
        {
            var sysMenu = (await base.repository.FindAsync(x => x.Menu_Id == menuId))
                .Select(
                p => new
                {
                    p.Menu_Id,
                    p.ParentId,
                    p.MenuName,
                    p.Url,
                    p.Auth,
                    p.OrderNo,
                    p.Icon,
                    p.AuthData,
                    p.LinkType,
                    p.Enable,
                    // 2022.03.26增移動端加菜單類型
                    MenuType = p.MenuType ?? 0,
                    p.CreateDate,
                    p.Creator,
                    p.TableName,
                    p.ModifyDate
                }).FirstOrDefault();
            return sysMenu;
        }
    }
}


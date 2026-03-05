using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core;
using VolPro.Core.Configuration;
using VolPro.Core.Controllers.Basic;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.Filters;
using VolPro.Core.ManageUser;
using VolPro.Core.UserManager;
using VolPro.Core.Utilities;
using VolPro.Entity.AttributeManager;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Sys.Repositories;
using VolPro.Sys.Services;

namespace VolPro.Sys.Controllers
{
    [Route("api/role")]
    public partial class Sys_RoleController
    {
        private readonly ISys_RoleService _service;//訪問業務代碼
        private readonly ISys_RoleRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_RoleController(
            ISys_RoleService service,
            ISys_RoleRepository repository,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("getCurrentTreePermission")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetCurrentTreePermission()
        {
            return Json(await Service.GetCurrentTreePermission());
        }

        [HttpPost, Route("getUserTreePermission")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetUserTreePermission(int roleId)
        {
            return Json(await Service.GetUserTreePermission(new int[] { roleId }));
        }

        [HttpPost, Route("savePermission")]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public async Task<IActionResult> SavePermission([FromBody] List<UserPermissions> userPermissions, int roleId)
        {
            return Json(await Service.SavePermission(userPermissions, roleId));
        }

        /// <summary>
        /// 获取當前角色下的所有角色 
        /// </summary>
        /// <returns></returns>

        [HttpPost, Route("getUserChildRoles")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public IActionResult GetUserChildRoles()
        {
            var roleIds = UserContext.Current.RoleIds;
            var data = RoleContext.GetAllChildren(UserContext.Current.RoleIds);

            if (UserContext.Current.IsSuperAdmin)
            {
                return Json(WebResponseContent.Instance.OK(null, data));
            }
            //不是超级管理，將自己的角色查出来，在树形菜單上作為根节點
            var self = Sys_RoleRepository.Instance.FindAsIQueryable(x => roleIds.Contains(x.Role_Id))
                 .Select(s => new RoleNodes()
                 {
                     Id = s.Role_Id,
                     ParentId = 0,//將自己的角色作為root节點
                     RoleName = s.RoleName
                 }).ToList();
            data.AddRange(self);
            return Json(WebResponseContent.Instance.OK(null, data));
        }



        /// <summary>
        /// treetable 获取子节點數據(2021.05.02)
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpPost, Route("GetPageData")]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            //获取根节點數據(對應Sys_Role1.js中searchBefore方法)
            //if (loadData.Value.GetInt() == 1)
            //{
            // return GetTreeTableRootData(loadData).Result;
            //}
            return base.GetPageData(loadData);
        }

        /// <summary>
        /// treetable 获取子节點數據(2021.05.02)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableRootData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableRootData([FromBody] PageDataOptions options)
        {
            //页面加载根节點數據条件x => x.ParentId == 0,自己根據需要設置


            //页面加载根节點數據条件x => x.ParentId == 0,自己根據需要設置
            var dbServiceId = UserContext.CurrentServiceId;
            var query = _repository.FindAsIQueryable(x => true);
            if (UserContext.Current.IsSuperAdmin)
            {
                query = query.Where(x => x.ParentId == 0 || x.Role_Id == 1);
            }
            else
            {
                var roleIds = UserContext.Current.RoleIds;
                var roleList = RoleContext.GetRoles(x => roleIds.Contains(x.Id)).ToList();
                roleIds = roleList.Where(x => !roleList.Any(c => x.ParentId == c.Id)).Select(c => c.Id).ToArray();
                query = query.Where(x => roleIds.Contains(x.Role_Id));
                //顯示租户下的數據
                //query = query.WhereIF(AppSetting.UseDynamicShareDB, x => UserContext.Current.UserDbService.Select(c => c.DbServiceId).ToList().Contains((Guid)x.DbServiceId));
            }

            var queryChild = _repository.FindAsIQueryable(x => true);
            var rows = await query.TakeOrderByPage(options.Page, options.Rows)
                .OrderBy(x => x.Role_Id).Select(s => new
                {
                    s.Role_Id,
                    s.ParentId,
                    s.RoleName,
                    s.DeptName,
                    s.Dept_Id,
                    s.DbServiceId,
                    s.Enable,
                    s.CreateDate,
                    s.Creator,
                    s.Modifier,
                    s.ModifyDate,
                    s.OrderNo,
                    s.DatAuth,
                    hasChildren = queryChild.Any(x => x.ParentId == s.Role_Id)
                }).ToListAsync();
            return JsonNormal(new { total = await query.CountAsync(), rows });
        }

        /// <summary>
        ///treetable 获取子节點數據(2021.05.02)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableChildrenData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableChildrenData(int roleId)
        {
            if (!UserContext.Current.IsSuperAdmin && !UserContext.Current.RoleIds.Contains(roleId) && !RoleContext.GetAllChildrenIds(UserContext.Current.RoleIds).Any(x => x == roleId))
            {
                return JsonNormal(new { rows = new object[] { } });
            }
            //點击节點時，加载子节點數據
            var roleRepository = Sys_RoleRepository.Instance.FindAsIQueryable(x => true);
            var query = roleRepository.Where(x => x.ParentId == roleId);
            //顯示租户下的數據
            //if (AppSetting.UseDynamicShareDB)
            //{
            //    query = query.Where(x => x.DbServiceId == UserContext.CurrentServiceId);
            //}
            var rows = await query
                .Select(s => new
                {
                    s.Role_Id,
                    s.ParentId,
                    s.RoleName,
                    s.DeptName,
                    s.Dept_Id,
                    s.DbServiceId,
                    s.Enable,
                    s.CreateDate,
                    s.Creator,
                    s.Modifier,
                    s.ModifyDate,
                    s.OrderNo,
                    s.DatAuth,
                    hasChildren = roleRepository.Any(x => x.ParentId == s.Role_Id)
                }).ToListAsync();
            return JsonNormal(new { rows });
        }


        [HttpGet, Route("getAuthFields")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetAuthFields(int menuId, int roleId)
        {
            string table = Sys_MenuService.Instance.GetCurrentMenuList().Where(x => x.Menu_Id == menuId)
                 .Select(s => s.TableName).FirstOrDefault();

            var fields = await _repository.DbContext.Set<Sys_TableColumn>()
                   .Where(x => x.TableName == table && x.IsDisplay == 1)
                   .Select(s => new { field = s.ColumnName, name = s.ColumnCnName })
                   .ToListAsync();
            if (!UserContext.Current.IsSuperAdmin)
            {
                //获取用户當前拥有的字典
                var roleIds = UserContext.Current.RoleIds;
                var userFields = ((await _repository.DbContext.Set<Sys_RoleFields>()
                  .Where(x => roleIds.Contains(x.RoleId) && x.TableName == table)
                  .Select(s => s.AuthFields).FirstOrDefaultAsync()))?.Split(",") ?? new string[] { };
                if (userFields.Count() > 0)
                {
                    fields = fields.Where(x => userFields.Contains(x.field)).ToList();
                }
            }

            var roleFields = ((await _repository.DbContext.Set<Sys_RoleFields>()
                  .Where(x => x.RoleId == roleId && x.TableName == table)
                  .Select(s => s.AuthFields).FirstOrDefaultAsync()) ?? "")
                  .Split(",");

            return JsonNormal(new
            {
                fields,
                roleFields
            });

        }

        [HttpPost, Route("saveAuthFields")]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public async Task<ActionResult> SaveAuthFields([FromBody] string[] fields, int menuId, int roleId)
        {
            string table = Sys_MenuService.Instance.GetCurrentMenuList().Where(x => x.Menu_Id == menuId)
               .Select(s => s.TableName).FirstOrDefault();
            if (string.IsNullOrEmpty(table) || table == "." || table == "/")
            {
                return Json("保存成功".Translator());
            }
            string authFields = fields == null ? "" : string.Join(",", fields);

            var roleFields = await _repository.DbContext.Set<Sys_RoleFields>()
                 .Where(x => x.RoleId == roleId && x.TableName == table)
                 .FirstOrDefaultAsync();
            if (roleFields == null)
            {
                roleFields = new Sys_RoleFields()
                {
                    RoleId = roleId,
                    TableName = table,
                    Enable = 1,
                    AuthFields = authFields
                };
                _repository.DbContext.Set<Sys_RoleFields>().Add(roleFields.SetCreateDefaultVal());
            }
            else
            {
                roleFields.AuthFields = authFields;
                _repository.Update<Sys_RoleFields>(roleFields.SetModifyDefaultVal(), x => new { x.AuthFields });
            }
            _repository.SaveChanges();
            RoleContext.UpdateRoleFields(roleFields);
            return Json("保存成功".Translator());
        }
        /// <summary>
        /// 获取自定義數據權限字段
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost, Route("getFields")]
        public async Task<IActionResult> GetFields(int menuId, int roleId)
        {
            string table = Sys_MenuService.Instance.GetCurrentMenuList().Where(x => x.Menu_Id == menuId)
                .Select(s => s.TableName).FirstOrDefault();
            var data = await _repository.DbContext.Set<Sys_TableColumn>().Where(x => x.TableName == table)
                .OrderByDescending(x => x.OrderNo)
                 .Select(s => new { field = s.ColumnName, title = s.ColumnCnName, key = s.DropNo, type = s.EditType ?? s.ColumnType })
                 .ToListAsync();
            var authField = typeof(UserInfo).GetProperties()
                 .Where(s => s.GetTypeCustomValue<UserAuthFieldAttribute>(x => x.Name) != null)
                  .Select(s => new
                  {
                      field = s.Name,
                      name = s.GetTypeCustomValue<UserAuthFieldAttribute>(x => x.Name)
                  }).ToList();
            return Json(new { data, authField });
        }
    }
}



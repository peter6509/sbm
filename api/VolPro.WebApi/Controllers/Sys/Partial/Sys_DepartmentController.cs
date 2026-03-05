/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Sys_Department",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IServices;
using VolPro.Core.Filters;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Sys.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VolPro.Core.ManageUser;
using VolPro.Core.UserManager;

namespace VolPro.Sys.Controllers
{
    public partial class Sys_DepartmentController
    {
        private readonly ISys_DepartmentService _service;//訪問業務代碼
        private readonly ISys_DepartmentRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_DepartmentController(
            ISys_DepartmentService service,
             ISys_DepartmentRepository repository,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
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
            //if (loadData.Value.GetInt() == 1)
            //{
            //    return GetTreeTableRootData(loadData).Result;
            //}
            return base.GetPageData(loadData);
        }

        /// <summary>
        /// treetable 获取子节點數據
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableRootData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableRootData([FromBody] PageDataOptions options)
        {
            //页面加载根节點數據条件x => x.ParentId == 0,自己根據需要設置
            var query = _repository.FindAsIQueryable(x => true);
            if (UserContext.Current.IsSuperAdmin)
            {
                query = query.Where(x => x.ParentId == null);
            }
            else
            {
                var deptIds = UserContext.Current.DeptIds;
                var list = DepartmentContext.GetAllDept().Where(c => deptIds.Contains(c.id)).ToList();
                deptIds = list.Where(c=>!list.Any(x=>x.id==c.parentId)).Select(x => x.id).ToList();
                query = query.Where(x => deptIds.Contains(x.DepartmentId));
            }
            var queryChild = _repository.FindAsIQueryable(x => true);
            var rows = await query.TakeOrderByPage(options.Page, options.Rows)
                .OrderBy(x => x.DepartmentName).Select(s => new
                {
                    s.DepartmentId,
                    s.ParentId,
                    s.DepartmentName,
                    s.DepartmentCode,
                    s.Enable,
                    s.DepartmentType,
                    s.Remark,
                    s.CreateDate,
                    s.Creator,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = queryChild.Any(x => x.ParentId == s.DepartmentId)
                }).ToListAsync();
            return JsonNormal(new { total = await query.CountAsync(), rows });
        }

        /// <summary>
        ///treetable 获取子节點數據
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableChildrenData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableChildrenData(Guid departmentId)
        {
            //點击节點時，加载子节點數據
            var query = _repository.FindAsIQueryable(x => true);
            var rows = await query.Where(x => x.ParentId == departmentId)
                .Select(s => new
                {
                    s.DepartmentId,
                    s.ParentId,
                    s.DepartmentName,
                    s.DepartmentCode,
                    s.Enable,
                    s.DepartmentType,
                    s.Remark,
                    s.CreateDate,
                    s.Creator,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = query.Any(x => x.ParentId == s.DepartmentId)
                }).ToListAsync();
            return JsonNormal(new { rows });
        }
    }
}


/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Catalog",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.DbTest.IServices;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.Filters;
using VolPro.DbTest.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VolPro.DbTest.Controllers
{
    public partial class Demo_CatalogController
    {
        private readonly IDemo_CatalogService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        [ActivatorUtilitiesConstructor]
        public Demo_CatalogController(
            IDemo_CatalogService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 商品信息tree页面获取左边的tree的所有商品分類
        /// </summary>
        /// <returns></returns>
        [Route("getList"), HttpGet]
        public async Task<IActionResult> GetList()
        {
            var data = await Demo_CatalogRepository.Instance.FindAsIQueryable(x => true)
                  .Select(s => new
                  {
                      id = s.CatalogId,
                      s.ParentId,
                      name = s.CatalogName
                  })
                  .ToListAsync();
            return Json(data);
        }

        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            //没有查詢条件顯示所有一级节點數據
            if (loadData.Value.GetInt() == 1)
            {
                return GetCatalogRootData(loadData);
            }
            //有查詢条件使用框架默認的查詢方法
            return base.GetPageData(loadData);
        }

        /// <summary>
        /// treetable 获取根节點數據
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost, Route("getCatalogRootData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public ActionResult GetCatalogRootData([FromBody] PageDataOptions options)
        {
            //页面加载(一级)根节點數據条件x => x.ParentId==null,自己根據需要設置
            var query = Demo_CatalogRepository.Instance.FindAsIQueryable(x => x.ParentId == null);

            var rows = query.TakeOrderByPage(options.Page, options.Rows)
                .OrderBy(x => x.CatalogName).Select(s => new
                {
                    s.CatalogId,
                    s.CatalogName,
                    s.CatalogCode,
                    s.ParentId,
                    s.Img,
                    s.Enable,
                    s.Remark,
                    s.CreateID,
                    s.Creator,
                    s.CreateDate,
                    s.ModifyID,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = true
                }).ToList();
            return JsonNormal(new { total = query.Count(), rows });
        }


        /// <summary>
        ///treetable 获取子节點數據
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getChildrenData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetChildrenData(Guid catalogId)
        {
            //點击节點時，加载子节點數據
            var roleRepository = Demo_CatalogRepository.Instance.FindAsIQueryable(x => 1 == 1);

            var rows = await roleRepository.Where(x => x.ParentId == catalogId)
                .Select(s => new
                {
                    s.CatalogId,
                    s.CatalogName,
                    s.CatalogCode,
                    s.ParentId,
                    s.Img,
                    s.Enable,
                    s.Remark,
                    s.CreateID,
                    s.Creator,
                    s.CreateDate,
                    s.ModifyID,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = roleRepository.Any(x => x.ParentId == s.CatalogId)
                }).ToListAsync();
            return JsonNormal(new { rows });
        }
    }
}

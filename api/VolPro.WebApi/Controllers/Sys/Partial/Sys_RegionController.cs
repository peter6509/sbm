/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Sys_Region",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IServices;
using VolPro.Sys.IRepositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VolPro.Sys.Controllers
{
    public partial class Sys_RegionController
    {
        private readonly ISys_RegionService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_RegionRepository _regionRepository;
        [ActivatorUtilitiesConstructor]
        public Sys_RegionController(
            ISys_RegionService service,
            IHttpContextAccessor httpContextAccessor,
            ISys_RegionRepository regionRepository
        )
        : base(service)
        {
            _service = service;
            _regionRepository = regionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取省、市下面的所有數據
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet, Route("getList")]
        public async Task<IActionResult> GetList(int code)
        {
            return Json(await _regionRepository.FindAsIQueryable(x => x.parentId == code)
                  .Select(s => new
                  {
                      key = s.code,
                      value = s.name
                  }).ToListAsync());
        }

        /// <summary>
        /// 获取地图省、市數據
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet, Route("getMapList")]
        public async Task<IActionResult> GetMapList()
        {
            return Json(await _regionRepository.FindAsIQueryable(x => x.level == 1 || x.level == 2)
                  .Select(s => new
                  {
                      id = s.code,
                      s.level,
                      s.parentId,
                      s.name,
                      s.Lat,
                      s.Lng
                  }).ToListAsync());
        }
    }
}

/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("sbm_stockoutopen",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolPro.Entity.DomainModels;
using VolPro.sbm.IServices;

namespace VolPro.sbm.Controllers
{
    public partial class sbm_stockoutopenController
    {
        private readonly Isbm_stockoutopenService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public sbm_stockoutopenController(
            Isbm_stockoutopenService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        //[HttpPost, Route("GetPageData")]
        //[AllowAnonymous]
        //public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        //{
        //    return base.GetPageData(loadData);
        //}
    }
}

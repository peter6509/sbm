/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("sbm_stockmove",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.sbm.IServices;

namespace VolPro.sbm.Controllers
{
    public partial class sbm_stockmoveController
    {
        private readonly Isbm_stockmoveService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public sbm_stockmoveController(
            Isbm_stockmoveService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

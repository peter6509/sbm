/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("MES_QualityInspectionPlan",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.MES.IServices;

namespace VolPro.MES.Controllers
{
    public partial class MES_QualityInspectionPlanController
    {
        private readonly IMES_QualityInspectionPlanService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public MES_QualityInspectionPlanController(
            IMES_QualityInspectionPlanService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

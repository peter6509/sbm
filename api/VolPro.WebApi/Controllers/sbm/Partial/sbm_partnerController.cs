/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("sbm_partner",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core.BaseProvider;
using VolPro.Entity.DomainModels;
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;

namespace VolPro.sbm.Controllers
{
    public partial class sbm_partnerController
    {
        private readonly Isbm_partnerService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Isbm_partnerRepository _partnerRepository;

        [ActivatorUtilitiesConstructor]
        public sbm_partnerController(
            Isbm_partnerService service,
            IHttpContextAccessor httpContextAccessor,
            Isbm_partnerRepository isbm_PartnerRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _partnerRepository = isbm_PartnerRepository;
        }

        [Route("Getcontact"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Getcontact(int code)
        {

            return Json(await _partnerRepository.FindAsIQueryable(x => x.parent_id == code && x.partner_type ==2)
                .Select(s => new
                {
                    key = s.id,
                    value = s.name
                }).ToListAsync());
        }
        [HttpPost, Route("GetPageData")]
        [AllowAnonymous]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return base.GetPageData(loadData);
        }

    }
}

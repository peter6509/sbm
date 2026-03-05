/*
 *接口編寫處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("sbm_reqopen",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using VolPro.Entity.DomainModels;
using VolPro.sbm.IServices;
using VolPro.sbm.IRepositories;

namespace VolPro.sbm.Controllers
{
    public partial class sbm_reqopenController
    {
        private readonly Isbm_reqopenService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly Isbm_reqopenReposity _ReqopenReposity;
        //private readonly Isbm_sale_orderRepository _sale_orderRepository;

        [ActivatorUtilitiesConstructor]
        public sbm_reqopenController(
            Isbm_reqopenService service,
            IHttpContextAccessor httpContextAccessor
            //Isbm_reqopenReposity isbm_reqopenReposity, // 修正型別名稱及參數命名
            // Isbm_sale_orderRepository isbm_Sale_OrderRepository

        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            //_ReqopenReposity = isbm_reqopenReposity; // 修正指派
            //_sale_orderRepository = isbm_Sale_OrderRepository;
        }

        //[HttpPost, Route("GetPageData")]
        //[AllowAnonymous]
        //public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        //{
        //    return base.GetPageData(loadData);
        //}

        //[AllowAnonymous]
        //public override ActionResult Add([FromBody] SaveModel saveModel)
        //{
            //    // 1) 先檢查輸入
            //    if (saveModel?.MainData == null ||
            //        !saveModel.MainData.TryGetValue("so_id", out var nameObj) ||
            //        string.IsNullOrWhiteSpace(nameObj?.ToString()))
            //    {
            //        return Json(WebResponse.Error("報價單號為必填!"));
            //    }
            //    var name = nameObj?.ToString().Trim();

            //    // 2) 檢查是否重複（改為檢查 sf_project_instance 表）
            //    // 修正型別比對：sale_id 為 int，name 為 string，需嘗試轉型
            //    if (int.TryParse(name, out int saleIdValue) && _sale_orderRepository.Exists(x => x.sale_id == saleIdValue))
            //    {
            //        return Json(WebResponse.Error("報價單號已產生詢價單了!"));
            //    }

            //    // 3) 通過驗證才真正新增
        //    return base.Add(saveModel);
        //}

    }
}

public interface Isbm_reqopenReposity
{
    bool Exists(Func<dynamic, bool> predicate);
}

// 請確認 WebResponse 類別是否有 Error 方法，若無則可新增如下靜態方法
public static class WebResponse
{
    public static object Error(string message)
    {
        return new
        {
            Success = false,
            Message = message
        };
    }
}

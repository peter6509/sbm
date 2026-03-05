/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Order",Enums.ActionPermissionOptions.Search)]
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
using VolPro.Core.Filters;
using VolPro.DbTest.IRepositories;
using Microsoft.EntityFrameworkCore;
using VolPro.Core.Extensions;
using System.Linq;
using VolPro.Core.Tenancy;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace VolPro.DbTest.Controllers
{
    public partial class Demo_OrderController
    {
        private readonly IDemo_OrderService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IDemo_GoodsService _goodsService;//商品信息業務類

        private readonly IDemo_OrderListRepository _orderListRepository;
        private readonly IDemo_OrderRepository _orderRepository;
        [ActivatorUtilitiesConstructor]
        public Demo_OrderController(
            IDemo_OrderService service,
            IHttpContextAccessor httpContextAccessor,
            IDemo_GoodsService goodsService,
            IDemo_OrderListRepository orderListRepository,
            IDemo_OrderRepository orderRepository

        )
        : base(service)
        {
            _service = service;
            _orderRepository = orderRepository;
            _goodsService = goodsService;
            _orderListRepository = orderListRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return base.GetPageData(loadData);
        }

        [HttpGet, Route("test1")]
        public IActionResult Test1()
        {
            return Content("test1");
        }

        //批量选择获取明商品數據
        [Route("getGoods"), HttpPost]
        public IActionResult GetGoods([FromBody] PageDataOptions loadData)
        {
            //調用商品信息的查詢方法
            var gridData = _goodsService.GetPageData(loadData);

            return JsonNormal(gridData);
        }
        /// <summary>
        /// 获取訂單明细數據
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("getDetailRows"), HttpGet]
        public async Task<IActionResult> GetDetailRows(Guid Order_Id)
        {
            var rows = await _orderListRepository.FindAsIQueryable(x => x.Order_Id == Order_Id)
                  .ToListAsync();
            return JsonNormal(rows);
        }

        /// <summary>
        /// 获取訂單明细數據
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("submitAudit"), HttpPost]
        public IActionResult SubmitAudit([FromBody] object[] ids)
        {
            var result = Service.SubmitWorkFlowAudit(ids);
            return Json(result);
        }

        /// <summary>
        /// 获取訂單明细數據
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("getTotal"), HttpGet]
        public async Task<IActionResult> GetTotal()
        {
          //获取汇总
          var total=  await _orderRepository.FindAsIQueryable(x => true)
                  .GroupBy(x => true).
                  Select(x => new
                  {
                      orderType = -1,
                      count = x.Count(),
                      qty = x.Sum(c => c.TotalQty),
                      totalPrice = x.Sum(c => c.TotalPrice),
                  }).FirstAsync();
            //获取每個訂單類型數據
            var data = await _orderRepository.FindAsIQueryable(x => true)
                   .GroupBy(x => x.OrderType).
                   Select(x => new
                   {
                       orderType = x.Key,
                       count = x.Count(),
                       qty = x.Sum(c => c.TotalQty),
                       totalPrice = x.Sum(c => c.TotalPrice),
                   }).ToListAsync();

            List<object> list = new List<object>() { total };
            list.AddRange(data);

            return Json(list);
        }


    }
}

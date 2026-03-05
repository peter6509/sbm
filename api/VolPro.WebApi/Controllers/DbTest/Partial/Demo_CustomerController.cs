/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Demo_Customer",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.Entity.DomainModels;
using VolPro.DbTest.IServices;
using VolPro.DbTest.IRepositories;
using System.Linq;
using VolPro.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace VolPro.DbTest.Controllers
{
    public partial class Demo_CustomerController
    {
        private readonly IDemo_CustomerService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_CustomerRepository _repository;
        [ActivatorUtilitiesConstructor]
        public Demo_CustomerController(
            IDemo_CustomerService service,
            IHttpContextAccessor httpContextAccessor,
             IDemo_CustomerRepository repository
        )
        : base(service)
        {
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        //可以定義接口權限
        //[ApiActionPermission(ActionPermissionOptions.Search)]
        [Route("search"), HttpPost]
        public async Task<IActionResult> Search([FromBody] PageDataOptions loadData)
        {
            //loadData.Value是前端loadBefore方法設置的value值(输入框搜索的值)
            string value = loadData.Value?.ToString()?.Trim();

            //生成多個字段or查詢条件
            var query = _repository.WhereIF(!string.IsNullOrEmpty(value), x => x.Customer.Contains(value) || x.Remark.Contains(value));

            //返回數據數據必须包括rows與total屬性
            var data = new
            {
                rows = await query.OrderByDescending(x => x.Customer)
                            .TakePage(loadData.Page, loadData.Rows)
                            //返回的字段注意與前端配置的字段一致
                            .Select(s => new
                            {
                                s.Customer_Id,
                                s.Customer,
                                s.Province,
                                s.DetailAddress,
                                s.PhoneNo,
                                s.Remark
                            }).ToListAsync(),
                //返回总行數
                total = await query.CountAsync()
            };
            //注意前后端字段配置的大小写一致
            return JsonNormal(data);
        }
    }
}

/*
 *接口编写處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("Sys_QuartzOptions",Enums.ActionPermissionOptions.Search)]
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

namespace VolPro.Sys.Controllers
{
    public partial class Sys_QuartzOptionsController
    {
        private readonly ISys_QuartzOptionsService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_QuartzOptionsController(
            ISys_QuartzOptionsService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// api加上屬性 [ApiTask]
        /// </summary>
        /// <returns></returns>
        [ApiTask]
        [HttpGet, HttpPost, Route("test")]
        public IActionResult Test([FromBody] Dictionary<string, string> dic, string val)
        {
            Console.WriteLine(dic?.Serialize());
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }

        /// <summary>
        /// api加上屬性 [ApiTask]
        /// </summary>
        /// <returns></returns>
        [ApiTask]
        [HttpGet, HttpPost, Route("taskTest")]
        public IActionResult TaskTest()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }



        /// <summary>
        /// 手動執行一次
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("run"), HttpPost]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Run([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Run(taskOptions);
        }
        /// <summary>
        /// 開啟任務
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("start"), HttpPost]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Start([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Start(taskOptions);
        }

        /// <summary>
        /// 暂停任務
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("pause"), HttpPost]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Pause([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Pause(taskOptions);
        }
    }
}

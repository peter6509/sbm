/*
 *接口编寫處...
*如果接口需要做Action的權限驗証，請在Action上使用屬性
*如: [ApiActionPermission("sbm_sale_order",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using VolPro.Core.BaseProvider;
using VolPro.Entity.DomainModels;
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using MailKit.Net.Smtp; // 新增這一行

namespace VolPro.sbm.Controllers
{
    public partial class sbm_sale_orderController
    {
        private readonly Isbm_sale_orderService _service;//訪問業務代碼
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Isbm_sale_orderRepository _sale_orderRepository;

        [ActivatorUtilitiesConstructor]
        public sbm_sale_orderController(
            Isbm_sale_orderService service,
            IHttpContextAccessor httpContextAccessor,
            Isbm_sale_orderRepository isbm_Sale_OrderRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _sale_orderRepository = isbm_Sale_OrderRepository;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail(IFormFile file, string email)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("系統通知", "no-reply@your.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "報價單";

            var builder = new BodyBuilder
            {
                TextBody = "您好，附件為本次報價單。"
            };

            builder.Attachments.Add("quotation.pdf", stream.ToArray());
            message.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient(); // 使用 MailKit 的 SmtpClient
            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate("帳號", "密碼");
            client.Send(message);
            client.Disconnect(true);

            return Ok();
        }

        //[HttpPost, Route("GetPageData")]
        //[AllowAnonymous]
        //public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        //{
        //    try
        //    {
        //        return base.GetPageData(loadData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(401, new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}

        //[Route("getPtask"), HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetPTask(int proj, int level)
        //{

        //    return Json(await _repository.FindAsIQueryable(x => x.level_name == level - 1 && x.project_id == proj)
        //        .Select(s => new
        //        {
        //            key = s.sub_id,
        //            value = s.sub_no
        //        }).ToListAsync());
        //}
    }
}

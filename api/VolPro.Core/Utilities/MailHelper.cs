using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using VolPro.Core.Configuration;
using VolPro.Core.Extensions;

namespace VolPro.Core.Utilities
{
    public static class MailHelper
    {
        private static string address { get; set; }
        private static string authPwd { get; set; }
        private static string name { get; set; }
        private static string host { get; set; }
        private static int port;
        private static bool enableSsl { get; set; }
        static MailHelper()
        {
            IConfigurationSection section = AppSetting.GetSection("Mail");
            address = section["Address"];
            authPwd = section["AuthPwd"];
            name = section["Name"];
            host = section["Host"];
            port = section["Port"].GetInt();
            enableSsl = section["EnableSsl"].GetBool();
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="title">標题</param>
        /// <param name="content">内容</param>
        /// <param name="list">收件人</param>
        public static void Send(string title, string content, string[] list)
        {
            Send(title, content, false, null, list);
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="title">標题</param>
        /// <param name="content">内容</param>
        /// <param name="attachmentPath">附件</param>
        /// <param name="list">收件人</param>
        public static void Send(string title, string content, string attachmentPath, string[] list)
        {
            Send(title, content, false, attachmentPath, list);
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="title">標题</param>
        /// <param name="content">内容</param>
        /// <param name="IsBodyHtml">是否html</param>
        /// <param name="list">收件人</param>
        public static void Send(string title, string content, bool IsBodyHtml, string[] list)
        {
            Send(title, content, IsBodyHtml, null, list);
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="title">標题</param>
        /// <param name="content">内容</param>
        /// <param name="IsBodyHtml">是否html</param>
        /// <param name="attachmentPath">附件</param>
        /// <param name="list">收件人</param>
        public static void Send(string title, string content, bool IsBodyHtml, string attachmentPath, params string[] list)
        {
            //Console.WriteLine(AppSetting.GetSection("ModifyMember")["DateUTCField"]);
            MailMessage message = new MailMessage
            {
                From = new MailAddress(address, name)//發送人郵箱
            };
            foreach (var item in list)
            {
                message.To.Add(item);//收件人地址
            }

            message.Subject = title;//發送郵件的標题

            message.Body = content;//發送郵件的内容
            message.IsBodyHtml = IsBodyHtml;
            // 添加附件
            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                Attachment attachment = new Attachment(attachmentPath);
                message.Attachments.Add(attachment);
            }
            //配置smtp服務地址
            using SmtpClient client = new SmtpClient
            {
                Host = host,
                Port = port,//端口587
                EnableSsl = enableSsl,
                //發送人郵箱與授權密碼
                Credentials = new NetworkCredential(address, authPwd)
            };
            client.Send(message);
        }
    }
}

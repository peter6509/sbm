using System;
using System.Collections.Generic;
using System.Text;

namespace VolPro.Core.Enums
{
    public struct ApiMessage
    {

        /// <summary>
        /// 参數有误
        /// </summary>
        public const string ParameterError = "請求参數不正确!";
        /// <summary>
        /// 没有配置好输入参數
        /// </summary>
        public const string NotInputEntity = "没有配置好输入参數!";
        /// <summary>
        /// token丢失
        /// </summary>
        public const string TokenLose = "token丢失!";

        /// <summary>
        /// 版本號不能為空
        /// </summary>

        public const string VersionEmpty = "版本號不能為空!";
        /// <summary>
        /// content不能為空
        /// </summary>

        public const string ContentEmpty = "biz_content不能為空!";
        /// <summary>
        /// content不能為空
        /// </summary>
        public const string TokenError = "token不正确";

        public const string AccountLocked = "帳號已被锁定!";

        public const string PhoneNoInvalid = "输入的不是手機號";


        public const string PINTypeNotRange= "获取驗証的類型不正确";
        public const string OperToBusy = "操作太频繁，請稍后再试";

        public const string SendSTKError = "短信發送异常,請稍后再试";
        public const string SendSTKSuccess = "短信發送成功";
        public const string STKNotSend = "請先获取驗証碼";
        public const string AccountExists = "手機號已經被注册";

        public const string AccountNotExists = "手機號没有注册";

        public const string PINExpire = "驗証碼已過期,請重新获取";

        public const string PINError = "驗証碼不正确";

        public const string ParameterEmpty = "参數不能為空";
    }
}

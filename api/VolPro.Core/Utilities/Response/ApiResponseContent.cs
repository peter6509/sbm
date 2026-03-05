using Newtonsoft.Json;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;

namespace VolPro.Core.Utilities
{
    public class ApiResponseContent
    {
        public ApiResponseContent()
        {
        }
        public ApiResponseContent(int status)
        {
            this.Status = status;
        }
        [JsonProperty(PropertyName = "message")]
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        [JsonProperty(PropertyName = "status")]
        /// <summary>
        /// 返回状態
        /// </summary>
        public int Status { get; set; }
        [JsonProperty(PropertyName = "data")]
        /// <summary>
        /// 所有返回的業務數據
        /// </summary>
        public object Data { get; set; }

        public ApiResponseContent OK(string msg = null)
        {
            return this.Set(ResponseType.OperSuccess, msg, ApiStatutsCode.Ok);
        }

        public ApiResponseContent Set(ResponseType responseType, ApiStatutsCode? status = null)
        {
            return this.Set(responseType, null, status);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="responseType">返回消息類型</param>
        /// <param name="msg">返回消息，若msg為null,则取responseType的描述信息</param>
        /// <param name="status">返回状態，目前只有0、失败，1、成功，2、token過期</param>
        public ApiResponseContent Set(ResponseType responseType, string msg, ApiStatutsCode? status = null)
        {
            if (status != null)
                this.Status = (int)status;
            if (!string.IsNullOrEmpty(msg))
            {
                this.Message = msg;
                return this;
            }
            if (!string.IsNullOrEmpty(this.Message))
                return this;
            this.Message = responseType.GetMsg();
            return this;
        }
    }
}

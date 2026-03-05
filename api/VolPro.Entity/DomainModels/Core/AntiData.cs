using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolPro.Entity.DomainModels
{
    public class AntiData
    {
        /// <summary>
        /// 審批的主鍵
        /// </summary>
        public object Key { get; set; }
        /// <summary>
        /// 審批意见
        /// </summary>
        public string AuditReason { get; set; }

        /// <summary>
        /// 是否審批流程
        /// </summary>
        public bool IsFlow { get; set; }

        /// <summary>
        /// 退回节點
        /// </summary>
        public string StepId { get; set; }
    }
}

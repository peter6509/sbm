using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolPro.Core.Print
{
    public class PrintQuery
    {
        public object[] Ids { get; set; }
        public Guid TemplateId { get; set; }
        /// <summary>
        /// 打印模板名稱
        /// </summary>
        public string TemplateName { get; set; }
        public string Table { get; set; }

        /// <summary>
        /// 是否批量打印主表與明细表數據
        /// </summary>
        public bool BatchMainAndDetail { get; set; }

        /// <summary>
        /// 是否批量打印主表數據
        /// </summary>
        public bool BatchMain { get; set; }

        /// <summary>
        /// 是否打印明细表數據
        /// </summary>
        public bool Detail { get; set; }

        /// <summary>
        /// 自定義参數
        /// </summary>
        public string Options { get; set; }
    }
}

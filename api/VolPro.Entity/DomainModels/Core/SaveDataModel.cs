
using System.Collections;
using System.Collections.Generic;

namespace VolPro.Entity.DomainModels
{
    public class SaveModel
    {
        public Dictionary<string, object> MainData { get; set; }
        public List<Dictionary<string, object>> DetailData { get; set; }
        public List<object> DelKeys { get; set; }

        /// <summary>
        /// 从前台傳入的其他参數(自定義扩展可以使用)
        /// </summary>
        public object Extra { get; set; }

        /// <summary>
        /// 一對多明细
        /// </summary>
        public List<DetailInfo> Details { get; set; }

        public List<SubDelInfo> SubDelInfo { get; set; }

        /// <summary>
        /// 2024.01.22
        /// 是否審批流程
        /// </summary>
        public bool IsFlow { get; set; }

        //2024.06.10增加數據版本號管理
        public string DataVersionField { get; set; }
        public string DataVersionValue { get; set; }
    }

    public class DetailInfo
    {
        public string Table { get; set; }

        public List<Dictionary<string, object>> Data { get; set; }
        public List<object> DelKeys { get; set; }
    }

    public class SubDelInfo
    {
        public bool IsProescc { get; set; }
        public string Table { get; set; }
        public List<object> DelKeys { get; set; }
    }
}

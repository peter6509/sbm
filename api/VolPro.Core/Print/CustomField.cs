using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolPro.Core.Print
{
    /// <summary>
    /// 自定義字段，某些字段不在當前表，可以预先自定義字段，在PrintCustom類QueryResult字自定義返回這些字段的值
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// 列顯示名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }
    }
}

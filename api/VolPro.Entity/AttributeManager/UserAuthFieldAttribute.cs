using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolPro.Entity.AttributeManager
{
    public class UserAuthFieldAttribute : Attribute
    {
        /// <summary>
        /// 數據權限要過濾的名稱
        /// </summary>
        public string Name { get; set; }
    }
}

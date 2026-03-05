using System;

namespace VolPro.Entity.AttributeManager
{
    public class PermissionTableAttribute : Attribute
    {
        /// <summary>
        /// 需要控制權限的表名與Sys_Menu表的表名必须一致
        /// </summary>
        public string Name { get; set; }
    }
}

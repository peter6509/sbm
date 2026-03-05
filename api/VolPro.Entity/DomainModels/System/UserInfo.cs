using VolPro.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Entity.AttributeManager;

namespace VolPro.Entity.DomainModels
{
    /// <summary>
    /// 字段上標記UserAuth屬性后在角色自定義數據權限可以选择
    /// </summary>
    public class UserInfo
    {
        [UserAuthField(Name = "用户ID")]
        public int User_Id { get; set; }

        /// <summary>
        /// 多個角色ID
        /// </summary>
        public int Role_Id { get; set; }

        public string RoleName { get; set; }

        [UserAuthField(Name = "用户帳號")]
        public string UserName { get; set; }

        [UserAuthField(Name = "用户姓名")]
        public string UserTrueName { get; set; }

        /// <summary>
        /// 岗位id
        /// </summary>
        public List<Guid> PostIds { get; set; }

        [UserAuthField(Name = "用户部门")]
        public List<Guid> DeptIds { get; set; }

        [UserAuthField(Name = "用户角色")]
        public int[] RoleIds { get; set; }

        public string Token { get; set; }

        public int Enable { get; set; }
        public string TenancyValue { get; set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolPro.Entity.DomainModels
{
    public class Permissions
    {
        public int Menu_Id { get; set; }
        public int ParentId { get; set; }
        public string TableName { get; set; }
        public string MenuAuth { get; set; }
        public string UserAuth { get; set; }
        /// <summary>
        /// 當前用户權限,存储的是權限的值，如:Add,Search等
        /// </summary>
        public string[] UserAuthArr { get; set; }

        /// <summary>
        /// 2022.03.26
        /// 菜單類型1:移動端，0:PC端
        /// </summary>
        public int MenuType { get; set; }


        /// <summary>
        ///菜單數據權限
        ///2024.08.11
        /// </summary>
        [Display(Name = "菜單數據權限")]
        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string AuthMenuData { get; set; }

        private List<Dictionary<string, string>> _customAuth = null;
        public List<Dictionary<string, string>> CustomAuth
        {
            get
            {
                if (string.IsNullOrEmpty(AuthMenuData) || AuthMenuData[0]!='[')
                {
                    return null;
                }
                if (_customAuth == null)
                {
                    _customAuth = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(AuthMenuData);
                }
                return _customAuth;
            }
        }
    }
}

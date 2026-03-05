using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VolPro.Entity.DomainModels
{
    public class LoginInfo
    {


        [Display(Name = "用户名")]
        [MaxLength(50)]
        [Required(ErrorMessage = "用户名不能為空")]
        public string UserName { get; set; }
        [MaxLength(50)]
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; }
        [MaxLength(6)]
        [Display(Name = "驗証碼")]
        [Required(ErrorMessage = "驗証碼不能為空")]
        public string VerificationCode { get; set; }
        [Required(ErrorMessage = "参數不完整")]
        /// <summary>
        /// 2020.06.12增加驗証碼
        /// </summary>
        public string UUID { get; set; }
    }
}

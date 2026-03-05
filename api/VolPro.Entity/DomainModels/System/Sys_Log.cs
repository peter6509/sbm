/*
 *Author：jxx
 *Contact：283591387@qq.com
 *Date：2018-07-01
 * 此代碼由框架生成，請勿随意更改
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Entity;
using VolPro.Entity.SystemModels;

namespace VolPro.Entity.DomainModels
{
    [Table("Sys_Log")]
    [EntityAttribute(TableCnName = "系统日志")]
    public class Sys_Log:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///開始時间
       /// </summary>
       [Display(Name ="開始時间")]
       [Column(TypeName="datetime")]
       public DateTime? BeginDate { get; set; }

       /// <summary>
       ///請求地址
       /// </summary>
       [Display(Name ="請求地址")]
       [MaxLength(30000)]
       [Column(TypeName="varchar(30000)")]
       public string Url { get; set; }

       /// <summary>
       ///日志類型
       /// </summary>
       [Display(Name ="日志類型")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string LogType { get; set; }

       /// <summary>
       ///响應状態
       /// </summary>
       [Display(Name ="响應状態")]
       [Column(TypeName="int")]
       public int? Success { get; set; }

       /// <summary>
       ///時长(毫秒)
       /// </summary>
       [Display(Name ="時长(毫秒)")]
       [Column(TypeName="int")]
       public int? ElapsedTime { get; set; }

       /// <summary>
       ///請求参數
       /// </summary>
       [Display(Name ="請求参數")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string RequestParameter { get; set; }

       /// <summary>
       ///响應参數
       /// </summary>
       [Display(Name ="响應参數")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string ResponseParameter { get; set; }

       /// <summary>
       ///异常信息
       /// </summary>
       [Display(Name ="异常信息")]
       [MaxLength(10000)]
       [Column(TypeName="nvarchar(10000)")]
       public string ExceptionInfo { get; set; }

       /// <summary>
       ///用户IP
       /// </summary>
       [Display(Name ="用户IP")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string UserIP { get; set; }

       /// <summary>
       ///服務器IP
       /// </summary>
       [Display(Name ="服務器IP")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string ServiceIP { get; set; }

       /// <summary>
       ///浏览器類型
       /// </summary>
       [Display(Name ="浏览器類型")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string BrowserType { get; set; }

       /// <summary>
       ///用户ID
       /// </summary>
       [Display(Name ="用户ID")]
       [Column(TypeName="int")]
       public int? User_Id { get; set; }

       /// <summary>
       ///用户名稱
       /// </summary>
       [Display(Name ="用户名稱")]
       [MaxLength(30000)]
       [Column(TypeName="varchar(30000)")]
       public string UserName { get; set; }

       /// <summary>
       ///角色ID
       /// </summary>
       [Display(Name ="角色ID")]
       [Column(TypeName="int")]
       public int? Role_Id { get; set; }

       /// <summary>
       ///结束時间
       /// </summary>
       [Display(Name ="结束時间")]
       [Column(TypeName="datetime")]
       public DateTime? EndDate { get; set; }

       
    }
}

/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果數據庫字段發生变化，請在代碼生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Entity.SystemModels;

namespace VolPro.Entity.DomainModels
{
    [Entity(TableCnName = "定時任務",TableName = "Sys_QuartzOptions",DBServer = "SysDbContext")]
    public partial class Sys_QuartzOptions:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid Id { get; set; }

       /// <summary>
       ///任務名稱
       /// </summary>
       [Display(Name ="任務名稱")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string TaskName { get; set; }

       /// <summary>
       ///任務分组
       /// </summary>
       [Display(Name ="任務分组")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string GroupName { get; set; }

       /// <summary>
       ///請求方式
       /// </summary>
       [Display(Name ="請求方式")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string Method { get; set; }

       /// <summary>
       ///超時時间(秒)
       /// </summary>
       [Display(Name ="超時時间(秒)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? TimeOut { get; set; }

       /// <summary>
       ///Corn表达式
       /// </summary>
       [Display(Name ="Corn表达式")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string CronExpression { get; set; }

       /// <summary>
       ///Url地址
       /// </summary>
       [Display(Name ="Url地址")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string ApiUrl { get; set; }

       /// <summary>
       ///post参數
       /// </summary>
       [Display(Name ="post参數")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string PostData { get; set; }

       /// <summary>
       ///AuthKey
       /// </summary>
       [Display(Name ="AuthKey")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string AuthKey { get; set; }

       /// <summary>
       ///AuthValue
       /// </summary>
       [Display(Name ="AuthValue")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string AuthValue { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string Describe { get; set; }

       /// <summary>
       ///最后執行執行
       /// </summary>
       [Display(Name ="最后執行執行")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? LastRunTime { get; set; }

       /// <summary>
       ///运行状態
       /// </summary>
       [Display(Name ="运行状態")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///創建時间
       /// </summary>
       [Display(Name ="創建時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時间
       /// </summary>
       [Display(Name ="修改時间")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       
    }
}
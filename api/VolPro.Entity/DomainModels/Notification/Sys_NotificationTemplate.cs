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
    [Entity(TableCnName = "消息通知模板",TableName = "Sys_NotificationTemplate",DBServer = "SysDbContext")]
    public partial class Sys_NotificationTemplate:SysEntity
    {
        /// <summary>
       ///主鍵ID
       /// </summary>
       [Key]
       [Display(Name ="主鍵ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid NotificationTemplateId { get; set; }

       /// <summary>
       ///業務表
       /// </summary>
       [Display(Name ="業務表")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string TableName { get; set; }

       /// <summary>
       ///業務表主鍵id
       /// </summary>
       [Display(Name ="業務表主鍵id")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string TableKey { get; set; }

       /// <summary>
       ///通知標题
       /// </summary>
       [Display(Name ="通知標题")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string NotificationTitle { get; set; }

       /// <summary>
       ///通知编碼
       /// </summary>
       [Display(Name ="通知编碼")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string NotificationCode { get; set; }

       /// <summary>
       ///業務功能
       /// </summary>
       [Display(Name ="業務功能")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string BusinessFunction { get; set; }

       /// <summary>
       ///通知類型
       /// </summary>
       [Display(Name ="通知類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string NotificationType { get; set; }

       /// <summary>
       ///通知级别
       /// </summary>
       [Display(Name ="通知级别")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string NotificationLevel { get; set; }

       /// <summary>
       ///通知對象類型
       /// </summary>
       [Display(Name ="通知對象類型")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string TargetObjectType { get; set; }

       /// <summary>
       ///通知對象
       /// </summary>
       [Display(Name ="通知對象")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string TargetObjectValue { get; set; }

       /// <summary>
       ///通知對象
       /// </summary>
       [Display(Name ="通知對象")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string TargetObjectText { get; set; }

       /// <summary>
       ///字段模板
       /// </summary>
       [Display(Name ="字段模板")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? IsFieldTemplate { get; set; }

       /// <summary>
       ///發布状態
       /// </summary>
       [Display(Name ="發布状態")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? PublishStatus { get; set; }

       /// <summary>
       ///通知内容
       /// </summary>
       [Display(Name ="通知内容")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string NotificationContent { get; set; }

       /// <summary>
       ///跳转地址
       /// </summary>
       [Display(Name ="跳转地址")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string LinkUrl { get; set; }

       /// <summary>
       ///跳转類型
       /// </summary>
       [Display(Name ="跳转類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string LinkType { get; set; }

       /// <summary>
       ///備注
       /// </summary>
       [Display(Name ="備注")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Enable")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Enable { get; set; }

       /// <summary>
       ///創建人ID
       /// </summary>
       [Display(Name ="創建人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///創建時间
       /// </summary>
       [Display(Name ="創建時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateDate { get; set; }

       /// <summary>
       ///修改人Id
       /// </summary>
       [Display(Name ="修改人Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時间
       /// </summary>
       [Display(Name ="修改時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       
    }
}
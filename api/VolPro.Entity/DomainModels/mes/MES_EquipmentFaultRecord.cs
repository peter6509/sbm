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
    [Entity(TableCnName = "設備故障",DBServer = "ServiceDbContext")]
    public partial class MES_EquipmentFaultRecord:ServiceEntity
    {
        /// <summary>
       ///故障記錄ID
       /// </summary>
       [Key]
       [Display(Name ="故障記錄ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid FaultRecordID { get; set; }

       /// <summary>
       ///設備ID
       /// </summary>
       [Display(Name ="設備ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? EquipmentID { get; set; }

       /// <summary>
       ///故障日期
       /// </summary>
       [Display(Name ="故障日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? FaultDate { get; set; }

       /// <summary>
       ///故障類型
       /// </summary>
       [Display(Name ="故障類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string FaultType { get; set; }

       /// <summary>
       ///故障描述
       /// </summary>
       [Display(Name ="故障描述")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string FaultDescription { get; set; }

       /// <summary>
       ///故障影响
       /// </summary>
       [Display(Name ="故障影响")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string FaultImpact { get; set; }

       /// <summary>
       ///故障報告人
       /// </summary>
       [Display(Name ="故障報告人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string FaultReportedBy { get; set; }

       /// <summary>
       ///故障状態
       /// </summary>
       [Display(Name ="故障状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string FaultStatus { get; set; }

       /// <summary>
       ///故障排查開始時间
       /// </summary>
       [Display(Name ="故障排查開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? TroubleshootingStartTime { get; set; }

       /// <summary>
       ///故障排查结束時间
       /// </summary>
       [Display(Name ="故障排查结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? TroubleshootingEndTime { get; set; }

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
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
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
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
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
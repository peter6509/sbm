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
    [Entity(TableCnName = "变更記錄",TableName = "MES_ProductionPlanChangeRecord",DBServer = "ServiceDbContext")]
    public partial class MES_ProductionPlanChangeRecord:ServiceEntity
    {
        /// <summary>
       ///变更記錄ID
       /// </summary>
       [Key]
       [Display(Name ="变更記錄ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid ChangeRecordID { get; set; }

       /// <summary>
       ///计划明细ID
       /// </summary>
       [Display(Name ="计划明细ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? PlanDetailID { get; set; }

       /// <summary>
       ///訂單编號
       /// </summary>
       [Display(Name ="訂單编號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string OrderNumber { get; set; }

       /// <summary>
       ///客户名稱
       /// </summary>
       [Display(Name ="客户名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string CustomerName { get; set; }

       /// <summary>
       ///訂單日期
       /// </summary>
       [Display(Name ="訂單日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? OrderDate { get; set; }

       /// <summary>
       ///变更日期
       /// </summary>
       [Display(Name ="变更日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime ChangeDate { get; set; }

       /// <summary>
       ///原计划數量
       /// </summary>
       [Display(Name ="原计划數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int OriginalPlanQuantity { get; set; }

       /// <summary>
       ///新计划數量
       /// </summary>
       [Display(Name ="新计划數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int NewPlanQuantity { get; set; }

       /// <summary>
       ///原计划開始時间
       /// </summary>
       [Display(Name ="原计划開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? OriginalPlannedStartTime { get; set; }

       /// <summary>
       ///新计划開始時间
       /// </summary>
       [Display(Name ="新计划開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? NewPlannedStartTime { get; set; }

       /// <summary>
       ///原计划结束時间
       /// </summary>
       [Display(Name ="原计划结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? OriginalPlannedEndTime { get; set; }

       /// <summary>
       ///新计划结束時间
       /// </summary>
       [Display(Name ="新计划结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? NewPlannedEndTime { get; set; }

       /// <summary>
       ///变更原因
       /// </summary>
       [Display(Name ="变更原因")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string ChangeReason { get; set; }

       /// <summary>
       ///变更人
       /// </summary>
       [Display(Name ="变更人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string ChangedBy { get; set; }

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
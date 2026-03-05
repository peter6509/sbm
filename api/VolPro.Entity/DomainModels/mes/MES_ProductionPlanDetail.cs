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
    [Entity(TableCnName = "訂單明细",TableName = "MES_ProductionPlanDetail",DBServer = "ServiceDbContext")]
    public partial class MES_ProductionPlanDetail:ServiceEntity
    {
        /// <summary>
       ///计划明细ID
       /// </summary>
       [Key]
       [Display(Name ="计划明细ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid PlanDetailID { get; set; }

       /// <summary>
       ///訂單ID
       /// </summary>
       [Display(Name ="訂單ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? OrderID { get; set; }

       /// <summary>
       ///物料编碼
       /// </summary>
       [Display(Name ="物料编碼")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string MaterialCode { get; set; }

       /// <summary>
       ///物料名稱
       /// </summary>
       [Display(Name ="物料名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string MaterialName { get; set; }

       /// <summary>
       ///物料规格
       /// </summary>
       [Display(Name ="物料规格")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string MaterialSpecification { get; set; }

       /// <summary>
       ///單位
       /// </summary>
       [Display(Name ="單位")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Unit { get; set; }

       /// <summary>
       ///訂單數量
       /// </summary>
       [Display(Name ="訂單數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? PlanQuantity { get; set; }

       /// <summary>
       ///计划開始時间
       /// </summary>
       [Display(Name ="计划開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? PlannedStartTime { get; set; }

       /// <summary>
       ///计划结束時间
       /// </summary>
       [Display(Name ="计划结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? PlannedEndTime { get; set; }

       /// <summary>
       ///计划状態
       /// </summary>
       [Display(Name ="计划状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string PlanStatus { get; set; }

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
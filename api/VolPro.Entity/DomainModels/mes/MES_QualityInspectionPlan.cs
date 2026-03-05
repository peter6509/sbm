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
    [Entity(TableCnName = "质量檢驗",TableName = "MES_QualityInspectionPlan",DetailTable =  new Type[] { typeof(MES_QualityInspectionPlanDetail)},DetailTableCnName = "质檢明细",DBServer = "ServiceDbContext")]
    public partial class MES_QualityInspectionPlan:ServiceEntity
    {
        /// <summary>
       ///檢驗计划ID
       /// </summary>
       [Key]
       [Display(Name ="檢驗计划ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)] 
       public Guid InspectionPlanID { get; set; }

       /// <summary>
       ///檢驗單號
       /// </summary>
       [Display(Name ="檢驗單號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string InspectionPlanNumber { get; set; }

       /// <summary>
       ///訂單ID
       /// </summary>
       [Display(Name ="訂單ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? OrderID { get; set; }

       /// <summary>
       ///檢驗開始時间
       /// </summary>
       [Display(Name ="檢驗開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime PlanStartTime { get; set; }

       /// <summary>
       ///檢驗结束時间
       /// </summary>
       [Display(Name ="檢驗结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime PlanEndTime { get; set; }

       /// <summary>
       ///檢驗人
       /// </summary>
       [Display(Name ="檢驗人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string ResponsiblePerson { get; set; }

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

       [Display(Name ="质檢明细")]
       [ForeignKey("InspectionPlanID")]
       public List<MES_QualityInspectionPlanDetail> MES_QualityInspectionPlanDetail { get; set; }


       
    }
}
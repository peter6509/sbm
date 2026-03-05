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
    [Entity(TableCnName = "质檢記錄",DBServer = "ServiceDbContext")]
    public partial class MES_QualityInspectionRecord:ServiceEntity
    {
        /// <summary>
       ///檢驗記錄ID
       /// </summary>
       [Key]
       [Display(Name ="檢驗記錄ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid InspectionRecordID { get; set; }

       /// <summary>
       ///檢驗计划明细ID
       /// </summary>
       [Display(Name ="檢驗计划明细ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? InspectionPlanDetailID { get; set; }

       /// <summary>
       ///檢驗單號
       /// </summary>
       [Display(Name ="檢驗單號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string InspectionNumber { get; set; }

       /// <summary>
       ///檢驗員
       /// </summary>
       [Display(Name ="檢驗員")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Inspector { get; set; }

       /// <summary>
       ///檢驗時间
       /// </summary>
       [Display(Name ="檢驗時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime InspectionTime { get; set; }

       /// <summary>
       ///實際檢驗數量
       /// </summary>
       [Display(Name ="實際檢驗數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int InspectedQuantity { get; set; }

       /// <summary>
       ///合格數量
       /// </summary>
       [Display(Name ="合格數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int PassedQuantity { get; set; }

       /// <summary>
       ///不合格數量
       /// </summary>
       [Display(Name ="不合格數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int FailedQuantity { get; set; }

       /// <summary>
       ///檢驗结果（合格、不合格）
       /// </summary>
       [Display(Name ="檢驗结果（合格、不合格）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string InspectionResult { get; set; }

       /// <summary>
       ///缺陷描述
       /// </summary>
       [Display(Name ="缺陷描述")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string DefectDescription { get; set; }

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
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
    [Entity(TableCnName = "BOM明细",TableName = "MES_Bom_Detail",DBServer = "ServiceDbContext")]
    public partial class MES_Bom_Detail:ServiceEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid DomDetailId { get; set; }

       /// <summary>
       ///BomId
       /// </summary>
       [Display(Name ="BomId")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? BomId { get; set; }

       /// <summary>
       ///序號
       /// </summary>
       [Display(Name ="序號")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Sequence { get; set; }

       /// <summary>
       ///子件物料编碼
       /// </summary>
       [Display(Name ="子件物料编碼")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string MaterialCode { get; set; }

       /// <summary>
       ///子件物料名稱
       /// </summary>
       [Display(Name ="子件物料名稱")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       public string MaterialName { get; set; }

       /// <summary>
       ///规格型號
       /// </summary>
       [Display(Name ="规格型號")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string Spec { get; set; }

       /// <summary>
       ///單台用量
       /// </summary>
       [Display(Name ="單台用量")]
       [DisplayFormat(DataFormatString="24,3")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal UsageQty { get; set; }

       /// <summary>
       ///消耗方式
       /// </summary>
       [Display(Name ="消耗方式")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string ConsumeModel { get; set; }

       /// <summary>
       ///投料仓庫
       /// </summary>
       [Display(Name ="投料仓庫")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Warehouse { get; set; }

       /// <summary>
       ///供應商
       /// </summary>
       [Display(Name ="供應商")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string SupplierCode { get; set; }

       /// <summary>
       ///齐套比例
       /// </summary>
       [Display(Name ="齐套比例")]
       [DisplayFormat(DataFormatString="24,3")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? KitScale { get; set; }

       /// <summary>
       ///啟用状態
       /// </summary>
       [Display(Name ="啟用状態")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Enable { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
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
       ///更新人
       /// </summary>
       [Display(Name ="更新人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       /// <summary>
       ///更新人名稱
       /// </summary>
       [Display(Name ="更新人名稱")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Modifier { get; set; }

       /// <summary>
       ///更新時间
       /// </summary>
       [Display(Name ="更新時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       
    }
}
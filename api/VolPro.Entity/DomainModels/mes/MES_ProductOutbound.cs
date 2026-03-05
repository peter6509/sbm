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
    [Entity(TableCnName = "產品出庫",DBServer = "ServiceDbContext")]
    public partial class MES_ProductOutbound:ServiceEntity
    {
        /// <summary>
       ///出庫ID
       /// </summary>
       [Key]
       [Display(Name ="出庫ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid OutboundID { get; set; }

       /// <summary>
       ///單據號
       /// </summary>
       [Display(Name ="單據號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string DocumentNo { get; set; }

       /// <summary>
       ///物料名稱
       /// </summary>
       [Display(Name ="物料名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string MaterialName { get; set; }

       /// <summary>
       ///物料编號
       /// </summary>
       [Display(Name ="物料编號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string MaterialCode { get; set; }

       /// <summary>
       ///规格型號
       /// </summary>
       [Display(Name ="规格型號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string SpecificationModel { get; set; }

       /// <summary>
       ///仓庫ID
       /// </summary>
       [Display(Name ="仓庫ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? WarehouseID { get; set; }

       /// <summary>
       ///货位ID
       /// </summary>
       [Display(Name ="货位ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? LocationID { get; set; }

       /// <summary>
       ///出庫數量
       /// </summary>
       [Display(Name ="出庫數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int OutboundQuantity { get; set; }

       /// <summary>
       ///出庫單位
       /// </summary>
       [Display(Name ="出庫單位")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string OutboundUnit { get; set; }

       /// <summary>
       ///出庫日期
       /// </summary>
       [Display(Name ="出庫日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? OutboundDate { get; set; }

       /// <summary>
       ///出庫操作員
       /// </summary>
       [Display(Name ="出庫操作員")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string OutboundOperator { get; set; }

       /// <summary>
       ///出庫原因
       /// </summary>
       [Display(Name ="出庫原因")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string OutboundReason { get; set; }

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
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
    [Entity(TableCnName = "產線設備",DBServer = "ServiceDbContext")]
    public partial class MES_ProductionLineDevice:ServiceEntity
    {
        /// <summary>
       ///設備ID
       /// </summary>
       [Key]
       [Display(Name ="設備ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid DeviceID { get; set; }

       /// <summary>
       ///產線ID
       /// </summary>
       [Display(Name ="產線ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? ProductionLineID { get; set; }

       /// <summary>
       ///設備名稱
       /// </summary>
       [Display(Name ="設備名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string DeviceName { get; set; }

       /// <summary>
       ///設備型號
       /// </summary>
       [Display(Name ="設備型號")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string DeviceModel { get; set; }

       /// <summary>
       ///制造商
       /// </summary>
       [Display(Name ="制造商")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Manufacturer { get; set; }

       /// <summary>
       ///购买日期
       /// </summary>
       [Display(Name ="购买日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? PurchaseDate { get; set; }

       /// <summary>
       ///保修日期
       /// </summary>
       [Display(Name ="保修日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? WarrantyDate { get; set; }

       /// <summary>
       ///設備状態
       /// </summary>
       [Display(Name ="設備状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Status { get; set; }

       /// <summary>
       ///線上位置
       /// </summary>
       [Display(Name ="線上位置")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string LocationOnLine { get; set; }

       /// <summary>
       ///備注信息
       /// </summary>
       [Display(Name ="備注信息")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Remarks { get; set; }

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
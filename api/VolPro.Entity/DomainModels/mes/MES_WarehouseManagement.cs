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
    [Entity(TableCnName = "仓庫管理",TableName = "MES_WarehouseManagement",DBServer = "ServiceDbContext")]
    public partial class MES_WarehouseManagement:ServiceEntity
    {
        /// <summary>
       ///仓庫ID
       /// </summary>
       [Key]
       [Display(Name ="仓庫ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid WarehouseID { get; set; }

       /// <summary>
       ///仓庫编碼
       /// </summary>
       [Display(Name ="仓庫编碼")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseCode { get; set; }

       /// <summary>
       ///仓庫名稱
       /// </summary>
       [Display(Name ="仓庫名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseName { get; set; }

       /// <summary>
       ///仓庫類型
       /// </summary>
       [Display(Name ="仓庫類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseType { get; set; }

       /// <summary>
       ///仓庫面积
       /// </summary>
       [Display(Name ="仓庫面积")]
       [DisplayFormat(DataFormatString="10,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal WarehouseArea { get; set; }

       /// <summary>
       ///仓庫地址
       /// </summary>
       [Display(Name ="仓庫地址")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseAddress { get; set; }

       /// <summary>
       ///仓庫电话
       /// </summary>
       [Display(Name ="仓庫电话")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehousePhone { get; set; }

       /// <summary>
       ///仓庫管理員
       /// </summary>
       [Display(Name ="仓庫管理員")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseManager { get; set; }

       /// <summary>
       ///仓庫状態
       /// </summary>
       [Display(Name ="仓庫状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string WarehouseStatus { get; set; }

       /// <summary>
       ///仓庫容量
       /// </summary>
       [Display(Name ="仓庫容量")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int WarehouseCapacity { get; set; }

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
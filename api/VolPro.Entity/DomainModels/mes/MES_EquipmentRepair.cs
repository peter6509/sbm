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
    [Entity(TableCnName = "設備维修",DBServer = "ServiceDbContext")]
    public partial class MES_EquipmentRepair:ServiceEntity
    {
        /// <summary>
       ///维修ID
       /// </summary>
       [Key]
       [Display(Name ="维修ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid RepairID { get; set; }

       /// <summary>
       ///設備ID
       /// </summary>
       [Display(Name ="設備ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? EquipmentID { get; set; }

       /// <summary>
       ///维修日期
       /// </summary>
       [Display(Name ="维修日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? RepairDate { get; set; }

       /// <summary>
       ///维修原因
       /// </summary>
       [Display(Name ="维修原因")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string RepairReason { get; set; }

       /// <summary>
       ///维修内容
       /// </summary>
       [Display(Name ="维修内容")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string RepairContent { get; set; }

       /// <summary>
       ///维修人員
       /// </summary>
       [Display(Name ="维修人員")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string RepairPerson { get; set; }

       /// <summary>
       ///维修成本
       /// </summary>
       [Display(Name ="维修成本")]
       [DisplayFormat(DataFormatString="10,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? RepairCost { get; set; }

       /// <summary>
       ///维修状態
       /// </summary>
       [Display(Name ="维修状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string RepairStatus { get; set; }

       /// <summary>
       ///维修開始時间
       /// </summary>
       [Display(Name ="维修開始時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? RepairStartTime { get; set; }

       /// <summary>
       ///维修结束時间
       /// </summary>
       [Display(Name ="维修结束時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? RepairEndTime { get; set; }

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
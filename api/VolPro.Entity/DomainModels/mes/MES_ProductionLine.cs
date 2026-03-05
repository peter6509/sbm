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
    [Entity(TableCnName = "產線管理",TableName = "MES_ProductionLine",DetailTable =  new Type[] { typeof(MES_ProductionLineDevice)},DetailTableCnName = "產線設備",DBServer = "ServiceDbContext")]
    public partial class MES_ProductionLine:ServiceEntity
    {
        /// <summary>
       ///產線ID
       /// </summary>
       [Key]
       [Display(Name ="產線ID")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid ProductionLineID { get; set; }

       /// <summary>
       ///產線名稱
       /// </summary>
       [Display(Name ="產線名稱")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string LineName { get; set; }

       /// <summary>
       ///產線類型
       /// </summary>
       [Display(Name ="產線類型")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string LineType { get; set; }

       /// <summary>
       ///產能信息
       /// </summary>
       [Display(Name ="產能信息")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string Capacity { get; set; }

       /// <summary>
       ///產線状態
       /// </summary>
       [Display(Name ="產線状態")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string Status { get; set; }

       /// <summary>
       ///负责人
       /// </summary>
       [Display(Name ="负责人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string ResponsiblePerson { get; set; }

       /// <summary>
       ///產線位置
       /// </summary>
       [Display(Name ="產線位置")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string Location { get; set; }

       /// <summary>
       ///啟用日期
       /// </summary>
       [Display(Name ="啟用日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? StartDate { get; set; }

       /// <summary>
       ///停用日期
       /// </summary>
       [Display(Name ="停用日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? EndDate { get; set; }

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

       [Display(Name ="產線設備")]
       [ForeignKey("ProductionLineID")]
       public List<MES_ProductionLineDevice> MES_ProductionLineDevice { get; set; }


       
    }
}
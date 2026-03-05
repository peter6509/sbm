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
    [Entity(TableCnName = "產品尺寸",TableName = "Demo_ProductSize",DetailTable =  new Type[] { typeof(Demo_ProductSizeSub)},DetailTableCnName = "產品尺寸明细",DBServer = "SysDbContext")]
    public partial class Demo_ProductSize:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="ProductSizeId")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid ProductSizeId { get; set; }

       /// <summary>
       ///商品id
       /// </summary>
       [Display(Name ="商品id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? ProductId { get; set; }

       /// <summary>
       ///尺寸
       /// </summary>
       [Display(Name ="尺寸")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Size { get; set; }

       /// <summary>
       ///單位
       /// </summary>
       [Display(Name ="單位")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Unit { get; set; }

       /// <summary>
       ///備注
       /// </summary>
       [Display(Name ="備注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///創建人
       /// </summary>
       [Display(Name ="創建人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Creator { get; set; }

       /// <summary>
       ///創建時间
       /// </summary>
       [Display(Name ="創建時间")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時间
       /// </summary>
       [Display(Name ="修改時间")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       [Display(Name ="產品尺寸明细")]
       [ForeignKey("ProductSizeId")]
       public List<Demo_ProductSizeSub> Demo_ProductSizeSub { get; set; }


       
    }
}
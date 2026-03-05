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
    [Entity(TableCnName = "報價單明細檔",TableName = "sbm_sale_order_line",DBServer = "SysDbContext")]
    public partial class sbm_sale_order_line:SysEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int id { get; set; }

       /// <summary>
       ///主檔關聯
       /// </summary>
       [Display(Name ="主檔關聯")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? sale_id { get; set; }

       /// <summary>
       ///上階ID
       /// </summary>
       [Display(Name ="上階ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? parent_id { get; set; }

       /// <summary>
       ///項次
       /// </summary>
       [Display(Name ="項次")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string sale_item { get; set; }

       /// <summary>
       ///產品
       /// </summary>
       [Display(Name ="產品")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string prod_name { get; set; }

       /// <summary>
       ///數量
       /// </summary>
       [Display(Name ="數量")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? prod_num { get; set; }

       /// <summary>
       ///單位
       /// </summary>
       [Display(Name ="單位")]
       [MaxLength(10)]
       [Column(TypeName="varchar(10)")]
       [Editable(true)]
       public string prod_uom { get; set; }

       /// <summary>
       ///單價
       /// </summary>
       [Display(Name ="單價")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? price_unit { get; set; }

       /// <summary>
       ///小計
       /// </summary>
       [Display(Name ="小計")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? price_sub { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [MaxLength(300)]
       [Column(TypeName="varchar(300)")]
       [Editable(true)]
       public string prod_desc { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Creator")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

       
    }
}
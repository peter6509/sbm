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
    [Entity(TableCnName = "事業單位",TableName = "sbm_bu",DBServer = "SysDbContext")]
    public partial class sbm_bu:SysEntity
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
       ///單位名稱
       /// </summary>
       [Display(Name ="單位名稱")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string bu_name { get; set; }

       /// <summary>
       ///單位LOGO
       /// </summary>
       [Display(Name ="單位LOGO")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string bu_logo { get; set; }

       /// <summary>
       ///單位發票章
       /// </summary>
       [Display(Name ="單位發票章")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string bu_invoice_stamp { get; set; }

       /// <summary>
       ///銀行代碼
       /// </summary>
       [Display(Name ="銀行代碼")]
       [MaxLength(10)]
       [Column(TypeName="varchar(10)")]
       [Editable(true)]
       public string bu_bankno { get; set; }

       /// <summary>
       ///銀行名稱
       /// </summary>
       [Display(Name ="銀行名稱")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string bu_bankname { get; set; }

       /// <summary>
       ///分行代碼
       /// </summary>
       [Display(Name ="分行代碼")]
       [MaxLength(10)]
       [Column(TypeName="varchar(10)")]
       [Editable(true)]
       public string bu_bankbno { get; set; }

       /// <summary>
       ///分行名稱
       /// </summary>
       [Display(Name ="分行名稱")]
       [MaxLength(10)]
       [Column(TypeName="varchar(10)")]
       [Editable(true)]
       public string bu_bankbname { get; set; }

       /// <summary>
       ///帳號
       /// </summary>
       [Display(Name ="帳號")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string bu_bankaccno { get; set; }

       /// <summary>
       ///戶名
       /// </summary>
       [Display(Name ="戶名")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string bu_bankaccname { get; set; }

       
    }
}
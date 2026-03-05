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
    [Entity(TableCnName = "報價單主檔",TableName = "sbm_sale_order",DetailTable =  new Type[] { typeof(sbm_sale_order_line)},DetailTableCnName = "報價單明細檔",DBServer = "SysDbContext")]
    public partial class sbm_sale_order:SysEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int sale_id { get; set; }

       /// <summary>
       ///報價單號
       /// </summary>
       [Display(Name ="報價單號")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string name { get; set; }

       /// <summary>
       ///報價日期
       /// </summary>
       [Display(Name ="報價日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? sale_date { get; set; }

       /// <summary>
       ///業務員
       /// </summary>
       [Display(Name ="業務員")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? sales { get; set; }

       /// <summary>
       ///客戶名稱
       /// </summary>
       [Display(Name ="客戶名稱")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? partner_company { get; set; }

       /// <summary>
       ///客戶聯絡人
       /// </summary>
       [Display(Name ="客戶聯絡人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? partner_contact { get; set; }

       /// <summary>
       ///未稅小計
       /// </summary>
       [Display(Name ="未稅小計")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? untax_amount { get; set; }

       /// <summary>
       ///優惠小計
       /// </summary>
       [Display(Name ="優惠小計")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? discount_amount { get; set; }

       /// <summary>
       ///稅金
       /// </summary>
       [Display(Name ="稅金")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? tax { get; set; }

       /// <summary>
       ///含稅合計
       /// </summary>
       [Display(Name ="含稅合計")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? tot_amount { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string description { get; set; }

       /// <summary>
       ///製表日期
       /// </summary>
       [Display(Name ="製表日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? CreateID { get; set; }

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

       /// <summary>
       ///聯絡人電話
       /// </summary>
       [Display(Name ="聯絡人電話")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string partner_mobile { get; set; }

       /// <summary>
       ///業務員手機
       /// </summary>
       [Display(Name ="業務員手機")]
       [MaxLength(11)]
       [Column(TypeName="varchar(11)")]
       [Editable(true)]
       public string sales_mobile { get; set; }

       /// <summary>
       ///客戶EMAIL
       /// </summary>
       [Display(Name ="客戶EMAIL")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string partner_email { get; set; }

       /// <summary>
       ///業務EMAIL
       /// </summary>
       [Display(Name ="業務EMAIL")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string sales_email { get; set; }

       /// <summary>
       ///成本中心
       /// </summary>
       [Display(Name ="成本中心")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? company_id { get; set; }

       /// <summary>
       ///LOGO
       /// </summary>
       [Display(Name ="LOGO")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string bu_logo { get; set; }

       /// <summary>
       ///發票章
       /// </summary>
       [Display(Name ="發票章")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string bu_invoice_stamp { get; set; }

       /// <summary>
       ///匯款資訊
       /// </summary>
       [Display(Name ="匯款資訊")]
       [MaxLength(300)]
       [Column(TypeName="varchar(300)")]
       [Editable(true)]
       public string bu_bank_info { get; set; }

       [Display(Name ="報價單明細檔")]
       [ForeignKey("sale_id")]
       public List<sbm_sale_order_line> sbm_sale_order_line { get; set; }


       
    }
}
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
    [Entity(TableCnName = "詢價單主檔",TableName = "sbm_require_purchase",DetailTable =  new Type[] { typeof(sbm_require_purchase_line),typeof(sbm_require_purchase_doc)},DetailTableCnName = "詢價單明細檔,詢價單上傳檔案",DBServer = "SysDbContext")]
    public partial class sbm_require_purchase:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="req_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int req_id { get; set; }

       /// <summary>
       ///詢價單號
       /// </summary>
       [Display(Name ="詢價單號")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string name { get; set; }

       /// <summary>
       ///SO單號
       /// </summary>
       [Display(Name ="SO單號")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? sale_id { get; set; }

       /// <summary>
       ///終端客戶
       /// </summary>
       [Display(Name ="終端客戶")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? end_user { get; set; }

       /// <summary>
       ///廠商名稱
       /// </summary>
       [Display(Name ="廠商名稱")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? partner_company { get; set; }

       /// <summary>
       ///廠商聯絡人
       /// </summary>
       [Display(Name ="廠商聯絡人")]
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

       [Display(Name ="詢價單明細檔")]
       [ForeignKey("req_id")]
       public List<sbm_require_purchase_line> sbm_require_purchase_line { get; set; }


       [Display(Name ="詢價單上傳檔案")]
       [ForeignKey("req_id")]
       public List<sbm_require_purchase_doc> sbm_require_purchase_doc { get; set; }


       
    }
}
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
    [Entity(TableCnName = "合作夥伴",TableName = "sbm_partner",DBServer = "SysDbContext")]
    public partial class sbm_partner:SysEntity
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
       ///夥伴名稱
       /// </summary>
       [Display(Name ="夥伴名稱")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string name { get; set; }

       /// <summary>
       ///公司/個人
       /// </summary>
       [Display(Name ="公司/個人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? partner_type { get; set; }

       /// <summary>
       ///所屬公司
       /// </summary>
       [Display(Name ="所屬公司")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? parent_id { get; set; }

       /// <summary>
       ///職位
       /// </summary>
       [Display(Name ="職位")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string position { get; set; }

       /// <summary>
       ///公司地址
       /// </summary>
       [Display(Name ="公司地址")]
       [MaxLength(300)]
       [Column(TypeName="varchar(300)")]
       [Editable(true)]
       public string address { get; set; }

       /// <summary>
       ///電話
       /// </summary>
       [Display(Name ="電話")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string tel { get; set; }

       /// <summary>
       ///行動電話
       /// </summary>
       [Display(Name ="行動電話")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string mobile { get; set; }

       /// <summary>
       ///EMail
       /// </summary>
       [Display(Name ="EMail")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string email { get; set; }

       /// <summary>
       ///備註
       /// </summary>
       [Display(Name ="備註")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string memo { get; set; }

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
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? ModifyID { get; set; }

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
       ///統編
       /// </summary>
       [Display(Name ="統編")]
       [MaxLength(8)]
       [Column(TypeName="varchar(8)")]
       [Editable(true)]
       public string vat { get; set; }

       /// <summary>
       ///客戶/供應商
       /// </summary>
       [Display(Name ="客戶/供應商")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? bus_type { get; set; }

       /// <summary>
       ///銀行代號
       /// </summary>
       [Display(Name ="銀行代號")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string bank_code { get; set; }

       /// <summary>
       ///銀行名稱
       /// </summary>
       [Display(Name ="銀行名稱")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string bank_name { get; set; }

       /// <summary>
       ///分行代號
       /// </summary>
       [Display(Name ="分行代號")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string bank_bcode { get; set; }

       /// <summary>
       ///分行名稱
       /// </summary>
       [Display(Name ="分行名稱")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string bank_bname { get; set; }

       /// <summary>
       ///帳戶名稱
       /// </summary>
       [Display(Name ="帳戶名稱")]
       [MaxLength(150)]
       [Column(TypeName="varchar(150)")]
       [Editable(true)]
       public string account_name { get; set; }

       /// <summary>
       ///帳號
       /// </summary>
       [Display(Name ="帳號")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string account_no { get; set; }

       /// <summary>
       ///帳務聯絡窗口
       /// </summary>
       [Display(Name ="帳務聯絡窗口")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string account_window { get; set; }

       
    }
}
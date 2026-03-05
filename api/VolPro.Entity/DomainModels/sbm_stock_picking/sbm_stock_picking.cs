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
    [Entity(TableCnName = "調撥單主檔",TableName = "sbm_stock_picking",DetailTable =  new Type[] { typeof(sbm_stockmove)},DetailTableCnName = "調撥單明細",DBServer = "SysDbContext")]
    public partial class sbm_stock_picking:SysEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int picking_id { get; set; }

       /// <summary>
       ///調撥單單號
       /// </summary>
       [Display(Name ="調撥單單號")]
       [MaxLength(30)]
       [Column(TypeName="varchar(30)")]
       [Editable(true)]
       public string name { get; set; }

       /// <summary>
       ///調撥日期
       /// </summary>
       [Display(Name ="調撥日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime picking_date { get; set; }

       /// <summary>
       ///調撥類型
       /// </summary>
       [Display(Name ="調撥類型")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int picking_type { get; set; }

       /// <summary>
       ///來源單號
       /// </summary>
       [Display(Name ="來源單號")]
       [Column(TypeName="varchar()")]
       [Editable(true)]
       public string origin { get; set; }

       /// <summary>
       ///來源位置
       /// </summary>
       [Display(Name ="來源位置")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string location_source { get; set; }

       /// <summary>
       ///目的位置
       /// </summary>
       [Display(Name ="目的位置")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string location_destination { get; set; }

       /// <summary>
       ///作業人員
       /// </summary>
       [Display(Name ="作業人員")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? picking_owner { get; set; }

       /// <summary>
       ///事業單位
       /// </summary>
       [Display(Name ="事業單位")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int company_id { get; set; }

       /// <summary>
       ///合作夥伴公司
       /// </summary>
       [Display(Name ="合作夥伴公司")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int partner_company { get; set; }

       /// <summary>
       ///夥伴聯絡人
       /// </summary>
       [Display(Name ="夥伴聯絡人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? partner_contact { get; set; }

       /// <summary>
       ///LOGO
       /// </summary>
       [Display(Name ="LOGO")]
       [MaxLength(200)]
       [Column(TypeName="varchar(200)")]
       [Editable(true)]
       public string bu_logo { get; set; }

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

       [Display(Name ="調撥單明細")]
       [ForeignKey("picking_id")]
       public List<sbm_stockmove> sbm_stockmove { get; set; }


       
    }
}
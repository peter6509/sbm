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
    [Entity(TableCnName = "詢價單上傳檔案",TableName = "sbm_require_purchase_doc",DBServer = "SysDbContext")]
    public partial class sbm_require_purchase_doc:SysEntity
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
       public int? req_id { get; set; }

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
       ///詢價案名
       /// </summary>
       [Display(Name ="詢價案名")]
       [MaxLength(300)]
       [Column(TypeName="varchar(300)")]
       [Editable(true)]
       public string name { get; set; }

       /// <summary>
       ///詢價文件檔
       /// </summary>
       [Display(Name ="詢價文件檔")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string req_doc { get; set; }

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

       
    }
}
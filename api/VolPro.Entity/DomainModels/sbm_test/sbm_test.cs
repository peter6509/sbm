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
    [Entity(TableCnName = "TEST",TableName = "sbm_test",DBServer = "SysDbContext")]
    public partial class sbm_test:SysEntity
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
       ///TEST單號
       /// </summary>
       [Display(Name ="TEST單號")]
       [Column(TypeName="varchar()")]
       [Editable(true)]
       public string test_no { get; set; }

       /// <summary>
       ///TEST名稱
       /// </summary>
       [Display(Name ="TEST名稱")]
       [Column(TypeName="varchar()")]
       [Editable(true)]
       public string test_name { get; set; }

       /// <summary>
       ///TEST屬性
       /// </summary>
       [Display(Name ="TEST屬性")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? test_type { get; set; }

       /// <summary>
       ///TEST地址
       /// </summary>
       [Display(Name ="TEST地址")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string test_addr { get; set; }

       /// <summary>
       ///TEST備註
       /// </summary>
       [Display(Name ="TEST備註")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string test_memo { get; set; }

       
    }
}
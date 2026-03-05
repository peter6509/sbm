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
    [Entity(TableCnName = "字典數據",TableName = "Sys_Dictionary",DetailTable =  new Type[] { typeof(Sys_DictionaryList)},DetailTableCnName = "字典明细",DBServer = "SysDbContext")]
    public partial class Sys_Dictionary:SysEntity
    {
        /// <summary>
       ///字典ID
       /// </summary>
       [Key]
       [Display(Name ="字典ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Dic_ID { get; set; }

       /// <summary>
       ///字典编號
       /// </summary>
       [Display(Name ="字典编號")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string DicNo { get; set; }

       /// <summary>
       ///字典名稱
       /// </summary>
       [Display(Name ="字典名稱")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string DicName { get; set; }

       /// <summary>
       ///父级ID
       /// </summary>
       [Display(Name ="父级ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ParentId { get; set; }

       /// <summary>
       ///配置项
       /// </summary>
       [Display(Name ="配置项")]
       [Column(TypeName="nvarchar(max)")]
       public string Config { get; set; }

       /// <summary>
       ///sql语句
       /// </summary>
       [Display(Name ="sql语句")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string DbSql { get; set; }

       /// <summary>
       ///所在數據庫
       /// </summary>
       [Display(Name ="所在數據庫")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string DBServer { get; set; }

       /// <summary>
       ///排序號
       /// </summary>
       [Display(Name ="排序號")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? OrderNo { get; set; }

       /// <summary>
       ///備注
       /// </summary>
       [Display(Name ="備注")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(4000)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///是否啟用
       /// </summary>
       [Display(Name ="是否啟用")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Enable { get; set; }

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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時间
       /// </summary>
       [Display(Name ="修改時间")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       [Display(Name ="字典明细")]
       [ForeignKey("Dic_ID")]
       public List<Sys_DictionaryList> Sys_DictionaryList { get; set; }

    }
}
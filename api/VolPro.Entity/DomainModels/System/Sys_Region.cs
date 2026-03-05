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
    [Entity(TableCnName = "省市区县",TableName = "Sys_Region",DBServer = "SysDbContext")]
    public partial class Sys_Region:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int id { get; set; }

       /// <summary>
       ///编碼
       /// </summary>
       [Display(Name ="编碼")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string code { get; set; }

       /// <summary>
       ///名稱
       /// </summary>
       [Display(Name ="名稱")]
       [MaxLength(40)]
       [Column(TypeName="varchar(40)")]
       [Editable(true)]
       public string name { get; set; }

       /// <summary>
       ///上级编碼
       /// </summary>
       [Display(Name ="上级编碼")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? parentId { get; set; }

       /// <summary>
       ///级别
       /// </summary>
       [Display(Name ="级别")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? level { get; set; }

       /// <summary>
       ///完整地址
       /// </summary>
       [Display(Name ="完整地址")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string mername { get; set; }

       /// <summary>
       ///經度
       /// </summary>
       [Display(Name ="經度")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? Lng { get; set; }

       /// <summary>
       ///纬度
       /// </summary>
       [Display(Name ="纬度")]
       [Column(TypeName="float")]
       [Editable(true)]
       public float? Lat { get; set; }

       /// <summary>
       ///拼音
       /// </summary>
       [Display(Name ="拼音")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       [Editable(true)]
       public string pinyin { get; set; }

       
    }
}
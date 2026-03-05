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
    public partial class Sys_User
    {
        //此處配置字段(字段配置见此model的另一個partial),如果表中没有此字段請加上 [NotMapped]屬性，否则會异常

        /// <summary>
        ///是否在線 2023.12.10增加是否在線字段,顯示用户在線状態，强制下線功能
        /// </summary>
        [Display(Name = "是否在線")]
        [Column(TypeName = "int")]
        [NotMapped]
        public int IsOnline { get; set; }
    }
}
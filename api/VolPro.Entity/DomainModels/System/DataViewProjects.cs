/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果數據庫字段發生变化，请在代碼生器重新生成此Model
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
    [Entity(TableCnName = "DataViewProjects", TableName = "DataViewProjects", DBServer = "SysDbContext")]
    public partial class DataViewProjects : SysEntity
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "bigint")]
        [Required(AllowEmptyStrings = false)]
        public long Id { get; set; }

        /// <summary>
        ///名稱
        /// </summary>
        [Display(Name = "名稱")]
        [Column(TypeName = "nvarchar(max)")]
        [Editable(true)]
        public string ProjectName { get; set; }

        /// <summary>
        ///發布状態
        /// </summary>
        [Display(Name = "發布状態")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int State { get; set; }

        /// <summary>
        ///删除状態
        /// </summary>
        [Display(Name = "删除状態")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int IsDel { get; set; }

        /// <summary>
        ///预览图片
        /// </summary>
        [Display(Name = "预览图片")]
        [Column(TypeName = "nvarchar(max)")]
        [Editable(true)]
        public string IndexImage { get; set; }

        /// <summary>
        ///備注
        /// </summary>
        [Display(Name = "備注")]
        [Column(TypeName = "nvarchar(max)")]
        [Editable(true)]
        public string Remarks { get; set; }

        /// <summary>
        ///排序號
        /// </summary>
        [Display(Name = "排序號")]
        [Column(TypeName = "int")]
        public int? OrderNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateId")]
        [Column(TypeName = "int")]
        public int? CreateId { get; set; }

        /// <summary>
        ///創建人
        /// </summary>
        [Display(Name = "創建人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Creator { get; set; }

        /// <summary>
        ///創建時间
        /// </summary>
        [Display(Name = "創建時间")]
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Content")]
        [Column(TypeName = "nvarchar(max)")]
        // [Column(TypeName = "NCLOB")]
        public string Content { get; set; }

        [Display(Name = "")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? DbServiceId { get; set; }
    }
}
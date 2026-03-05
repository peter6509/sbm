using VolPro.Entity.SystemModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace VolPro.Entity.DomainModels
{
    [Table("Sys_TableInfo")]
    [EntityAttribute(DetailTable = new Type[] { typeof(Sys_TableColumn) })]
    public class Sys_TableInfo : SysEntity
    {
        [Key]
        [Column(TypeName = "int")]
        public int Table_Id { get; set; }
        [Editable(true)]
        [Column(TypeName = "int")]
        public int? ParentId { get; set; }

        [Editable(true)]
        public string TableName { get; set; }
        [Editable(true)]
        public string TableTrueName { get; set; }
        [Editable(true)]
        public string ColumnCNName { get; set; }


        public string Namespace { get; set; }

        [Editable(true)]
        public string FolderName { get; set; }

        [Editable(true)]
        public string DataTableType { get; set; }

        [Editable(true)]
        public string EditorType { get; set; }
        [Editable(true)]
        [Column(TypeName = "int")]
        public int? OrderNo { get; set; }
        [Editable(true)]
        public string UploadField { get; set; }
        [Editable(true)]
        public int? UploadMaxCount { get; set; }
        [Editable(true)]
        public string RichText { get; set; }
        [Editable(true)]
        public string ExpressField { get; set; }
        [Editable(true)]
        public string DBServer { get; set; }
        [Editable(true)]
        public string SortName { get; set; }
        [Editable(true)]
        public string DetailCnName { get; set; }


        [Editable(true)]
        public string DetailName { get; set; }

        [Editable(true)]
        [Column(TypeName = "int")]
        public int? Enable { get; set; }


        [Editable(true)]
        public string CnName { get; set; }


        [Editable(true)]
        public int? SearchType { get; set; }

        [Editable(true)]
        public int? EditType { get; set; }

        /// <summary>
        /// 查詢數據源
        /// </summary>
        [Editable(true)]
        public string SearchDropNo { get; set; }

        /// <summary>
        /// 數據庫脚本
        /// </summary>
        [Editable(true)]
        public string DbSql { get; set; }

        /// <summary>
        /// 與主表关联的字段
        /// </summary>
        [Editable(true)]
        public string ForeignField { get; set; }
        /// <summary>
        /// 動態加载页面
        /// </summary>
        [Editable(true)]
        public int? DyPage { get; set; }

        /// <summary>
        /// 動態脚本脚本
        /// </summary>
        [Editable(true)]
        public string DyScript{ get; set; }

        /// <summary>
        /// 顯示所有查詢条件
        /// </summary>
        [Editable(true)]
        public int? FixedSearch { get; set; }


        [ForeignKey("Table_Id")]
        public List<Sys_TableColumn> TableColumns { get; set; } 

    }
}

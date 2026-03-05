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
    [Entity(TableCnName = "審批流程节點",TableName = "Sys_WorkFlowTableStep",DBServer = "SysDbContext")]
    public partial class Sys_WorkFlowTableStep:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Sys_WorkFlowTableStep_Id")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid Sys_WorkFlowTableStep_Id { get; set; }

       /// <summary>
       ///主表id
       /// </summary>
       [Display(Name ="主表id")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid WorkFlowTable_Id { get; set; }

       /// <summary>
       ///流程id
       /// </summary>
       [Display(Name ="流程id")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       public Guid? WorkFlow_Id { get; set; }

       /// <summary>
       ///节點id
       /// </summary>
       [Display(Name ="节點id")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string StepId { get; set; }

       /// <summary>
       ///节名稱
       /// </summary>
       [Display(Name ="节名稱")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string StepName { get; set; }

       /// <summary>
       ///審批類型
       /// </summary>
       [Display(Name ="審批類型")]
       [Column(TypeName="int")]
       public int? StepType { get; set; }

       /// <summary>
       ///节點類型(1=按用户審批,2=按角色審批)
       /// </summary>
       [Display(Name ="节點類型(1=按用户審批,2=按角色審批)")]
       [Column(TypeName="nvarchar(max)")]
       public string StepValue { get; set; }

       /// <summary>
       ///審批顺序
       /// </summary>
       [Display(Name ="審批顺序")]
       [Column(TypeName="int")]
       public int? OrderId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Remark")]
       [Column(TypeName="nvarchar(max)")]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Creator")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Enable")]
       [Column(TypeName="tinyint")]
       public byte? Enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyDate")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///審核人id
       /// </summary>
       [Display(Name ="審核人id")]
       [Column(TypeName="int")]
       public int? AuditId { get; set; }

       /// <summary>
       ///審核人
       /// </summary>
       [Display(Name ="審核人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string Auditor { get; set; }

       /// <summary>
       ///審核状態
       /// </summary>
       [Display(Name ="審核状態")]
       [Column(TypeName="int")]
       public int? AuditStatus { get; set; }

       /// <summary>
       ///審核時间
       /// </summary>
       [Display(Name ="審核時间")]
       [Column(TypeName="datetime")]
       public DateTime? AuditDate { get; set; }

       /// <summary>
       ///节點屬性(start、node、end))
       /// </summary>
       [Display(Name ="节點屬性(start、node、end))")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string StepAttrType { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ParentId")]
       [Column(TypeName="nvarchar(max)")]
       public string ParentId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="NextStepId")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string NextStepId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Weight")]
       [Column(TypeName="int")]
       public int? Weight { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="AuditMethod")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditMethod { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="FormOptions")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string FormOptions { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="SourceType")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string SourceType { get; set; }

       /// <summary>
       ///附件
       /// </summary>
       [Display(Name ="附件")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string AttachFile { get; set; }

       /// <summary>
       ///附件類型
       /// </summary>
       [Display(Name ="附件類型")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string AttachType { get; set; }

       /// <summary>
       ///编輯表單
       /// </summary>
       [Display(Name ="编輯表單")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string StepEditForm { get; set; }

       /// <summary>
       ///附件數量
       /// </summary>
       [Display(Name ="附件數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AttachQty { get; set; }

       
    }
}
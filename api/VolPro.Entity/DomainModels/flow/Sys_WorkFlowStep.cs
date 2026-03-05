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
    [Entity(TableCnName = "審批节點配置",TableName = "Sys_WorkFlowStep",DBServer = "SysDbContext")]
    public partial class Sys_WorkFlowStep:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="WorkStepFlow_Id")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid WorkStepFlow_Id { get; set; }

       /// <summary>
       ///流程主表id
       /// </summary>
       [Display(Name ="流程主表id")]
       [MaxLength(36)]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? WorkFlow_Id { get; set; }

       /// <summary>
       ///流程节點Id
       /// </summary>
       [Display(Name ="流程节點Id")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string StepId { get; set; }

       /// <summary>
       ///节點名稱
       /// </summary>
       [Display(Name ="节點名稱")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string StepName { get; set; }

       /// <summary>
       ///节點類型(1=按用户審批,2=按角色審批)
       /// </summary>
       [Display(Name ="节點類型(1=按用户審批,2=按角色審批)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? StepType { get; set; }

       /// <summary>
       ///審批用户id或角色id
       /// </summary>
       [Display(Name ="審批用户id或角色id")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string StepValue { get; set; }

       /// <summary>
       ///備注
       /// </summary>
       [Display(Name ="備注")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///審批顺序
       /// </summary>
       [Display(Name ="審批顺序")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? OrderId { get; set; }

       /// <summary>
       ///創建時间
       /// </summary>
       [Display(Name ="創建時间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

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
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改時间
       /// </summary>
       [Display(Name ="修改時间")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///下一個審批节點
       /// </summary>
       [Display(Name ="下一個審批节點")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string NextStepIds { get; set; }

       /// <summary>
       ///父级节點
       /// </summary>
       [Display(Name ="父级节點")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string ParentId { get; set; }

       /// <summary>
       ///審核未通過(返回上一节點,流程重新開始,流程结束)
       /// </summary>
       [Display(Name ="審核未通過(返回上一节點,流程重新開始,流程结束)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditRefuse { get; set; }

       /// <summary>
       ///驳回(返回上一节點,流程重新開始,流程结束)
       /// </summary>
       [Display(Name ="驳回(返回上一节點,流程重新開始,流程结束)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditBack { get; set; }

       /// <summary>
       ///審批方式(啟用會签)
       /// </summary>
       [Display(Name ="審批方式(啟用會签)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AuditMethod { get; set; }

       /// <summary>
       ///審核后發送郵件通知
       /// </summary>
       [Display(Name ="審核后發送郵件通知")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? SendMail { get; set; }

       /// <summary>
       ///審核条件
       /// </summary>
       [Display(Name ="審核条件")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Filters { get; set; }

       /// <summary>
       ///节點屬性(start、node、end))
       /// </summary>
       [Display(Name ="节點屬性(start、node、end))")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string StepAttrType { get; set; }

       /// <summary>
       ///權重(相同条件權重大的优先匹配)
       /// </summary>
       [Display(Name ="權重(相同条件權重大的优先匹配)")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Weight { get; set; }

       /// <summary>
       ///节點编輯表彰
       /// </summary>
       [Display(Name ="节點编輯表彰")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string StepEditForm { get; set; }

       /// <summary>
       ///上傳附件
       /// </summary>
       [Display(Name ="上傳附件")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AllowUpload { get; set; }

       /// <summary>
       ///附件類型
       /// </summary>
       [Display(Name ="附件類型")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string AttachType { get; set; }

       /// <summary>
       ///附件數量
       /// </summary>
       [Display(Name ="附件數量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? AttachQty { get; set; }

       
    }
}
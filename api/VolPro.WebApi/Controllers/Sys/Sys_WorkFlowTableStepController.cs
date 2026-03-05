/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾Sys_WorkFlowTableStepController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.Sys.IServices;
namespace VolPro.Sys.Controllers
{
    [Route("api/Sys_WorkFlowTableStep")]
    [PermissionTable(Name = "Sys_WorkFlowTableStep")]
    public partial class Sys_WorkFlowTableStepController : ApiBaseController<ISys_WorkFlowTableStepService>
    {
        public Sys_WorkFlowTableStepController(ISys_WorkFlowTableStepService service)
        : base(service)
        {
        }
    }
}


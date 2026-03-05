/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾MES_ProductionPlanDetailController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.MES.IServices;
namespace VolPro.MES.Controllers
{
    [Route("api/MES_ProductionPlanDetail")]
    [PermissionTable(Name = "MES_ProductionPlanDetail")]
    public partial class MES_ProductionPlanDetailController : ApiBaseController<IMES_ProductionPlanDetailService>
    {
        public MES_ProductionPlanDetailController(IMES_ProductionPlanDetailService service)
        : base(service)
        {
        }
    }
}


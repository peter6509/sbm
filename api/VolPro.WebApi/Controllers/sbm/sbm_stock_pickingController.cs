/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾sbm_stock_pickingController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.sbm.IServices;
namespace VolPro.sbm.Controllers
{
    [Route("api/sbm_stock_picking")]
    [PermissionTable(Name = "sbm_stock_picking")]
    public partial class sbm_stock_pickingController : ApiBaseController<Isbm_stock_pickingService>
    {
        public sbm_stock_pickingController(Isbm_stock_pickingService service)
        : base(service)
        {
        }
    }
}


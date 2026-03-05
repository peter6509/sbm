/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾sbm_sale_order_lineController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.sbm.IServices;
namespace VolPro.sbm.Controllers
{
    [Route("api/sbm_sale_order_line")]
    [PermissionTable(Name = "sbm_sale_order_line")]
    public partial class sbm_sale_order_lineController : ApiBaseController<Isbm_sale_order_lineService>
    {
        public sbm_sale_order_lineController(Isbm_sale_order_lineService service)
        : base(service)
        {
        }
    }
}


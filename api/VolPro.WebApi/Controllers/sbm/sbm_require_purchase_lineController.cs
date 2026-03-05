/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾sbm_require_purchase_lineController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.sbm.IServices;
namespace VolPro.sbm.Controllers
{
    [Route("api/sbm_require_purchase_line")]
    [PermissionTable(Name = "sbm_require_purchase_line")]
    public partial class sbm_require_purchase_lineController : ApiBaseController<Isbm_require_purchase_lineService>
    {
        public sbm_require_purchase_lineController(Isbm_require_purchase_lineService service)
        : base(service)
        {
        }
    }
}


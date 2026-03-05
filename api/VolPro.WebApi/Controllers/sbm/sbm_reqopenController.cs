/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾sbm_reqopenController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.sbm.IServices;
namespace VolPro.sbm.Controllers
{
    [Route("api/sbm_reqopen")]
    [PermissionTable(Name = "sbm_reqopen")]
    public partial class sbm_reqopenController : ApiBaseController<Isbm_reqopenService>
    {
        public sbm_reqopenController(Isbm_reqopenService service)
        : base(service)
        {
        }
    }
}


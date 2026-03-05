/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾sbm_buController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.sbm.IServices;
namespace VolPro.sbm.Controllers
{
    [Route("api/sbm_bu")]
    [PermissionTable(Name = "sbm_bu")]
    public partial class sbm_buController : ApiBaseController<Isbm_buService>
    {
        public sbm_buController(Isbm_buService service)
        : base(service)
        {
        }
    }
}


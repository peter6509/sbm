/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾Sys_NotificationController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.Sys.IServices;
namespace VolPro.Sys.Controllers
{
    [Route("api/Sys_Notification")]
    [PermissionTable(Name = "Sys_Notification")]
    public partial class Sys_NotificationController : ApiBaseController<ISys_NotificationService>
    {
        public Sys_NotificationController(ISys_NotificationService service)
        : base(service)
        {
        }
    }
}


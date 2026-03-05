/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾Demo_OrderController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.DbTest.IServices;
namespace VolPro.DbTest.Controllers
{
    [Route("api/Demo_Order")]
    [PermissionTable(Name = "Demo_Order")]
    public partial class Demo_OrderController : ApiBaseController<IDemo_OrderService>
    {
        public Demo_OrderController(IDemo_OrderService service)
        : base(service)
        {
        }
    }
}


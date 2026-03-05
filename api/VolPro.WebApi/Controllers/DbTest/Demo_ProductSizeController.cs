/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾Demo_ProductSizeController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.DbTest.IServices;
namespace VolPro.DbTest.Controllers
{
    [Route("api/Demo_ProductSize")]
    [PermissionTable(Name = "Demo_ProductSize")]
    public partial class Demo_ProductSizeController : ApiBaseController<IDemo_ProductSizeService>
    {
        public Demo_ProductSizeController(IDemo_ProductSizeService service)
        : base(service)
        {
        }
    }
}


/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾TestDbController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.DbTest.IServices;
namespace VolPro.DbTest.Controllers
{
    [Route("api/TestDb")]
    [PermissionTable(Name = "TestDb")]
    public partial class TestDbController : ApiBaseController<ITestDbService>
    {
        public TestDbController(ITestDbService service)
        : base(service)
        {
        }
    }
}


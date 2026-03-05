/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾FormDesignOptionsController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.Sys.IServices;
namespace VolPro.Sys.Controllers
{
    [Route("api/FormDesignOptions")]
    [PermissionTable(Name = "FormDesignOptions")]
    public partial class FormDesignOptionsController : ApiBaseController<IFormDesignOptionsService>
    {
        public FormDesignOptionsController(IFormDesignOptionsService service)
        : base(service)
        {
        }
    }
}


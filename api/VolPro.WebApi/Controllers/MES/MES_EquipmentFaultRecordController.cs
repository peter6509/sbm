/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *如果要增加方法請在當前目錄下Partial文件夾MES_EquipmentFaultRecordController编写
 */
using Microsoft.AspNetCore.Mvc;
using VolPro.Core.Controllers.Basic;
using VolPro.Entity.AttributeManager;
using VolPro.MES.IServices;
namespace VolPro.MES.Controllers
{
    [Route("api/MES_EquipmentFaultRecord")]
    [PermissionTable(Name = "MES_EquipmentFaultRecord")]
    public partial class MES_EquipmentFaultRecordController : ApiBaseController<IMES_EquipmentFaultRecordService>
    {
        public MES_EquipmentFaultRecordController(IMES_EquipmentFaultRecordService service)
        : base(service)
        {
        }
    }
}


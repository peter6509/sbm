using VolPro.Builder.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VolPro.Core.Filters;
using VolPro.Entity.DomainModels;
using VolPro.Core.Utilities;
using System.Linq;
using VolPro.Core.Extensions;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using VolPro.Builder.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace VolPro.WebApi.Controllers.Builder
{
    [JWTAuthorize]
    [Route("/api/Builder")]
    public class BuilderController : Controller
    {
        private ISys_TableInfoService Service;
        private ISys_TableInfoRepository _repository;
        public BuilderController(ISys_TableInfoService service, ISys_TableInfoRepository repository)
        {
            Service = service;
            _repository = repository;
        }
        [HttpPost]
        [Route("GetTableTree")]
        //[ApiActionPermission(ActionRolePermission.SuperAdmin)]
        public async Task<ActionResult> GetTableTree()
        {
            try
            {
                (string, string) builderInfo = await Service.GetTableTree();
                return Json(new { list = builderInfo.Item1, nameSpace = builderInfo.Item2 });
            }
            catch (Exception ex)
            {

                return Json(new { list = ex.Message + ex.StackTrace + ex.Source, nameSpace = ex.InnerException?.Message });
            }
        }

        [Route("CreateVuePage")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public ActionResult CreateVuePage([FromBody] Sys_TableInfo sysTableInfo, string vuePath, int tableId, string table)
        {
            return Content(Service.CreateVuePage(sysTableInfo, vuePath, tableId, table));
        }

        [Route("loadOptions")]

        [HttpPost]
        public ActionResult CreateVuePage(int tableId, string table)
        {
            var res = Service.CreateVuePage(null, null, tableId, table);

            return Json(new { status = res.Contains("fun"), content = res });
        }
        [Route("CreateModel")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public ActionResult CreateEntityModel([FromBody] Sys_TableInfo tableInfo)
        {
            return Content(Service.CreateEntityModel(tableInfo));
        }
        [Route("Save")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public ActionResult SaveEidt([FromBody] Sys_TableInfo tableInfo)
        {
            return Json(Service.SaveEidt(tableInfo));
        }
        [Route("CreateServices")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public ActionResult CreateServices(string tableName, string nameSpace, string foldername, bool? partial, bool? api)
        {
            return Content(Service.CreateServices(tableName, nameSpace, foldername, false, true));
        }
        [Route("LoadTableInfo")]
        [HttpPost]
        public ActionResult LoadTable(int parentId, string tableName, string columnCNName, string nameSpace, string foldername, int table_Id, bool isTreeLoad, string dbServer)
        {
            tableName = (tableName ?? "").Replace("，", ",");
            if (isTreeLoad || !tableName.Contains(','))
            {
                return Json(Service.LoadTable(parentId, tableName, columnCNName, nameSpace, foldername, table_Id, isTreeLoad, dbServer));
            }
            var tables = tableName.Split(",").Distinct();

            foreach (var table in tables)
            {
                var res = Service.LoadTable(parentId, table, table, nameSpace, foldername, 0, false, dbServer);

                WebResponseContent webResponse = res as WebResponseContent;
                if (webResponse == null || !webResponse.Status)
                {
                    Console.WriteLine(res.Serialize());
                }
                var tableInfo = webResponse.Data as Sys_TableInfo;
                string message = Service.CreateEntityModel(tableInfo);
                Console.WriteLine($"表[{table}]生成實體類:{message}");

                message = Service.CreateServices(table, nameSpace, foldername, false, true);
                Console.WriteLine($"表[{table}]生成業務類:{message}");
            }
            return Json(new { status = false, message = "批量生成完成,請刷新页面后配置查詢、编輯信息再點击生成页面" });
        }
        [Route("delTree")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public async Task<ActionResult> DelTree(int table_Id)
        {
            return Json(await Service.DelTree(table_Id));
        }
        [Route("syncTable")]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost]
        public async Task<ActionResult> SyncTable(string tableName)
        {
            return Json(await Service.SyncTable(tableName));
        }

        [Route("getDyTable")]
        [HttpPost]
        public async Task<ActionResult> GetDyTable()
        {
            var data = await _repository.FindAsIQueryable(x => x.DyPage == 1 || x.DyScript != null)
                   .Select(s => new { table = s.TableName, dyPage = s.DyPage == 1, dyScript = s.DyScript != null })
                   .ToListAsync();
            return Json(data);
        }

        [Route("getDyScript")]
        [HttpPost]
        public async Task<ActionResult> GetDyScript(string table)
        {
            var data = await _repository.FindAsIQueryable(x => x.TableName == table)
                   .Select(s => s.DyScript)
                   .FirstOrDefaultAsync();
            return Json(new { data });
        }
    }
}

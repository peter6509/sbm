using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core.CacheManager;
using VolPro.Core.Configuration;
using VolPro.Core.DBManager;
using VolPro.Core.Extensions;
using VolPro.Core.Filters;
using VolPro.Core.ManageUser;
using VolPro.Core.Utilities;
using VolPro.Entity.DomainModels;

namespace VolPro.WebApi.Controllers
{

    [JWTAuthorize, ApiController]
    [Route("api/db")]
    public class DbManagerController : Controller
    {
        [Route("exectue"), HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        public async Task<ActionResult> Exectue([FromBody] TextInfo info)
        {
            if (!AppSetting.UseDynamicShareDB)
            {
                return Content($"只有動態分庫才能執行脚本");
            }
            List<Task> tasks = new List<Task>();
            ConcurrentBag<string> result = new ConcurrentBag<string>();

            var list = DbCache.GetList().Select(s => new
            {
                s.DatabaseName,
                DbServiceName = s.DbServiceName,
                ConnectionString = DbCache.GetConnectionString(s)
            }).ToList();

            if (list.Count == 0)  
            {
                return Content($"没有配置數據庫");
            }
            var db = DbCache.GetList().First().Serialize().DeserializeObject<Sys_DbService>();

            db.DatabaseName = "DB_Empty";
            //如果數據庫都不在同一台服務器上，每個服務器都要有一個DB_Empty數據庫,這里也應该也要根據ip去重循環，待完
            
            list.Add(new
            {
                db.DatabaseName,
                DbServiceName = "空數據庫",
                ConnectionString = DbCache.GetConnectionString(db)
            });

            int errorCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                tasks.Add(Task.Run(() =>
                {
                    string text = $"{(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"))},實例：{item.DbServiceName},數據庫:{item.DatabaseName},";
                    try
                    {
                        DBServerProvider.GetSqlDapper(item.ConnectionString).SetTimeout(600).ExcuteNonQuery(info.Text, null);
                        text = text + "執行成功";
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        text = text + "執行失败," + ex.Message + ex.StackTrace;
                    }
                    result.Add(text);
                }));
                if (tasks.Count == 10)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
            result.Add($" --------{(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"))}({UserContext.Current.UserTrueName})------------ ");
            result.Add($"執行sql({UserContext.Current.UserTrueName})：{info.Text}");
            result.Add("==========================================");
            result.Add("\r\n");
            FileHelper.WriteFile("DbLogger//DbExecute".MapPath(), DateTime.Now.ToString("yyyy-MM-dd") + ".txt", string.Join("\r\n", result), true);

            return Content($"執行成功:{list.Count - errorCount}個,失败：{errorCount}個");
        }
    }

    public class TextInfo
    {
        public string Text { get; set; }

    }
}
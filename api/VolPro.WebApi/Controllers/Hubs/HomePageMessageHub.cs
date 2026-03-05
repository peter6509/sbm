using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Core.CacheManager;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Sys.IServices;

namespace VolPro.WebApi.Controllers.Hubs
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/aspnet/core/signalr/introduction?view=aspnetcore-3.1ss
    /// </summary>
    public class HomePageMessageHub : Hub
    {
        private readonly ICacheService _cacheService;


        //public static ConcurrentDictionary<string, string>  UserCache.ConnectionIds = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 构造 注入
        /// </summary>
        public HomePageMessageHub(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 建立連接時异步触發
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //Console.WriteLine($"建立連接{Context.ConnectionId}");
            UserCache.Add(Context);
            //添加到一個组下
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            //發送上線消息
            //await Clients.All.SendAsync("ReceiveHomePageMessage", 1, new { title = "系统消息", content = $"{Context.ConnectionId} 上線" });
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離開連接時异步触發
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //Console.WriteLine($"断開連接{Context.ConnectionId}");
            //从组中删除
            // await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            //可自行調用下線業務處理方法...

            await UserOffline();
            //發送下線消息
            //   await Clients.All.SendAsync("ReceiveHomePageMessage", 4, new { title = "系统消息", content = $"{Context.ConnectionId} 離線" });
            await base.OnDisconnectedAsync(ex);
        }


        /// <summary>
        /// 發送给指定的人
        /// </summary>
        /// <param name="username">BW_Core_System_user表的登陆帳號</param>
        /// <param name="message">發送的消息</param>
        /// <returns></returns>
        public async Task<bool> SendHomeMessage(string username, string title, string message)
        {
            await Clients.Clients(UserCache.GetCnnectionIds(username)).SendAsync("ReceiveHomePageMessage", new
            {
                //   username,
                title,
                message,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
            });
            return true;
        }

        /// <summary>
        /// 断開連接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserOffline()
        {
            UserCache.Remove(Context);
            await Task.CompletedTask;
            return true;
        }
    }
}

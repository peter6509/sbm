using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Core.ManageUser;

namespace VolPro.Core.Dashboard
{
    /// <summary>
    /// 自定義設置工作台参數
    /// </summary>
    public class DashboardFilter : IDashboardFilterMetaData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql">前端配置的sql或存储過程名稱</param>
        /// <param name="dbService">选择的數據庫</param>
        /// <param name="isProc">是否存储過程</param>
        /// <param name="date1">查詢日期1</param>
        /// <param name="date2">查詢日期1</param>
        /// <param name="filterType">過濾類型：今天、近7天...近一年等</param>
        /// <returns></returns>
        public (string sql, DynamicParameters parameters) OnActionExecuting(string sql, DynamicParameters parameters, string dbService, bool isProc, DateTime? date1, DateTime? date2, string filterType)
        {
            //根據不同的類型處理不同的值
            //if (isProc)
            //{
            //    if (sql == "存储過程名字")
            //    {
            //        sql = "";
            //    }
            //}

            ////手動在后台設置参數
            ////如：前端sql配置：select * from table where createId=@userId
            ////在這里就可以設置参數
            //parameters.Add("@userId", UserContext.Current.UserId);

            return (sql, parameters);
        }
    }
}

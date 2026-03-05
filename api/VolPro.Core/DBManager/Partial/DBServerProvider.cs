using System;
using System.Collections.Generic;
using System.Text;
using VolPro.Core.Configuration;
using VolPro.Core.Dapper;
using VolPro.Core.Enums;

namespace VolPro.Core.DBManager
{
    /// <summary>
    /// 2022.11.21增加其他數據庫(sqlserver、mysql、pgsql、oracle)連接配置说明
    /// 需要把两個DBServerProvider.cs文件都更新下
    /// </summary>
    public partial class DBServerProvider
    {
        //業務庫(與builderData.js、EFDbContext下的文件名相同)
        ///// <summary>
        ///// 單獨配置mysql數據庫
        ///// </summary>
        //public static ISqlDapper SqlDapperMySql
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MySql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperMySql.xx
        //    }
        //}


        ///// <summary>
        ///// 如果有多個不同的mysql數據庫，這里再加一個配置
        ///// </summary>
        //public static ISqlDapper SqlDapperMySql2
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key2");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MySql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperMySql2.xx
        //    }
        //}

        ///// <summary>
        ///// 單獨配置SqlServer數據庫
        ///// </summary>
        //public static ISqlDapper SqlDapperSqlServer
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MsSql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperSqlServer.xx
        //    }
        //}
    }
}

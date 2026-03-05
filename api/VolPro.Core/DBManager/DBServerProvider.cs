using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VolPro.Core.Configuration;
using VolPro.Core.Const;
using VolPro.Core.Dapper;
using VolPro.Core.EFDbContext;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using MySqlConnector;
using VolPro.Core.ManageUser;
using VolPro.Entity.SystemModels;
using VolPro.Entity;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using gridreport;
using System.Data.Common;


namespace VolPro.Core.DBManager
{
    public partial class DBServerProvider
    {
        private static Dictionary<string, string> ConnectionPool = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //系统庫
        private static readonly string DefaultConnName = "SysDbContext";
        //業務庫(與builderData.js、EFDbContext下的文件名相同)
        private static readonly string _service = "ServiceDbContext";


        //業務庫(與builderData.js、EFDbContext下的文件名相同)
        private static readonly string _test = "TestDbContext";


        //這里必须與配置文件里面添加的屬性相同
        private static readonly string 自定義名字 = "自定義DbContext";




        static DBServerProvider()
        {
            SetConnection(DefaultConnName, AppSetting.DbConnectionString);
            //初始化業務庫數據庫链接
            SetConnection(_service, AppSetting.GetSection("Connection")[_service]);

            //初始化業務庫數據庫链接
            SetConnection(_test, AppSetting.GetSection("Connection")[_test]);

            SetConnection(自定義名字, AppSetting.GetSection("Connection")[自定義名字]);


        }

        public static void SetConnection(string key, string val)
        {
            if (ConnectionPool.ContainsKey(key))
            {
                ConnectionPool[key] = val;
                return;
            }
            ConnectionPool.Add(key, val);
        }
        /// <summary>
        /// 設置默認數據庫連接
        /// </summary>
        /// <param name="val"></param>
        public static void SetDefaultConnection(string val)
        {
            SetConnection(DefaultConnName, val);
        }

        public static string GetConnectionString(string key)
        {
            key = key ?? DefaultConnName;
            if (ConnectionPool.ContainsKey(key))
            {
                return ConnectionPool[key];
            }
            return key;
        }
        /// <summary>
        /// 获取默認數據庫連接
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(DefaultConnName);
        }

        /// <summary>
        /// 扩展dapper 获取MSSQL數據庫DbConnection，默認系统获取配置文件的DBType數據庫類型，
        /// </summary>
        /// <param name="connString">如果connString為null 執行重载GetDbConnection(string connString = null)</param>
        /// <param name="dapperType">指定連接數據庫的類型：MySql/MsSql/PgSql</param>
        /// <returns></returns>
        public static DbConnection GetDbConnection(string connString = null, DbCurrentType dbCurrentType = DbCurrentType.Default)
        {
            //默認获取DbConnection
            if (connString == null)
            {
                connString = ConnectionPool[DefaultConnName];
            }
            if (dbCurrentType == DbCurrentType.Default)
            {
                dbCurrentType = (DbCurrentType)Enum.Parse(typeof(DbCurrentType), DBType.Name);
            }
            if (dbCurrentType == DbCurrentType.MySql)
            {
                return new MySqlConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.PgSql)
            {
                return new NpgsqlConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.Oracle)
            {
                return new OracleConnection(connString);
            }
            return new Microsoft.Data.SqlClient.SqlConnection(connString);

        }
        /// <summary>
        /// 获取系统 EF
        /// </summary>
        public static SysDbContext DbContext
        {
            get { return GetEFDbContext(); }
        }
        /// <summary>
        /// 获取系统 EF
        /// </summary>
        public static SysDbContext GetEFDbContext()
        {
            return GetEFDbContext(null);
        }
        /// <summary>
        /// 获取系统 EF
        /// </summary>
        public static SysDbContext GetEFDbContext(string dbName)
        {
            SysDbContext dbContext = Utilities.HttpContext.Current.RequestServices.GetService(typeof(SysDbContext)) as SysDbContext;
            if (dbName != null)
            {
                if (!ConnectionPool.ContainsKey(dbName))
                {
                    throw new Exception("數據庫連接名稱错误");
                }
                dbContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
            }
            return dbContext;
        }
        /// <summary>
        /// 根據實體model获取對應EF
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static BaseDbContext GetEFDbContext<TEntity>()
        {
            string dbServer = typeof(TEntity).GetTypeCustomValue<EntityAttribute>(x => x.DBServer);

            return Utilities.HttpContext.Current.RequestServices.GetService(DbRelativeCache.GetDbContextType(dbServer)) as BaseDbContext;
        }

        public static void SetDbContextConnection(SysDbContext sysContext, string dbName)
        {
            if (!ConnectionPool.ContainsKey(dbName))
            {
                throw new Exception("數據庫連接名稱错误");
            }
            sysContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
        }
        public static ISqlDapper SqlMain
        {
            get
            {
                return SqlDapper;
            }
        }

        public static ISqlDapper SqlDapper
        {
            get
            {
                return new SqlDapper(DefaultConnName);
            }
        }
        /// <summary>
        /// 指定获取數據庫,這里同時支持mysql、sqlserver等不同類型數據庫2024.06.20
        /// 需要在appsettings.json中Connection添加xxxDbTyp:"MySql/SqlServe等屬性"
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public static ISqlDapper GetSqlDapperWidthDbService(string dbService)
        {
            //2024.06.20增加获取指定數據庫與指定數據庫類型
            string dbType = DbRelativeCache.GetDbType(dbService)??DBType.Name;
            return GetSqlDapper((DbCurrentType)Enum.Parse(typeof(DbCurrentType), dbType), dbService);
        }
        public static string GetDbEntityName(string dbServer)
        {
            return DbRelativeCache.GetDbEntityType(dbServer).Name;
        }
        /// <summary>
        /// 根據數據庫id或數據庫key获取链接
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static ISqlDapper GetSqlDapper(string dbName = null)
        {
            return new SqlDapper(string.IsNullOrEmpty(dbName) ? DefaultConnName : dbName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbCurrentType">指定數據庫類型：MySql/MsSql/PgSql</param>
        /// <param name="dbName">指定數據連串名稱</param>
        /// <returns></returns>
        public static ISqlDapper GetSqlDapper(DbCurrentType dbCurrentType, string dbName = null)
        {
            if (dbName != null && nameof(ServiceDbContext) == dbName && AppSetting.UseDynamicShareDB)
            {
                return GetSqlDapper(UserContext.CurrentServiceId.ToString());
            }
            return new SqlDapper(dbName ?? DefaultConnName, dbCurrentType);
        }
        public static ISqlDapper GetSqlDapper<TEntity>()
        {
            //获取實體真實的數據庫連接池對象名，如果不存在则用默認數據連接池名
            string dbName = typeof(TEntity).GetTypeCustomValue<EntityAttribute>(x => x.DBServer) ?? DefaultConnName;
            return GetSqlDapperWidthDbService(dbName);
        }

        public static string GetDbConnectionString(string key)
        {
            if (ConnectionPool.TryGetValue(key, out string connString))
            {
                return connString;
            }
            throw new Exception($"未配置[{key}]的數據庫連接");
        }



        /// <summary>
        /// 获取系统庫的字符串連接
        /// </summary>
        public static string SysConnectingString
        {
            get { return GetDbConnectionString(DefaultConnName); }
        }

        /// <summary>
        /// 获取業務庫的字符串連接
        /// </summary>
        public static string ServiceConnectingString
        {
            get
            {
                //動態无限分庫获取用户當前选择的數據庫
                if (AppSetting.UseDynamicShareDB)
                {
                    return GetDbConnectionString(UserContext.CurrentServiceId.ToString());
                }
                return GetDbConnectionString(_service);
            }
        }

        /// <summary>
        /// 获取測試庫的字符串連接
        /// </summary>
        public static string ServiceTestString
        {
            get
            {
                return GetDbConnectionString(_test);
            }
        }


        /// <summary>
        /// 获取測試庫的字符串連接
        /// </summary>
        public static string 自定義String
        {
            get
            {
                return GetDbConnectionString(自定義名字);
            }
        }


        //其他數據庫链接配置

    }
}

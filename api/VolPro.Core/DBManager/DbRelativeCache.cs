using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using VolPro.Core.Configuration;
using VolPro.Core.Const;
using VolPro.Core.Dapper;
using VolPro.Core.EFDbContext;
using VolPro.Core.ManageUser;
using VolPro.Entity.SystemModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VolPro.Core.DBManager
{
    public static class DbRelativeCache
    {
        private static Dictionary<string, Type> DbContextTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Type> DbEntityTypes = new Dictionary<string, Type>();
        private static Dictionary<string, string> DbTypes = new Dictionary<string, string>();

        /// <summary>
        /// 所有數據庫链接字符串
        /// </summary>
        private static Dictionary<string, string> DbConnection = new Dictionary<string, string>();

        static DbRelativeCache()
        {
            InitDbContextType();
            InitDbEntityType();
        }
        /// <summary>
        /// 缓存分庫DbContext
        /// </summary>
        public static void InitDbContextType()
        {
            var compilationLibrary = DependencyContext
                 .Default
                 .RuntimeLibraries
                 .Where(x => x.Name.EndsWith(".Core") && !x.Serviceable && x.Type != "package" && x.Type == "project");
            foreach (var _compilation in compilationLibrary)
            {
                //加载指定類
                foreach (var item in AssemblyLoadContext.Default
                .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                .GetTypes().Where(x => x.GetTypeInfo().BaseType != null
                && x.BaseType == (typeof(BaseDbContext))))
                {
                    DbContextTypes[item.Name] = item;
                    //获取數據庫链接類型,在appsettings.json中Connection屬性添加xxxDbType，前缀與數據庫链接一样
                    //ServiceDbContext:"數據庫链接字符"=>ServiceDbType:"MsSql";數據庫链接類型

                    string typeName = item.Name.Replace("DbContext", "").Replace("Entity", "") + "DbType";
                    string dbType = AppSetting.GetSection("Connection")[typeName];
                    DbTypes[item.Name] = dbType ?? DBType.Name;
                    //缓存數據庫链接
                    string connectionString = AppSetting.GetSection("Connection")[item.Name];
                    DbConnection.TryAdd(item.Name, connectionString);
                }
            }
            //缓存系统數據庫链接
            DbConnection[nameof(SysDbContext)] = AppSetting.GetSection("Connection")["DbConnectionString"];
        }
        /// <summary>
        /// 缓存分庫model基類
        /// </summary>
        public static void InitDbEntityType()
        {
            var compilationLibrary = DependencyContext
                 .Default
                 .RuntimeLibraries
                 .Where(x => x.Name.EndsWith(".Entity") && !x.Serviceable && x.Type != "package" && x.Type == "project");
            foreach (var _compilation in compilationLibrary)
            {
                //加载指定類
                foreach (var item in AssemblyLoadContext.Default
                .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                .GetTypes().Where(x => x.GetTypeInfo().BaseType != null
                && x.BaseType == (typeof(BaseEntity))))
                {
                    DbEntityTypes[item.Name] = item;
                }
            }
        }
        /// <summary>
        /// 获取數據庫的链接類型。如數據庫是mysql还是pgsql類型
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public static string GetDbType(string dbService)
        {
            if (string.IsNullOrEmpty(dbService))
            {
                return null;
            }
            DbTypes.TryGetValue(dbService, out string value);
            return value;
        }
        /// <summary>
        /// 根據分庫名稱获取dbcontext
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public static Type GetDbContextType(string dbService)
        {
            return DbContextTypes[dbService];
        }

        /// <summary>
        /// 根據分庫名稱获取分庫model基類
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public static Type GetDbEntityType(string dbService)
        {
            Type dbContextType = DbContextTypes[dbService];
            string name = dbContextType.Name.Replace("DbContext", "");
            return DbEntityTypes[$"{name}Entity"];
        }

        /// <summary>
        /// 根據dbtype获取數據庫链接
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        public static string GetDbConnectionString(Type dbContextType)
        {
            return GetDbConnectionString(dbContextType.GetType().Name);
        }
        /// <summary>
        /// 根據dbtype获取數據庫链接
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        public static string GetDbConnectionString(string dbContextType)
        {
            if (dbContextType==null)
            {
                return null;
            }
            DbConnection.TryGetValue(dbContextType, out string value);
            if (dbContextType == nameof(ServiceDbContext))
            {
                if (AppSetting.UseDynamicShareDB)
                {
                    return  DBServerProvider.GetDbConnectionString(UserContext.CurrentServiceId.ToString());
                }
            }
            return value;
        }
    }
}

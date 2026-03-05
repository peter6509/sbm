/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_DashboardService與ISys_DashboardService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_DashboardService : ServiceBase<Sys_Dashboard, ISys_DashboardRepository>
    , ISys_DashboardService, IDependency
    {
    public static ISys_DashboardService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DashboardService>(); } }
    }
 }

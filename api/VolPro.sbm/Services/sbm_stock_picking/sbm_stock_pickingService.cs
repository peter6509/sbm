/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_stock_pickingService與Isbm_stock_pickingService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_stock_pickingService : ServiceBase<sbm_stock_picking, Isbm_stock_pickingRepository>
    , Isbm_stock_pickingService, IDependency
    {
    public static Isbm_stock_pickingService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_stock_pickingService>(); } }
    }
 }

/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_stockmoveService與Isbm_stockmoveService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_stockmoveService : ServiceBase<sbm_stockmove, Isbm_stockmoveRepository>
    , Isbm_stockmoveService, IDependency
    {
    public static Isbm_stockmoveService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_stockmoveService>(); } }
    }
 }

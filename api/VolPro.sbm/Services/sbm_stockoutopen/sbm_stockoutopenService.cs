/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_stockoutopenService與Isbm_stockoutopenService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_stockoutopenService : ServiceBase<sbm_stockoutopen, Isbm_stockoutopenRepository>
    , Isbm_stockoutopenService, IDependency
    {
    public static Isbm_stockoutopenService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_stockoutopenService>(); } }
    }
 }

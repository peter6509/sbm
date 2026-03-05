/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_buService與Isbm_buService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_buService : ServiceBase<sbm_bu, Isbm_buRepository>
    , Isbm_buService, IDependency
    {
    public static Isbm_buService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_buService>(); } }
    }
 }

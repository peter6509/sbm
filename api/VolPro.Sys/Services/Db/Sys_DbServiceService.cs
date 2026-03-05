/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_DbServiceService與ISys_DbServiceService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_DbServiceService : ServiceBase<Sys_DbService, ISys_DbServiceRepository>
    , ISys_DbServiceService, IDependency
    {
    public Sys_DbServiceService(ISys_DbServiceRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_DbServiceService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DbServiceService>(); } }
    }
 }

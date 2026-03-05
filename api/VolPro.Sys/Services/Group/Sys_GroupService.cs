/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_GroupService與ISys_GroupService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_GroupService : ServiceBase<Sys_Group, ISys_GroupRepository>
    , ISys_GroupService, IDependency
    {
    public Sys_GroupService(ISys_GroupRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_GroupService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_GroupService>(); } }
    }
 }

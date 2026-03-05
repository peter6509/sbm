/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_UserService與ISys_UserService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_UserService : ServiceBase<Sys_User, ISys_UserRepository>
    , ISys_UserService, IDependency
    {
    public Sys_UserService(ISys_UserRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_UserService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_UserService>(); } }
    }
 }

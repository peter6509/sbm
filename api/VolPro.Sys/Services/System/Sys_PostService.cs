/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_PostService與ISys_PostService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_PostService : ServiceBase<Sys_Post, ISys_PostRepository>
    , ISys_PostService, IDependency
    {
    public static ISys_PostService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_PostService>(); } }
    }
 }

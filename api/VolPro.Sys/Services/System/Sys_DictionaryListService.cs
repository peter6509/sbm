/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Sys_DictionaryListService與ISys_DictionaryListService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class Sys_DictionaryListService : ServiceBase<Sys_DictionaryList, ISys_DictionaryListRepository>
    , ISys_DictionaryListService, IDependency
    {
    public Sys_DictionaryListService(ISys_DictionaryListRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_DictionaryListService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DictionaryListService>(); } }
    }
 }

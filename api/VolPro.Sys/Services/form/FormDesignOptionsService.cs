/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下FormDesignOptionsService與IFormDesignOptionsService中编写
 */
using VolPro.Sys.IRepositories;
using VolPro.Sys.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Services
{
    public partial class FormDesignOptionsService : ServiceBase<FormDesignOptions, IFormDesignOptionsRepository>
    , IFormDesignOptionsService, IDependency
    {
    public FormDesignOptionsService(IFormDesignOptionsRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IFormDesignOptionsService Instance
    {
      get { return AutofacContainerModule.GetService<IFormDesignOptionsService>(); } }
    }
 }

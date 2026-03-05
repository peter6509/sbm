/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Demo_ProductService與IDemo_ProductService中编写
 */
using VolPro.DbTest.IRepositories;
using VolPro.DbTest.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.DbTest.Services
{
    public partial class Demo_ProductService : ServiceBase<Demo_Product, IDemo_ProductRepository>
    , IDemo_ProductService, IDependency
    {
    public Demo_ProductService(IDemo_ProductRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_ProductService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_ProductService>(); } }
    }
 }

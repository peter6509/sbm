/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下Demo_GoodsService與IDemo_GoodsService中编写
 */
using VolPro.DbTest.IRepositories;
using VolPro.DbTest.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.DbTest.Services
{
    public partial class Demo_GoodsService : ServiceBase<Demo_Goods, IDemo_GoodsRepository>
    , IDemo_GoodsService, IDependency
    {
    public Demo_GoodsService(IDemo_GoodsRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IDemo_GoodsService Instance
    {
      get { return AutofacContainerModule.GetService<IDemo_GoodsService>(); } }
    }
 }

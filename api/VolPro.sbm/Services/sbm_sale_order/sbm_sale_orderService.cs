/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_sale_orderService與Isbm_sale_orderService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_sale_orderService : ServiceBase<sbm_sale_order, Isbm_sale_orderRepository>
    , Isbm_sale_orderService, IDependency
    {
    public static Isbm_sale_orderService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_sale_orderService>(); } }
    }
 }

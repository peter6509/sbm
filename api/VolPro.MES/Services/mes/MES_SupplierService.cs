/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下MES_SupplierService與IMES_SupplierService中编写
 */
using VolPro.MES.IRepositories;
using VolPro.MES.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.MES.Services
{
    public partial class MES_SupplierService : ServiceBase<MES_Supplier, IMES_SupplierRepository>
    , IMES_SupplierService, IDependency
    {
    public static IMES_SupplierService Instance
    {
      get { return AutofacContainerModule.GetService<IMES_SupplierService>(); } }
    }
 }

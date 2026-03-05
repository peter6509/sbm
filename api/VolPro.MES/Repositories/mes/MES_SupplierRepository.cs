/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾MES_SupplierRepository编写代碼
 */
using VolPro.MES.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.MES.Repositories
{
    public partial class MES_SupplierRepository : RepositoryBase<MES_Supplier> , IMES_SupplierRepository
    {
    public MES_SupplierRepository(ServiceDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IMES_SupplierRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IMES_SupplierRepository>(); } }
    }
}

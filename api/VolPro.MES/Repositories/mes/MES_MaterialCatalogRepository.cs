/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾MES_MaterialCatalogRepository编写代碼
 */
using VolPro.MES.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.MES.Repositories
{
    public partial class MES_MaterialCatalogRepository : RepositoryBase<MES_MaterialCatalog> , IMES_MaterialCatalogRepository
    {
    public MES_MaterialCatalogRepository(ServiceDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IMES_MaterialCatalogRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IMES_MaterialCatalogRepository>(); } }
    }
}

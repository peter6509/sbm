/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾sbm_stock_pickingRepository编写代碼
 */
using VolPro.sbm.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Repositories
{
    public partial class sbm_stock_pickingRepository : RepositoryBase<sbm_stock_picking> , Isbm_stock_pickingRepository
    {
    public sbm_stock_pickingRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static Isbm_stock_pickingRepository Instance
    {
      get {  return AutofacContainerModule.GetService<Isbm_stock_pickingRepository>(); } }
    }
}

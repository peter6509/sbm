/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾Demo_ProductSizeRepository编写代碼
 */
using VolPro.DbTest.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.DbTest.Repositories
{
    public partial class Demo_ProductSizeRepository : RepositoryBase<Demo_ProductSize> , IDemo_ProductSizeRepository
    {
    public Demo_ProductSizeRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IDemo_ProductSizeRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IDemo_ProductSizeRepository>(); } }
    }
}

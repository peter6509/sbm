/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾Demo_ProductRepository编写代碼
 */
using VolPro.DbTest.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.DbTest.Repositories
{
    public partial class Demo_ProductRepository : RepositoryBase<Demo_Product> , IDemo_ProductRepository
    {
    public Demo_ProductRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IDemo_ProductRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IDemo_ProductRepository>(); } }
    }
}

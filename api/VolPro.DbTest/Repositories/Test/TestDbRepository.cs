/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾TestDbRepository编写代碼
 */
using VolPro.DbTest.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.DbTest.Repositories
{
    public partial class TestDbRepository : RepositoryBase<TestDb> , ITestDbRepository
    {
    public TestDbRepository(TestDbContext dbContext)
    : base(dbContext)
    {

    }
    public static ITestDbRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ITestDbRepository>(); } }
    }
}

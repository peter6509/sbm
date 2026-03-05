/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾Sys_UserRepository编写代碼
 */
using VolPro.Sys.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.Repositories
{
    public partial class Sys_UserRepository : RepositoryBase<Sys_User> , ISys_UserRepository
    {
    public Sys_UserRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_UserRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_UserRepository>(); } }
    }
}

/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾sbm_partnerRepository编写代碼
 */
using VolPro.sbm.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Repositories
{
    public partial class sbm_partnerRepository : RepositoryBase<sbm_partner> , Isbm_partnerRepository
    {
    public sbm_partnerRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static Isbm_partnerRepository Instance
    {
      get {  return AutofacContainerModule.GetService<Isbm_partnerRepository>(); } }
    }
}

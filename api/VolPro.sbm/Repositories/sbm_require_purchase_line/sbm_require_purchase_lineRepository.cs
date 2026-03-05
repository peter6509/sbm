/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾sbm_require_purchase_lineRepository编写代碼
 */
using VolPro.sbm.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Repositories
{
    public partial class sbm_require_purchase_lineRepository : RepositoryBase<sbm_require_purchase_line> , Isbm_require_purchase_lineRepository
    {
    public sbm_require_purchase_lineRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static Isbm_require_purchase_lineRepository Instance
    {
      get {  return AutofacContainerModule.GetService<Isbm_require_purchase_lineRepository>(); } }
    }
}

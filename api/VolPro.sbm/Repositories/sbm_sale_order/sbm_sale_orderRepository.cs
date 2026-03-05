/*
 *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
 *Repository提供數據庫操作，如果要增加數據庫操作請在當前目錄下Partial文件夾sbm_sale_orderRepository编写代碼
 */
using VolPro.sbm.IRepositories;
using VolPro.Core.BaseProvider;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Repositories
{
    public partial class sbm_sale_orderRepository : RepositoryBase<sbm_sale_order> , Isbm_sale_orderRepository
    {
    public sbm_sale_orderRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static Isbm_sale_orderRepository Instance
    {
      get {  return AutofacContainerModule.GetService<Isbm_sale_orderRepository>(); } }
    }
}

/*
 *所有关于MES_ProductionOrder類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*MES_ProductionOrderService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
*/
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;
using System.Linq;
using VolPro.Core.Utilities;
using System.Linq.Expressions;
using VolPro.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using VolPro.MES.IRepositories;

namespace VolPro.MES.Services
{
    public partial class MES_ProductionOrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMES_ProductionOrderRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public MES_ProductionOrderService(
            IMES_ProductionOrderRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }
        public override PageGridData<MES_ProductionOrder> GetPageData(PageDataOptions options)
        {
            SummaryExpress = (IQueryable<MES_ProductionOrder> queryable) =>
            {
                return queryable.GroupBy(x => 1).Select(x => new
                {
                    //AvgPrice注意大小写和數據庫字段大小写一样
                    OrderQty = x.Sum(o => o.OrderQty)
                })
                .FirstOrDefault();
            };
            return base.GetPageData(options);
        }
        protected override object GetDetailSummary<Detail>(IQueryable<Detail> queryeable)
        {

            //ef写法（需要與前端開發文档上的【table顯示合计】一起使用）
            return ((IQueryable<MES_ProductionPlanDetail>)queryeable).GroupBy(x => 1).Select(x => new
            {
                //Weight/Qty注意大小写和數據庫字段大小写一样
                PlanQuantity = x.Sum(o => o.PlanQuantity),

            }).FirstOrDefault();
        }
    }
}

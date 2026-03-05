/*
 *所有关于Demo_Product類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*Demo_ProductService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
using VolPro.DbTest.IRepositories;

namespace VolPro.DbTest.Services
{
    public partial class Demo_ProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_ProductRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Demo_ProductService(
            IDemo_ProductRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }


        /// <summary>
        /// 主表設置合计
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override PageGridData<Demo_Product> GetPageData(PageDataOptions options)
        {
            //查詢table界面顯示求和
            SummaryExpress = (IQueryable<Demo_Product> queryable) =>
            {
                return queryable.GroupBy(x => 1).Select(x => new
                {
                    //注意大小写和數據庫字段大小写一样
                    Price = x.Sum(o => o.Price)
                })
                .ToList().FirstOrDefault();
            };

            return base.GetPageData(options);
        }


        /// <summary>
        /// 設置弹出框明细表的合计信息
        /// </summary>
        /// <typeparam name="detail"></typeparam>
        /// <param name="queryeable"></param>
        /// <returns></returns> 
        protected override object GetDetailSummary<Detail>(IQueryable<Detail> queryeable)
        {
            //判断是哪個明细表
            if (typeof(Detail) ==typeof(Demo_ProductColor))
            {
                //转換為明细表
                return (queryeable as IQueryable<Demo_ProductColor>).GroupBy(x => 1).Select(x => new
                {
                    //Qty注意大小写和數據庫字段大小写一样
                //    Qty = x.Average(o => o.Qty)
                }).ToList().FirstOrDefault();
            }
            return null;
        }

    }
}

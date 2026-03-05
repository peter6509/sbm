/*
 *所有关于Demo_Order類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*Demo_OrderService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
using System.Diagnostics.CodeAnalysis;

namespace VolPro.DbTest.Services
{
    public partial class Demo_OrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_OrderRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Demo_OrderService(
            IDemo_OrderRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }

        public override PageGridData<Demo_Order> GetPageData(PageDataOptions options)
        {
            QueryRelativeExpression = (IQueryable<Demo_Order> queryable) =>
            {
                if (options.Value?.ToString() == "empty")
                {
                    return queryable.Where(x => x.TotalQty == null||x.TotalQty==0);
                }
                else if (options.Value != null)
                {
                    switch (options.Value.GetInt())
                    {
                        //新訂單
                        case 1:
                            queryable = queryable = queryable.Where(c => c.OrderType == 1);
                            break;
                        //采购訂單
                        case 2:
                            queryable = queryable = queryable.Where(c => c.OrderType == 2);
                            break;
                        //退货訂單
                        case 3:
                            queryable = queryable = queryable.Where(c => c.OrderType == 3);
                            break;
                        default:
                            break;
                    }
                }
                // queryable = queryable = queryable.Where(c => c.OrderType == options.Value.GetInt());

                //當前用户只能操作自己(與下级角色)創建的數據,如:查詢、删除、修改等操作
                //IQueryable<int> userQuery = RoleContext.GetCurrentAllChildUser();
                //queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId || userQuery.Contains(x.CreateID ?? 0));
                return queryable;
            };


            //查詢table界面顯示求和
            SummaryExpress = (IQueryable<Demo_Order> queryable) =>
            {
                return queryable.GroupBy(x => 1).Select(x => new
                {
                    //注意大小写和數據庫字段大小写一样
                    TotalPrice = x.Sum(o => o.TotalPrice),
                    TotalQty = x.Sum(o => o.TotalQty)
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
            return (queryeable as IQueryable<Demo_OrderList>).GroupBy(x => 1).Select(x => new
            {
                //Weight/Qty注意大小写和數據庫字段大小写一样
                Price = x.Average(o => o.Price),
                Qty = x.Sum(o => o.Qty)
            }).ToList().FirstOrDefault();
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {
            return base.Update(saveModel);
        }

        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            saveDataModel.MainData["OrderNo"] = "111";

            WebResponseContent webResponse = new WebResponseContent();
            // 在保存數據庫前的操作，所有數據都驗証通過了，這一步執行完就執行數據庫保存
            AddOnExecuting = (Demo_Order order, object list) =>
            {
                //生成訂單號
                // order.OrderNo = order.Create<Demo_Order>(x => x.OrderNo, "D", x => x.CreateDate);
                return webResponse.OK();
            };

            return base.Add(saveDataModel);
        }

        ///// <summary>
        ///// 自動生成訂單號
        ///// </summary>
        ///// <returns></returns>
        //public string GetOrderNo()
        //{
        //    //lock/redis自增
        //    DateTime dateNow = (DateTime)DateTime.Now.ToString("yyyy-MM-dd").GetDateTime();
        //    //查詢當天最新的訂單號
        //    string orderNo = repository.FindAsIQueryable(x => x.CreateDate > dateNow)
        //        .OrderByDescending(x => x.OrderNo)
        //        .Select(s => s.OrderNo)
        //        .FirstOrDefault();
        //    string rule = $"D{DateTime.Now.ToString("yyyyMMdd")}";
        //    if (string.IsNullOrEmpty(orderNo))
        //    {
        //        rule += "00001";
        //    }
        //    else
        //    {
        //        rule += (orderNo.Substring(orderNo.Length - 5).GetInt() + 1).ToString("00000");
        //    }
        //    return rule;
        //}

    }
}


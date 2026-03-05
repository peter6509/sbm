using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Quartz.Impl.AdoJobStore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolPro.Core.DBManager;
using VolPro.Core.EFDbContext;
using VolPro.Core.Extensions;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.Print
{
    public class PrintCustom : PrintFilter
    {
        public PrintCustom() { }
        /// <summary>
        /// 主表查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public override IQueryable<T> Query<T>(IQueryable<T> query, PrintQuery parms) where T : class
        {
            //判断是哪张主表自定義過濾条件
            if (typeof(T).Name == typeof(Demo_Order).Name)
            {
                var orderQuery = ((IQueryable<Demo_Order>)query);
                return orderQuery
                       //這里where可以写自定義条件
                       .Where(x => true) as IQueryable<T>;

            }
            return query;
        }
        /// <summary>
        /// 明细表查詢
        /// </summary>
        /// <typeparam name="Detail"></typeparam>
        /// <param name="query"></param>
        /// <param name="parms"></param>
        /// <returns></returns>

        public override IQueryable<Detail> QueryDetail<Detail>(IQueryable<Detail> query, PrintQuery parms) where Detail : class
        {
            //判断是哪张明细表自定義過濾条件
            if (typeof(Detail).Name == typeof(Demo_OrderList).Name)
            {
                var orderQuery = ((IQueryable<Demo_OrderList>)query);
                return orderQuery
                       //這里where可以写自定義条件
                       .Where(x => true) as IQueryable<Detail>;

            }
            return query;
        }

        /// <summary>
        /// 對返回的结果自定義處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Detail"></typeparam>
        /// <param name="result"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public override List<Dictionary<string, object>> QueryResult<T>(
            List<Dictionary<string, object>> result,
            PrintQuery parms,
            BaseDbContext dbContext)
        {
            if (result.Count == 0) return result;

            //判断表，自定義返回數據
            if (typeof(T).Name == typeof(Demo_Order).Name)
            {
                //判断是哪個打印模板，然后自定義返回數據
                if (parms.TemplateName == "訂單管理主从明细表打印")
                {
                    //返回DemoOrder表自定義配置
                    SetDemoOrderValue(result, parms, dbContext);
                }
            }





            if (typeof(T).Name == typeof(sbm_sale_order).Name)
            {
                //判断是哪個打印模板，然后自定義返回數據
                if (parms.TemplateName == "sbm_quotation")
                {
                    foreach (var row in result)
                    {
                        row["doc_title"] = "專業服務報價單";
                        row["custom_sign"] = "客戶簽名:";
                        row["sale_sign"] = "業務簽名:";
                        int companyId = 0;
                        if (!row.ContainsKey("company_id"))
                        {
                            var orderId = Convert.ToInt32(row["sale_id"]);

                            var dbCompanyId = dbContext.Set<sbm_sale_order>()
                                .Where(x => x.sale_id == orderId)
                                .Select(x => x.company_id)
                                .FirstOrDefault();

                            row["company_id"] = dbCompanyId;
                        }

                        Console.WriteLine($"company_id={row.GetValueOrDefault("company_id")}");
                     

                        if (row.TryGetValue("company_id", out var companyIdObj) && companyIdObj != null)
                        {
                            // 同時處理 int / long / decimal / string
                            companyId = Convert.ToInt32(companyIdObj);
                        }

                        row["company_title"] = companyId switch
                        {
                            1 => "連瑟科技股份有限公司",
                            2 => "歐度資訊有限公司",
                            3 => "創維資訊有限公司",
                            _ => "歐度資訊有限公司" // 預設
                        };
                       
                    }
                    //返回DemoOrder表自定義配置
                    //SetDemoOrderValue(result, parms, dbContext);
                }
            }

            if (typeof(T).Name == typeof(sbm_stock_picking).Name)
            {
                //判断是哪個打印模板，然后自定義返回數據
                if (parms.TemplateName == "sbm_stockout")
                {
                    foreach (var row in result)
                    {
                        row["doc_title"] = "出貨單";
                        row["custom_sign"] = "客戶簽名:";
                        row["sale_sign"] = "業務簽名";
                        int companyId = 0;
                        if (!row.ContainsKey("company_id"))
                        {
                            var pickingId = Convert.ToInt32(row["picking_id"]);

                            var dbCompanyId = dbContext.Set<sbm_stock_picking>()
                                .Where(x => x.picking_id == pickingId)
                                .Select(x => x.company_id)
                                .FirstOrDefault();

                            row["company_id"] = dbCompanyId;
                        }

                        Console.WriteLine($"company_id={row.GetValueOrDefault("company_id")}");


                        if (row.TryGetValue("company_id", out var companyIdObj) && companyIdObj != null)
                        {
                            // 同時處理 int / long / decimal / string
                            companyId = Convert.ToInt32(companyIdObj);
                        }

                        row["company_title"] = companyId switch
                        {
                            1 => "連瑟科技股份有限公司",
                            2 => "歐度資訊有限公司",
                            3 => "創維資訊有限公司",
                            _ => "歐度資訊有限公司" // 預設
                        };

                    }
                    //返回DemoOrder表自定義配置
                    ////SetDemoOrderValue(result, parms, dbContext);
                }
            }

            return result;
        }



        /// <summary>
        /// 返回DemoOrder表自定義配置
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parms"></param>
        /// <param name="dbContext"></param>
        private void SetDemoOrderValue(
            List<Dictionary<string, object>> result,
            PrintQuery parms,
            BaseDbContext dbContext)
        {
            //给明细表設置合计
            var data = dbContext.Set<Demo_OrderList>()
                  //根據主表id查詢返回明细表合计
                  .Where(x => x.Order_Id == parms.Ids[0].GetGuid())
                  .GroupBy(x => true)
                  .Select(s => new
                  {
                      單价合计 = s.Sum(c => c.Price),
                      數量合計 = s.Sum(c => c.Qty)
                  }).FirstOrDefault();

            //設置自定義返回的字段(模板設計页面需要定義：單价合计、數量合计兩個字段)
            result[0]["單价合计"] = data?.單价合计 ?? 0;
            result[0]["數量合计"] = data?.數量合計 ?? 0;

            //result[0]這里还可以自定義其他字段設置值與模板設計页面定義的字段一致即可

            ////例如：再返回一些自定義的表格數據
            //var otherTable = dbContext.Set<Demo_Product>()
            //    .Where(x => true)//這里写条件
            //    .Select(s => new
            //    {
            //        名稱 = s.ProductName,//[名稱]與[编號]是打印板自定義表格的里面输入的字段名
            //        编號 = s.ProductCode
            //    }).ToList();

            //result[0]["字段名"] = otherTable;
        }
    }
}

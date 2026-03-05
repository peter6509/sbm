using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Swashbuckle.AspNetCore.SwaggerGen;
using VolPro.Core.Configuration;
using VolPro.Core.Extensions;
using VolPro.Core.Filters;
using VolPro.Core.Language;
//using VolPro.Core.KafkaManager.IService;
//using VolPro.Core.KafkaManager.Service;
using VolPro.Core.Middleware;
using VolPro.Core.ObjectActionValidator;
using VolPro.Core.Quartz;
using VolPro.Core.Utilities.PDFHelper;
using VolPro.Core.WorkFlow;
using VolPro.Entity.DomainModels;
using VolPro.WebApi.Controllers.Hubs;
using VolPro.Core.Controllers.DynamicController;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VolPro.Core.Print;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using VolPro.Core.Controllers.Basic;

namespace VolPro.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureContainer()
        {
            //Services.AddModule(builder, Configuration);
            //初始化流程表，表里面必须有AuditStatus字段
            WorkFlowContainer.Instance
                .Use<MES_ProductionReporting, MES_ProductionReportingDetail>(
                 "生產報工",
                    filterFields: x => new {x.ReportingNumber, x.AcceptedQuantity, x.RejectedQuantity, x.Total, x.ReportedBy, x.ReportingTime },
                    //審批界面顯示表數據字段
                    formFields: x => new { x.ReportedBy, x.ReportingNumber, x.ReportingTime, x.AcceptedQuantity, x.RejectedQuantity, x.Total },
                    //明细表顯示的數據
                    formDetailFields: x => new { x.MaterialCode, x.MaterialName, x.MaterialSpecification, x.ReportedQuantity, x.RejectedQuantity, x.ReportHour }
                )
                .Use<Demo_Order, Demo_OrderList>("訂單管理",
                    //過濾条件字段
                    filterFields: x => new { x.OrderDate, x.OrderNo, x.OrderStatus, x.OrderType, x.Creator, x.CustomerId },
                    //審批界面顯示表數據字段
                    formFields: x => new { x.OrderDate, x.OrderNo, x.OrderStatus, x.Remark },
                    //明细表顯示的數據
                    formDetailFields: x => new { x.GoodsName, x.GoodsCode, x.Qty, x.Price, x.Specs, x.Img, x.CreateDate },
                    //defaultAduitStatus: AuditStatus.草稿,
                    //可编輯的字段(字段必须在上面的formFields内)
                    editFields: x => new { x.OrderDate, x.OrderNo, x.OrderStatus, x.OrderType }
                )
                 .Use<sbm_sale_order, sbm_sale_order_line>("sbm訂單管理",
                    //過濾条件字段
                    filterFields: x => new { x.name, x.sale_date, x.partner_company, x.partner_contact, x.sales},
                    //審批界面顯示表數據字段
                    formFields: x => new { x.name, x.sale_date, x.partner_company, x.partner_contact, x.sales, x.untax_amount, x.tax, x.tot_amount },
                    //明细表顯示的數據
                    formDetailFields: x => new { x.sale_item, x.prod_name, x.prod_num, x.prod_uom, x.price_unit, x.price_sub },
                    //defaultAduitStatus: AuditStatus.草稿,
                    //可编輯的字段(字段必须在上面的formFields内)
                    editFields: x => new { x.name, x.sale_date, x.partner_company, x.partner_contact,x.sales }
                )
                .Use<Demo_Product>(
                    "產品管理",
                    filterFields: x => new { x.Creator, x.Price, x.ProductCode, x.ProductName },
                    formFields: x => new { x.ProductCode, x.ProductName, x.Remark, x.Price, x.Creator, x.CreateDate }
                    )
                .Run();

            /**********************************打印配置****************************************************/
            PrintContainer.Instance
                 /*****************[全国城市]單表打印*****************/
                 .Use<MES_Material>(
                   //主表配置
                   name: "物料管理",
                   //主表可以打印的字段
                   printFields: x => new { x.MaterialCode, x.MaterialName, x.MaterialType, x.CatalogID, x.Img }
                 )
                   .Use<MES_ProductionReporting, MES_ProductionReportingDetail>(
                 "生產報工",
                    printFields: x => new {x.ReportingNumber,x.ReportingTime, x.AcceptedQuantity, x.RejectedQuantity, x.ReportedBy, x.Total },
                    //明细表配置
                   detailName: "報工明细",
                    //明细表顯示的數據
                    detailPrintFields: x => new { x.MaterialCode, x.MaterialName, x.MaterialSpecification, x.ReportedQuantity, x.RejectedQuantity, x.ReportHour }
                )
               .Use<MES_ProductionOrder, MES_ProductionPlanDetail>(
                 "生產訂單",
                    printFields: x => new { x.OrderNumber, x.OrderQty, x.OrderStatus,x.LV, x.OrderDate },
                   //明细表配置
                   detailName: "訂單明细",
                    //明细表顯示的數據
                    detailPrintFields: x => new { x.MaterialCode, x.MaterialName, x.MaterialSpecification, x.PlanQuantity, x.Unit}
                )
                  .Use<MES_Process, MES_ProcessRoute>(
                 "工序管理",
                    printFields: x => new { x.ProcessCode, x.ProcessName, x.ProcessType, x.ProcessStatus },
                   //明细表配置
                   detailName: "工藝路線",
                    //明细表顯示的數據
                    detailPrintFields: x => new { x.ProductName, x.RouteSequence, x.RouteResponsible, x.PreProcessID, x.NextProcessID }
                )
                 /*****************[全国城市]單表打印*****************/
                 .Use<Sys_Region>(
                   //主表配置
                   name: "地址打印管理",
                   //主表可以打印的字段
                   printFields: x => new { x.name, x.code, x.mername, x.pinyin, x.Lat, x.Lng }
                 )
                 //主从表同時打印(注意Use第一個参數是主表，第二個明细表)
                 .Use<Demo_Order, Demo_OrderList>(
                   //主表配置
                   name: "訂單打印管理",
                   //主表可以打印的字段
                   printFields: x => new { x.OrderNo, x.OrderStatus, x.OrderType, x.OrderDate, x.TotalPrice, x.TotalQty },

                   //明细表配置
                   detailName: "訂單詳情",
                   //明细表可以打印的字段
                   detailPrintFields: c => new { c.GoodsCode, c.GoodsName, c.Specs, c.Price, c.Qty }
                 )
                 /*****************[訂單表]打印配置(主从表明细表(一對一))*****************/
                 .Use<Demo_Order, Demo_OrderList>(
                   //主表配置
                   name: "訂單打印管理",
                   //主表可以打印的字段
                   printFields: x => new { x.OrderNo, x.OrderStatus, x.OrderType, x.OrderDate, x.TotalPrice, x.TotalQty },
                   //主表自定義打印的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                   customFields: new List<CustomField>() {
                            new CustomField() { Name="自定義1",   Field="test1"  } ,
                            new CustomField() {  Name="自定義2",  Field="test2" }
                    },
                   //明细表配置
                   detail: new PrintDetailOptions<Demo_OrderList>()
                   {
                       Name = "訂單詳情",
                       PrintFields = c => new { c.GoodsCode, c.GoodsName, c.Img, c.Specs, c.Price, c.Qty },
                       //明细表自定義的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                       CustomFields = new List<CustomField>() {
                            new CustomField() { Name="明细自定義1",   Field="test1"  } ,
                            new CustomField() { Name="明细自定義2",  Field="test2" }
                       }
                   }
                 )
                 /*****************[產品表]打印配置(一對多打印)*****************/
                 .Use<Demo_Product, Demo_ProductSize, Demo_ProductColor>(
                       //主表配置
                       name: "一對多打印測試",
                       //主表可以打印的字段
                       printFields: x => new { x.ProductName, x.ProductCode, x.AuditStatus, x.Price, x.Remark, x.CreateDate },
                       //主表自定義打印的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                       customFields: null,//配置同上
                                          //明细表[產品尺寸]打印配置
                       detail1: new PrintDetailOptions<Demo_ProductSize>()
                       {
                           Name = "產品尺寸",
                           //明细表打印的字段
                           PrintFields = x => new { x.ProductId, x.Size, x.Unit, x.Remark, x.Creator, x.CreateDate },
                           //明细表自定義的字段,需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                           CustomFields = null //自定義字段同上配置一样
                       },
                       //明细表[產品颜色]打印配置
                       detail2: new PrintDetailOptions<Demo_ProductColor>()
                       {
                           Name = "產品颜色",
                           //明细表打印的字段
                           PrintFields = x => new { x.ProductId, x.Img, x.Color, x.Qty, x.Unit, x.Remark, x.Creator, x.CreateDate },
                           //明细表自定義的字段需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                           CustomFields = null //自定義字段同上配置一样
                       }
                )

            .Use<sbm_sale_order, sbm_sale_order_line>(
                   //主表配置
                   name: "sbm訂單打印管理",
                   //主表可以打印的字段
                   printFields: x => new { x.name, x.sale_date, x.partner_company, x.partner_contact, x.sales, x.untax_amount,x.tax,x.tot_amount,x.Creator,x.description,x.sales_mobile,x.discount_amount,x.partner_mobile,x.partner_email,x.sales_email,x.bu_logo,x.bu_invoice_stamp,x.bu_bank_info},
                                       customFields: null,//配置同上
                                       //主表自定義打印的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                                       //customFields: new List<CustomField>() {
                                       //     new CustomField() { Name="專業服務報價單",  Field="doc_title" } ,
                                       //         new CustomField() { Name="條形碼",  Field="barcode" }
                                       //         new CustomField() { Name="二維碼",  Field="qrcode" },
                                       //         new CustomField() { Name="明细自定義1",   Field="test1"  } ,
                                       //         new CustomField() { Name="明细自定義2",  Field="test2" }
                                       // },
                                       //明细表配置
                   detail: new PrintDetailOptions<sbm_sale_order_line>()
                   {
                       Name = "訂單明細",
                       PrintFields = c => new { c.sale_item, c.prod_name, c.prod_num, c.prod_uom, c.price_unit, c.price_sub }

                       //明细表自定義的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                       //CustomFields = new List<CustomField>() {
                       //     new CustomField() { Name="明细自定義1",   Field="test1"  } ,
                       //     new CustomField() { Name="明细自定義2",  Field="test2" }
                     //  }
                   }
                 )
             .Use<sbm_stock_picking, sbm_stockmove>(
                   //主表配置
                   name: "sbm出貨單打印管理",
                   //主表可以打印的字段
                   printFields: x => new { x.name, x.picking_date, x.partner_company, x.partner_contact, x.picking_owner, x.bu_logo,x.origin},
                                       customFields: null,//配置同上
                                                          //主表自定義打印的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                                                          //customFields: new List<CustomField>() {
                                                          //     new CustomField() { Name="專業服務報價單",  Field="doc_title" } ,
                                                          //         new CustomField() { Name="條形碼",  Field="barcode" }
                                                          //         new CustomField() { Name="二維碼",  Field="qrcode" },
                                                          //         new CustomField() { Name="明细自定義1",   Field="test1"  } ,
                                                          //         new CustomField() { Name="明细自定義2",  Field="test2" }
                                                          // },
                                                          //明细表配置
                   detail: new PrintDetailOptions<sbm_stockmove>()
                   {
                       Name = "出貨單明細",
                       PrintFields = c => new { c.move_item, c.prod_name, c.prod_num, c.prod_uom, c.prod_desc}

                       //明细表自定義的字段,没有就填null;需要在:PrintCustom類QueryResult字自定義返回這些字段的值
                       //CustomFields = new List<CustomField>() {
                       //     new CustomField() { Name="明细自定義1",   Field="test1"  } ,
                       //     new CustomField() { Name="明细自定義2",  Field="test2" }
                       //  }
                   }
                 )


            ;


        }
    }
}

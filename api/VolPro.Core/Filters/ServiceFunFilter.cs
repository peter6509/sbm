using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VolPro.Core.WorkFlow;
using VolPro.Core.Enums;
using VolPro.Core.Utilities;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.Filters
{
    public abstract class ServiceFunFilter<T> where T : class
    {

        /// <summary>
        /// 2020.08.15是否開啟多租户功能
        /// 使用方法见文档或SellOrderService.cs
        /// </summary>
        protected bool IsMultiTenancy = true;

        /// <summary>
        /// 查詢界面table 统计、求和、平均值等
        /// 實现方式
        ///SummaryExpress = (IQueryable<App_TransactionAvgPrice> queryable) =>
        //                {
        //                return queryable.GroupBy(x => 1).Select(x => new
        //                {
        //                    AvgPrice = x.Average(o => o.AvgPrice),
        //                    Enable = x.Sum(o => o.Enable)
        //            }).FirstOrDefault();
        //};
        /// </summary>
        //   protected Func<IGrouping<T, T>, object> SummaryExpress = null;
        protected Func<IQueryable<T>, object> SummaryExpress = null;

        /// <summary>
        /// 明细table 统计、求和、平均值等
        /// </summary>
        /// <typeparam name="Detail"></typeparam>
        /// <param name="queryeable"></param>
        /// <returns></returns>
        protected abstract object GetDetailSummary<Detail>(IQueryable<Detail> queryeable);

        /// <summary>
        /// 是否開啟用户數據權限,true=用户只能操作自己(及下级角色)創建的數據,如:查詢、删除、修改等操作
        /// 注意：需要在代碼生成器界面选择【是】及生成Model才會生效)
        /// </summary>
        protected bool LimitCurrentUserPermission { get; set; } = false;

        ///默認导出最大表數量：0不限制 
        protected int Limit { get; set; } = 0;

        /// <summary>
        /// 默認上傳文件大小限制3M
        /// </summary>
        protected int LimitUpFileSizee { get; set; } = 3;


        /// <summary>
        /// 2020.08.15添加自定義原生查詢sql,這個對于不想写表达式关联或者复杂查詢非常有用
        /// 例：QuerySql=$"select * from tb1 as a where  a.name='xxxx' x.id in (select b.id from tb2 b)";
        ///  select * 這里可以自定義，但select 必须返回表所有的列，不能少
        /// </summary>
        protected string QuerySql = null;

        /// <summary>
        /// 查詢前,對现在有的查詢字符串条件增加或删除
        /// </summary>
        protected Action<List<SearchParameters>> QueryRelativeList { get; set; }

        //查詢前,在现有的查詢条件上通過表达式修改查詢条件
        protected Func<IQueryable<T>, IQueryable<T>> QueryRelativeExpression { get; set; }


        /// <summary>
        /// 指定查詢的列，格式:Expression<Func<T, object>> exp = x => new { x.字段1, x.字段2 }(暂時未啟用)
        /// </summary>
        protected Expression<Func<T, object>> Columns { get; set; }

        /// <summary>
        /// 設置查詢排序参數及方式,参數格式為：
        ///  Expression<Func<T, Dictionary<object, bool>>> orderBy = x => new Dictionary<object, QueryOrderBy>() 
        ///            {{ x.ID, QueryOrderBy.Asc },{ x.DestWarehouseName, QueryOrderBy.Desc } };
        /// 返回的是new Dictionary<object, bool>(){{}}key為排序字段，QueryOrderBy為排序方式
        /// </summary>
        protected Expression<Func<T, Dictionary<object, QueryOrderBy>>> OrderByExpression;

        /// <summary>
        /// 設置查詢的表名(已弃用)
        /// </summary>
        protected string TableName { get; set; }

        /// <summary>
        /// 页面查詢或导出，从數據庫查詢出来的结果
        /// </summary>
        protected Action<PageGridData<T>> GetPageDataOnExecuted;

        /// <summary>
        /// 調用新建處理前(SaveModel為傳入的原生數據)
        /// </summary>
        protected Func<SaveModel, WebResponseContent> AddOnExecute;

        /// <summary>
        /// 調用新建保存數據庫前處理(已將提交的原生數據转換成了對象)
        ///  Func<T, object,ResponseData>  T為主表數據，object為明细數據(如果需要使用明细對象,請用 object as List<T>转換) 
        /// </summary>
        protected Func<T, object, WebResponseContent> AddOnExecuting;

        /// <summary>
        /// 調用新建保存數據庫后處理。
        /// **實现當前方法時，内部默認已經開啟事務，如果實现的方法操作的是同一數據庫,则不需要在AddOnExecuted中事務
        ///  Func<T, object,ResponseData>  T為主表數據，object為明细數據(如果需要使用明细對象,請用 object as List<T>转換) 
        ///  此處已開啟了DbContext事務(重點),如果还有其他業務處事，直接在這里写EF代碼,不需要再開啟事務
        /// 如果執行的是手写sql請用repository.DbContext.Database.ExecuteSqlCommand()或 repository.DbContext.Set<T>().FromSql執行具體sql语句
        /// </summary>
        protected Func<T, object, WebResponseContent> AddOnExecuted;

        /// <summary>
        /// 進入審批流程方法之前
        /// </summary>
        protected Func<T, bool> AddWorkFlowExecuting;

        /// <summary>
        /// 写入審批流程數據之后
        /// list:審批的人id
        /// </summary>
        protected Action<T,List<int>> AddWorkFlowExecuted;

        /// <summary>
        /// 調用更新方法前處理(SaveModel為傳入的原生數據)
        /// </summary>
        protected Func<SaveModel, WebResponseContent> UpdateOnExecute;

        /// <summary>
        ///  調用更新方法保存數據庫前處理
        ///  (已將提交的原生數據转換成了對象,將明细新增、修改、删除的數據分别用object1/2/3標識出来 )
        ///  T=更新的主表對象
        ///  object1=為新增明细的對象，使用時將object as List<T>转換一下
        ///  object2=為更新明细的對象
        ///  List<object>=為删除的细的對象Key
        /// </summary>
        protected Func<T, object, object, List<object>, WebResponseContent> UpdateOnExecuting;

        /// <summary>
        ///  調用更新方法保存數據庫后處理
        ///   **實现當前方法時，内部默認已經開啟事務，如果實现的方法操作的是同一數據庫,则不需要在UpdateOnExecuted中事務
        ///  (已將提交的原生數據转換成了對象,將明细新增、修改、删除的數據分别用object1/2/3標識出来 )
        ///  T=更新的主表對象
        ///  object1=為新增明细的對象，使用時將object as List<T>转換一下
        ///  object2=為更新明细的對象
        ///  List<object>=為删除的细的對象Key
        /// 此處已開啟了DbContext事務(重點),如果还有其他業務處事，直接在這里写EF代碼,不需要再開啟事務
        /// 如果執行的是手写sql請用repository.DbContext.Database.ExecuteSqlCommand()或 repository.DbContext.Set<T>().FromSql執行具體sql语句
        /// </summary>
        protected Func<T, object, object, List<object>, WebResponseContent> UpdateOnExecuted;

        /// <summary>
        /// 删除前處理,object[]准備删除的主鍵
        /// </summary>
        protected Func<object[], WebResponseContent> DelOnExecuting;

        /// <summary>
        /// 删除后處理,object[]已删除的主鍵,此處已開啟了DbContext事務(重點),如果还有其他業務處事，直接在這里写EF代碼,不需要再開啟事務
        /// 如果執行的是手写sql請用repository.DbContext.Database.ExecuteSqlCommand()或 repository.DbContext.Set<T>().FromSql執行具體sql语句
        /// </summary>
        protected Func<object[], WebResponseContent> DelOnExecuted;

        /// <summary>
        /// 審核前處理
        /// </summary>
        protected Func<List<T>, WebResponseContent> AuditOnExecuting;
        /// <summary>
        /// 審核后處理
        /// </summary>
        protected Func<List<T>, WebResponseContent> AuditOnExecuted;


        /// <summary>
        /// 反核前處理
        /// </summary>
        protected Func<T, WebResponseContent> AntiAuditOnExecuting;
        /// <summary>
        /// 反核后處理
        /// </summary>
        protected Func<T, WebResponseContent> AntiAuditOnExecuted;

        /// <summary>
        /// 審批流程審核前
        /// T:當前審核的數據
        /// AuditStatus:審核状態
        /// bool:當前數據是否為最后一個人審核
        /// </summary>
        protected Func<T, AuditStatus, bool, WebResponseContent> AuditWorkFlowExecuting;

        /// <summary>
        /// 審批流程審核后
        /// T:當前審核的數據
        /// AuditStatus:審核状態
        /// list:下一個节點的審批人id
        /// bool:當前數據是否為最后一個人審核
        /// </summary>
        protected Func<T, AuditStatus,List<int>, bool, WebResponseContent> AuditWorkFlowExecuted;

        /// <summary>
        ///导出前處理,DataTable导出的表數據
        ///List<T>导出的數據, List<string>忽略不需要导出的字段
        ///此方法不建议使用,由下面ExportColumns委托替代2020.05.07
        /// </summary>
        protected Func<List<T>, List<string>, WebResponseContent> ExportOnExecuting;

        /// <summary>
        /// 2020.05.07
        /// 导出表數據(界面上导出操作),指定要导出的列，格式:Expression<Func<T, object>> exp = x => new { x.字段1, x.字段2 }
        /// </summary>
        protected Expression<Func<T, object>> ExportColumns { get; set; }


        /// <summary>
        /// 2020.05.07
        /// 导出下载模板，指定要导出的模板列，格式:Expression<Func<T, object>> exp = x => new { x.字段1, x.字段2 }
        /// </summary>
        protected Expression<Func<T, object>> DownLoadTemplateColumns { get; set; }
        /// <summary>
        /// 指定导入字段的名稱
        /// </summary>
        protected Expression<Func<T, Dictionary<object,string>>> ExcelHeaderMap { get; set; }
        /// <summary>
        /// 导入excel从第几行開始讀取
        /// </summary>
        protected int ImportStartRowIndex { get; set; } = 1;
        /// <summary>
        /// 导入時不驗証下拉框數據源的字段值
        /// </summary>
        protected Expression<Func<T, object>> ImportIgnoreSelectValidationColumns;
        /// <summary>
        /// 导入保存后
        /// </summary>
        protected Func<List<T>, WebResponseContent> ImportOnExecuted;

        /// <summary>
        /// 导入保存前
        /// </summary>
        protected Func<List<T>, WebResponseContent> ImportOnExecuting;

        /// <summary>
        /// 2022.06.20增加原生excel讀取方法(导入時可以自定義讀取excel内容)
        /// string=當前讀取的excel單元格的值
        /// ExcelWorksheet=excel對象
        /// ExcelRange當前excel單元格對象
        /// int=當前讀取的第几數
        /// int=當前讀取的第几列
        /// string=返回的值
        /// </summary>
        protected Func<string, ExcelWorksheet, ExcelRange, int, int, string> ImportOnReadCellValue;


        /// <summary>
        /// 自定義上傳文件夾(2022.10.07)
        /// </summary>
        protected string UploadFolder = null;
        protected bool IsRoot = true;
    }
}

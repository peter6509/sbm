
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VolPro.Core.CacheManager;
using VolPro.Core.Utilities;
using VolPro.Core.WorkFlow;
using VolPro.Entity.DomainModels;
using VolPro.Entity.SystemModels;

namespace VolPro.Core.BaseProvider
{
    public interface IService<T> where T : BaseEntity
    {

        ICacheService CacheContext { get; }
        Microsoft.AspNetCore.Http.HttpContext Context { get; }
        string WorkFlowTableName { get; set; }


        IQueryable<T> FindAsIQueryable(Expression<Func<T, bool>> predicate, bool filterDeleted = true);
        /// <summary>
        /// 將前端table的查詢条件转換為查詢ISugarQueryable
        /// </summary>
        /// <param name="options">前端查詢参數</param>
        /// <param name="useTenancy">是否使用數據隔離</param>
        /// <returns></returns>
        IQueryable<T> FindAsIQueryable(PageDataOptions options, bool useTenancy = true);
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        PageGridData<T> GetPageData(PageDataOptions pageData);

        object GetDetailPage(PageDataOptions pageData);

        WebResponseContent Upload(List<IFormFile> files);

        WebResponseContent DownLoadTemplate();

        WebResponseContent Import(List<IFormFile> files);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        WebResponseContent Export(PageDataOptions pageData);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDataModel">主表與子表的數據</param>
        /// <returns></returns>
        WebResponseContent Add(SaveModel saveDataModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">保存的實體</param>
        /// <param name="validationEntity">是否對實體進行校驗</param>
        /// <returns></returns>
        WebResponseContent AddEntity(T entity, bool validationEntity = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDetail"></typeparam>
        /// <param name="entity">保存的實體</param>
        /// <param name="list">保存的明细</param>
        /// <param name="validationEntity">是否對實體進行校驗</param>
        /// <returns></returns>
        WebResponseContent Add<TDetail>(T entity, List<TDetail> list = null, bool validationEntity = true) where TDetail : class;
        /// <summary>
        /// 编輯
        /// </summary>
        /// <param name="saveDataModel">主表與子表的數據</param>
        /// <returns></returns>
        WebResponseContent Update(SaveModel saveDataModel);


        /// <summary>
        /// 删除數據
        /// </summary>
        /// <param name="keys">删除的主鍵</param>
        /// <param name="delList">是否删除對應明细(默認會删除明细)</param>
        /// <returns></returns>
        WebResponseContent Del(object[] keys, bool delList = true);

        WebResponseContent Audit(object[] id, int? auditStatus, string auditReason);


        WebResponseContent AntiAudit(AntiData antiData);

        /// <summary>
        /// 重新生成流程或者回退流程
        /// </summary>
        /// <param name="id">表數據id</param>
        /// <param name="msg">重写流程的日志信息</param>
        /// <param name="flowWriteState">重新開始或回退到上一级节點</param>
        /// <returns></returns>
        WebResponseContent RestartWorkFlowAudit(object id, string msg, FlowWriteState flowWriteState);

        bool AddProcese(T entity);

        /// <summary>
        /// 提交審批數據 2023.11.12
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <param name="flowWriteState"></param>
        /// <returns></returns>
        WebResponseContent SubmitWorkFlowAudit(object[] ids);

        (string, T, bool) ApiValidate(string bizContent, Expression<Func<T, object>> expression = null);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">對指屬性驗証格式如：x=>new { x.UserName,x.Value }</param>
        /// <returns>(string,TInput, bool) string:返回驗証消息,TInput：bizContent序列化后的對象,bool:驗証是否通過</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="bizContent"></param>
        /// <param name="expression">對指屬性驗証格式如：x=>new { x.UserName,x.Value }</param>
        /// <param name="validateExpression">對指定的字段只做合法性判断比如长度是是否超长</param>
        /// <returns>(string,TInput, bool) string:返回驗証消息,TInput：bizContent序列化后的對象,bool:驗証是否通過</returns>
        (string, TInput, bool) ApiValidateInput<TInput>(string bizContent, Expression<Func<TInput, object>> expression, Expression<Func<TInput, object>> validateExpression);


        /// <summary>
        /// 將數據源映射到新的數據中,List<TSource>映射到List<TResult>或TSource映射到TResult
        /// 目前只支持Dictionary或實體類型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="resultExpression">只映射返回對象的指定字段</param>
        /// <param name="sourceExpression">只映射數據源對象的指定字段</param>
        /// 過濾条件表达式調用方式：List表达式x => new { x[0].MenuName, x[0].Menu_Id}，表示指定映射MenuName,Menu_Id字段
        ///  List<Sys_Menu> list = new List<Sys_Menu>();
        ///  list.MapToObject<List<Sys_Menu>, List<Sys_Menu>>(x => new { x[0].MenuName, x[0].Menu_Id}, null);
        ///  
        ///過濾条件表达式調用方式：實體表达式x => new { x.MenuName, x.Menu_Id}，表示指定映射MenuName,Menu_Id字段
        ///  Sys_Menu sysMenu = new Sys_Menu();
        ///  sysMenu.MapToObject<Sys_Menu, Sys_Menu>(x => new { x.MenuName, x.Menu_Id}, null);
        /// <returns></returns>
        TResult MapToEntity<TSource, TResult>(TSource source, Expression<Func<TResult, object>> resultExpression,
           Expression<Func<TSource, object>> sourceExpression = null) where TResult : class;

        /// <summary>
        /// 將一個實體的赋到另一個實體上,應用场景：
        /// 两個實體，a a1= new a();b b1= new b();  a1.P=b1.P; a1.Name=b1.Name;
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <param name="expression">指定對需要的字段赋值,格式x=>new {x.Name,x.P},返回的结果只會對Name與P赋值</param>
        void MapValueToEntity<TSource, TResult>(TSource source, TResult result, Expression<Func<TResult, object>> expression = null) where TResult : class;


    }
}

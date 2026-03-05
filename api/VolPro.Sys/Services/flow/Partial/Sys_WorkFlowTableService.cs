/*
 *所有关于Sys_WorkFlowTable類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*Sys_WorkFlowTableService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
using VolPro.Sys.IRepositories;
using VolPro.Core.ManageUser;
using VolPro.Core.WorkFlow;
using System;
using VolPro.Core.DBManager;
using System.Collections.Generic;

namespace VolPro.Sys.Services
{
    public partial class Sys_WorkFlowTableService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_WorkFlowTableRepository _repository;//訪問數據庫
        private readonly ISys_WorkFlowTableStepRepository _stepRepository;//訪問數據庫
        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowTableService(
            ISys_WorkFlowTableRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
             ISys_WorkFlowTableStepRepository stepRepository
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _stepRepository = stepRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }
        /// <summary>
        /// 待審核、審批中的數據
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        private IQueryable<Sys_WorkFlowTable> GetAuditQuery(IQueryable<Sys_WorkFlowTable> queryable, bool all = false, bool cc = false)
        {
            var user = UserContext.Current.UserInfo;
            var deptIds = user.DeptIds.Select(s => s.ToString());
            var stepQuery = _stepRepository.FindAsIQueryable(x => ((x.StepType == (int)AuditType.用户審批 && x.StepValue == user.User_Id.ToString())
              || (x.StepType == (int)AuditType.角色審批 && user.RoleIds.Select(s => s.ToString()).Contains(x.StepValue))
              || (x.StepType == (int)AuditType.部门審批 && deptIds.Contains(x.StepValue)))
               );
            //顯示當前用户的全部數據
            if (all)
            {
                queryable = queryable.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id && (x.CreateID == user.User_Id || x.CurrentStepId == c.StepId || c.AuditId == user.User_Id)));
                return queryable;
            }
            if (cc)
            {
                queryable = queryable.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id
                     &&c.StepAttrType == StepType.cc.ToString()
                      && repository.DbContext.Set<Sys_WorkFlowTableStep>().Any(a => c.ParentId == a.StepId && x.WorkFlowTable_Id == a.WorkFlowTable_Id && a.AuditStatus == 1)
                     ));
                return queryable;
            }
            queryable = queryable.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id
            && x.CurrentStepId == c.StepId && (c.AuditStatus == null || c.AuditStatus == 0))
                                     && (x.AuditStatus == (int)AuditStatus.待審核 || x.AuditStatus == (int)AuditStatus.審核中));
            return queryable;
        }
        public override PageGridData<Sys_WorkFlowTable> GetPageData(PageDataOptions options)
        {
            this.IsMultiTenancy = false;
            var user = UserContext.Current.UserInfo;

            //pc端
            //顯示當前用户需要審批的數據
            QueryRelativeExpression = (IQueryable<Sys_WorkFlowTable> queryable) =>
            {
                int value = options.Value.GetInt();
                switch (value)
                {
                    //我的提交
                    case 50:
                        queryable = queryable.Where(x => x.CreateID == UserContext.Current.UserId);
                        break;
                    //我的審核
                    case 40:
                        var stepQuery = _stepRepository.FindAsIQueryable(c => c.AuditId == user.User_Id);
                        queryable = queryable.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id));
                        break;
                    case (int)AuditStatus.待審核:
                    case (int)AuditStatus.審核中:
                        queryable = GetAuditQuery(queryable);
                        break;
                    //抄送我的
                    case -2:
                        queryable = GetAuditQuery(queryable, cc: true);
                        break;
                    case (int)AuditStatus.審核通過:
                    case (int)AuditStatus.審核未通過:
                    case (int)AuditStatus.驳回:
                        var _stepQuery = _stepRepository.FindAsIQueryable(x => x.AuditId == user.User_Id);
                        queryable = queryable.Where(x => _stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id));
                        if (value == (int)AuditStatus.審核通過)
                        {
                            queryable = queryable.Where(x => x.AuditStatus == (int)AuditStatus.審核通過);
                        }
                        else if (value == (int)AuditStatus.審核未通過)
                        {
                            queryable = queryable.Where(x => x.AuditStatus == (int)AuditStatus.審核未通過);
                        }
                        else
                        {
                            queryable = queryable.Where(x => x.AuditStatus == (int)AuditStatus.驳回);
                        }
                        break;
                    default:
                        break;
                }
                queryable = queryable.Where(x => (x.AuditStatus != (int)AuditStatus.草稿 && x.AuditStatus != (int)AuditStatus.待提交));
                if (value == -1 && !UserContext.Current.IsSuperAdmin)
                {
                    queryable = GetAuditQuery(queryable, true);
                }
                return queryable;
            };

            return base.GetPageData(options);
        }
    }
}

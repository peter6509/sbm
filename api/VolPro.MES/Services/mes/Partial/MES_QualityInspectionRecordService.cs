/*
 *所有关于MES_QualityInspectionRecord類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*MES_QualityInspectionRecordService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
    public partial class MES_QualityInspectionRecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMES_QualityInspectionRecordRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public MES_QualityInspectionRecordService(
            IMES_QualityInspectionRecordRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }
  }
}

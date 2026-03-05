/*
 *所有关于Sys_ReportOptions類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*Sys_ReportOptionsService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
using System.Collections.Generic;

namespace VolPro.Sys.Services
{
    public partial class Sys_ReportOptionsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_ReportOptionsRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Sys_ReportOptionsService(
            ISys_ReportOptionsRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户會用到這init代碼，其他情况可以不用
            //base.Init(dbRepository);
        }

        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            saveDataModel.MainData["ReportCode"] = new IdWorker().NextId().ToString();
            return base.Add(saveDataModel);
        }

        WebResponseContent webResponse = new WebResponseContent();
        public override WebResponseContent Upload(List<IFormFile> files)
        {
            if (!files.Any(x => x.FileName.ToLower().EndsWith(".grf")))
            {
                return webResponse.Error("只能上傳grf格式文件");
            }
            IsRoot = false;
            UploadFolder = "ReportTemplate/";
            return base.Upload(files);
        }
    }
}

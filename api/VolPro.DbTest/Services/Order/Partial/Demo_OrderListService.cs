/*
 *所有关于Demo_OrderList類的業務代碼應在此處编写
*可使用repository.調用常用方法，获取EF/Dapper等信息
*如果需要事務請使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手動获取數據庫相关信息
*用户信息、權限、角色等使用UserContext.Current操作
*Demo_OrderListService對增、删、改查、导入、导出、審核業務代碼扩展参照ServiceFunFilter
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
    public partial class Demo_OrderListService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDemo_OrderListRepository _repository;//訪問數據庫

        [ActivatorUtilitiesConstructor]
        public Demo_OrderListService(
            IDemo_OrderListRepository dbRepository,
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

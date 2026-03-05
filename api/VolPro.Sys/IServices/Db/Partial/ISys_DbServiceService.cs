/*
*所有关于Sys_DbService類的業務代碼接口應在此處编写
*/
using VolPro.Core.BaseProvider;
using VolPro.Entity.DomainModels;
using VolPro.Core.Utilities;
using System.Linq.Expressions;
using System;

namespace VolPro.Sys.IServices
{
    public partial interface ISys_DbServiceService
    {
        WebResponseContent CreateDb(Guid id);
    }
 }

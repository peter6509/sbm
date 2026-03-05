/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下MES_ProcessReportService與IMES_ProcessReportService中编写
 */
using VolPro.MES.IRepositories;
using VolPro.MES.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.MES.Services
{
    public partial class MES_ProcessReportService : ServiceBase<MES_ProcessReport, IMES_ProcessReportRepository>
    , IMES_ProcessReportService, IDependency
    {
    public static IMES_ProcessReportService Instance
    {
      get { return AutofacContainerModule.GetService<IMES_ProcessReportService>(); } }
    }
 }

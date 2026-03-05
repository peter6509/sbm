/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,此處任何更改都可能导致被代碼生成器覆盖
 *所有業務编写全部應在Partial文件夾下sbm_require_purchase_docService與Isbm_require_purchase_docService中编写
 */
using VolPro.sbm.IRepositories;
using VolPro.sbm.IServices;
using VolPro.Core.BaseProvider;
using VolPro.Core.Extensions.AutofacManager;
using VolPro.Entity.DomainModels;

namespace VolPro.sbm.Services
{
    public partial class sbm_require_purchase_docService : ServiceBase<sbm_require_purchase_doc, Isbm_require_purchase_docRepository>
    , Isbm_require_purchase_docService, IDependency
    {
    public static Isbm_require_purchase_docService Instance
    {
      get { return AutofacContainerModule.GetService<Isbm_require_purchase_docService>(); } }
    }
 }

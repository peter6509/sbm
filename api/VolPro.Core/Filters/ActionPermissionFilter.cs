using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core.Configuration;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Core.Services;
using VolPro.Core.Utilities;
using VolPro.Entity.AttributeManager;

namespace VolPro.Core.Filters
{
    /// <summary>
    /// 1、控制器或controller設置了AllowAnonymousAttribute直接返回
    /// 2、TableName、TableAction 同時為null，SysController為false的，只判断是否登陆
    /// 3、TableName、TableAction 都不null根據表名與action判断是否有權限
    /// 4、SysController為true，通過httpcontext获取表名與action判断是否有權限
    /// 5、Roles對指定角色驗証
    /// </summary>
    public class ActionPermissionFilter : IAsyncActionFilter
    {
        private WebResponseContent ResponseContent { get; set; }
        private ActionPermissionRequirement ActionPermission;
        private UserContext _userContext { get; set; }
        // private ResponseType responseType;
        public ActionPermissionFilter(ActionPermissionRequirement actionPermissionRequirement, UserContext userContext)
        {
            this.ResponseContent = new WebResponseContent();
            this.ActionPermission = actionPermissionRequirement;
            _userContext = userContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (OnActionExecutionPermission(context).Status)
            {
                await next();
                return;
            }
            FilterResponse.SetActionResult(context, ResponseContent);
        }
        private WebResponseContent OnActionExecutionPermission(ActionExecutingContext context)
        {
            //!context.Filters.Any(item => item is IFixedTokenFilter))固定token的是否驗証權限
            //if ((context.Filters.Any(item => item is IAllowAnonymousFilter)
            //    && !context.Filters.Any(item => item is IFixedTokenFilter))
            //    || UserContext.Current.IsSuperAdmin
            //    )
            if (context.Filters.Any(item => item is IAllowAnonymousFilter)
                || UserContext.Current.IsSuperAdmin)
                return ResponseContent.OK();

            //演示環境除了admin帳號，其他帳號都不能增删改等操作
            if (!_userContext.IsSuperAdmin && AppSetting.GlobalFilter.Enable
                && AppSetting.GlobalFilter.Actions.Contains(((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName))
            { 
                return ResponseContent.Error(AppSetting.GlobalFilter.Message);
            }

            //如果没有指定表的權限，则默認為代碼生成的控制器，优先获取PermissionTableAttribute指定的表，如果没有數據则使用當前控制器的名作為表名權限
            if (ActionPermission.SysController)
            {
                object[] permissionArray = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor)?.ControllerTypeInfo.GetCustomAttributes(typeof(PermissionTableAttribute), false);
                if (permissionArray == null || permissionArray.Length == 0)
                {
                    ActionPermission.TableName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                }
                else
                {
                    ActionPermission.TableName = (permissionArray[0] as PermissionTableAttribute).Name;
                }
                if (string.IsNullOrEmpty(ActionPermission.TableName))
                {
                    //responseType = ResponseType.ParametersLack;
                    return ResponseContent.Error(ResponseType.ParametersLack);
                }
            }

            //如果没有给定權限，不需要判断
            if (string.IsNullOrEmpty(ActionPermission.TableName)
                && string.IsNullOrEmpty(ActionPermission.TableAction)
                && (ActionPermission.RoleIds == null || ActionPermission.RoleIds.Length == 0))
            {
                return ResponseContent.OK();
            }

            //是否限制的角色ID稱才能訪問
            //權限判断角色ID,
            if (ActionPermission.RoleIds != null && ActionPermission.RoleIds.Length > 0)
            {
                if (ActionPermission.RoleIds.Contains(_userContext.UserInfo.Role_Id)) return ResponseContent.OK();
                //如果角色ID没有權限。並且也没控制器權限
                if (string.IsNullOrEmpty(ActionPermission.TableAction))
                {
                    return ResponseContent.Error(ResponseType.NoRolePermissions);
                }
            }
            //2020.05.05移除x.TableName.ToLower()转換,获取權限時已經转換成為小写
            var actionAuth = _userContext.GetPermissions(x => x.TableName == ActionPermission.TableName.ToLower())
                ?.UserAuthArr?.Contains(ActionPermission.TableAction) ?? false;

            if (!actionAuth)
            {
                //2023.06.30增加移動端權限二次判断
                if (UserContext.MenuType == 1)
                {
                    actionAuth = _userContext.Permissions.Where(x => x.TableName == ActionPermission.TableName.ToLower())
                        .Any(c => c.UserAuthArr.Contains(ActionPermission.TableAction));
                }
                if (!actionAuth)
                {
                    Logger.Info(LoggerType.Authorzie, $"没有權限操作," +
                   $"用户ID{_userContext.UserId}:{_userContext.UserTrueName}," +
                   $"操作權限{ActionPermission.TableName}:{ActionPermission.TableAction}");
                    return ResponseContent.Error(ResponseType.NoPermissions);
                }
            }
            return ResponseContent.OK();
        }
    }
}

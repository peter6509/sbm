using VolPro.Core.Enums;

namespace VolPro.Core.Extensions
{
    public static class ResponseMsg
    {
        public static string GetMsg(this ResponseType  responseType)
        {
            string msg;
            switch (responseType)
            {
                case ResponseType.LoginExpiration:
                    msg = "登陆已過期,請重新登陆"; break;
                case ResponseType.TokenExpiration:
                    msg = "Token已過期,請重新登陆"; break;
                case ResponseType.AccountLocked:
                    msg = "帳號已被锁定"; break;
                case ResponseType.LoginSuccess:
                    msg = "登陆成功"; break;
                case ResponseType.ParametersLack:
                    msg = "参數不完整"; break;
                case ResponseType.NoPermissions:
                    msg = "没有權限操作"; break;
                case ResponseType.NoRolePermissions:
                    msg = "角色没有權限操作"; break;
                case ResponseType.ServerError:
                    msg = "服務器好像出了點問题....."; break;
                case ResponseType.LoginError:
                    msg = "用户名或密碼错误"; break;
                case ResponseType.SaveSuccess:
                    msg = "保存成功"; break;
                case ResponseType.NoKey:
                    msg = "没有主鍵不能编輯"; break;
                case ResponseType.NoKeyDel:
                    msg = "没有主鍵不能删除"; break;
                case ResponseType.KeyError:
                    msg = "主鍵不正确或没有傳入主鍵"; break;
                case ResponseType.EidtSuccess:
                    msg = "编輯成功"; break;
                case ResponseType.DelSuccess:
                    msg = "删除成功"; break;
                case ResponseType.RegisterSuccess:
                    msg = "注册成功"; break;
                case ResponseType.AuditSuccess:
                    msg = "審核成功"; break;
                case ResponseType.ModifyPwdSuccess:
                    msg = "密碼修改成功"; break;
                case ResponseType.OperSuccess:
                    msg = "操作成功"; break;
                case ResponseType.PINError:
                    msg = "驗証碼不正确"; break;
                    
                default: msg = responseType.ToString(); break;
            }
            return msg;
        }

    }
}

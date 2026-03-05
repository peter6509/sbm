using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VolPro.Core.Extensions;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.ObjectActionValidator
{
    /// <summary>
    /// 對方法指定屬性校驗,此處配置完成就不用每處都写if esle判断值是合法
    /// 與自带模型校驗相比，此處可以通過表达式校驗指定字段，也不用担心model字段变化后还去手動修改配置的問题
    /// 目前只支持普通屬性，不支持复杂類型
    /// </summary>
    public static class ValidatorContainer
    {
        /// <summary>
        /// model校驗配置
        /// 方法参數名必须與枚舉名稱一致（不区分大小写）,如：public void Test(LoginInfo login)
        /// 表达式是model必须要驗証的字段，如果不填，默認驗証整個model
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMethodsModelParameters(this IServiceCollection services)
        {
            //登陆方法校驗参數,只驗証密碼與用户名
            ValidatorModel.Login.Add<LoginInfo>(x => new { x.Password, x.UserName,x.VerificationCode,x.UUID });

            //只驗証LoginInfo的密碼字段必填
            ValidatorModel.LoginOnlyPassWord.Add<LoginInfo>(x => new { x.Password });

            return services;
        }
        /// <summary>
        ///  普通屬性校驗
        /// 方法上添加[ObjectGeneralValidatorFilter(ValidatorGeneral.xxx)]即可進行参數自動驗証
        /// ValidatorGeneral為枚舉(也是方法的参數名)，自己需要校驗的参數在枚舉上添加
        /// ValidatorGeneral.xxx.Add() 配置自己的驗証規則
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMethodsGeneralParameters(this IServiceCollection services)
        {
            //配置用户名最多30個字符
            ValidatorGeneral.UserName.Add("用户名", 30);

            //方法参數名為newPwd，直接在方法加上[ObjectGeneralValidatorFilter(ValidatorGeneral.NewPwd)]進行参數驗証
            //如果newPwd為空會提示：新密碼不能為空
            //6,50代表newPwd参數最少6個字符，最多50個符
            //其他需要驗証的参數同样配置即可
            ValidatorGeneral.NewPwd.Add("新密碼", 6, 50);

            //如果OldPwd為空會提示：旧密碼不能為空
            ValidatorGeneral.OldPwd.Add("旧密碼");

            //校驗手機號碼格式
            ValidatorGeneral.PhoneNo.Add("手機號碼", (object value) =>
            {
                ObjectValidatorResult validatorResult = new ObjectValidatorResult(true);
                if (!value.ToString().IsPhoneNo())
                {
                    validatorResult = validatorResult.Error("請输入正确的手機號碼");
                }
                return validatorResult;
            });

            //測試驗証字符长度為6-10
            ValidatorGeneral.Local.Add("所在地",6,10);

            //測試驗証數字范围
            ValidatorGeneral.Qty.Add("存货量",ParamType.Int, 200, 500);

            return services;
        }
    }
    //方法参數是實體配置驗証字段
    public enum ValidatorModel
    {
        Login,
        LoginOnlyPassWord//只驗証密碼
    }
    /// <summary>
    /// 方法普通参數名配置(枚舉的名字必须與参數名字一样，不区分大小写)
    /// 通過在方法加上[ObjectGeneralValidatorFilter(ValidatorGeneral.UserName, ValidatorGeneral.PassWord)]指定要驗証的参數
    /// </summary>
    public enum ValidatorGeneral
    {
        UserName,
        OldPwd,
        NewPwd,
        PhoneNo,
        Local,//測試驗証字符长度
        Qty//測試 驗証值大小
    }
}

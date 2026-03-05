using System;
using System.Collections.Generic;
using System.Text;

namespace VolPro.Core.ObjectActionValidator
{
    /// <summary>
    /// 普通参數配置
    /// </summary>
    public class GeneralOptions
    {

        /// <summary>
        /// 自定義驗証
        /// </summary>
        /// <param name="generalName"></param>
        /// <param name="customValidator"></param>
        public GeneralOptions(ValidatorGeneral generalName, string CNName, Func<object, ObjectValidatorResult> customValidator)
        {
            this.CNName = CNName;
            this.CustomValidator = customValidator;
            this.Name = generalName.ToString().ToLower();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalName">普通参數的名字，與方法参數名字必须一致（不分大小写）,可以直接在ValidatorGeneral上添加</param>
        public GeneralOptions(ValidatorGeneral generalName, string CNName)
        {
            this.Name = generalName.ToString().ToLower();
            this.CNName = CNName;
            this.ParamType = ParamType.String;
        }
        public GeneralOptions(ValidatorGeneral generalName, string CNName, ParamType type)
        {
            this.Name = generalName.ToString().ToLower();
            this.CNName = CNName;
            this.ParamType = ParamType.String;
        }
        public GeneralOptions(ValidatorGeneral generalName, string CNName, int? min, int? max)
        {
            this.Name = generalName.ToString().ToLower();
            this.CNName = CNName;
            this.ParamType = ParamType.String;
            this.Min = min;
            this.Max = max;
        }

        public GeneralOptions(ValidatorGeneral generalName, string CNName, ParamType type, int? min, int? max)
        {
            this.Name = generalName.ToString().ToLower();
            this.CNName = CNName;
            this.ParamType = type;
            this.Min = min;
            this.Max = max;
        }

        public Func<object, ObjectValidatorResult> CustomValidator;
        /// <summary>
        /// 方法上的参數名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 中文名字,参數校驗错误的提示文字
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 参數類型，目前只列出了這几种,不够自己再加
        /// </summary>
        public ParamType ParamType { get; set; }
        /// <summary>
        /// 數字為最小值,字符串為最小长度
        /// </summary>
        public int? Min { get; set; }
        /// <summary>
        /// 數字為最大值,字符串為最大长度
        /// </summary>
        public int? Max { get; set; }

    }
    public enum ParamType
    {
        Int,
        //Long,
        //Byte,
        Bool,
        String,
        DateTime,
        Decimal,
        Guid
    }
}

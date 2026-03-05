using System;
using System.Collections.Generic;
using System.Text;

namespace VolPro.Core.Enums
{
    public enum ActionPermissionOptions
    {
        //注意添加的枚舉值一定要是前面的值倍數，即x2
        Add = 0,
        Update = 2,
        Search = 4,
        Export = 8,
        Delete = 12,
        Audit = 24,
        Upload = 48,//上傳文件
        Import = 96 //导入表數據Excel
    }
}

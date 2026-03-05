using System;
using System.Collections.Generic;
using System.Text;

namespace VolPro.Core.WorkFlow
{
    public enum AuditBack
    {
        流程结束 = 0,
        返回上一节點 = 1,
        流程重新開始 = 2
    }
}

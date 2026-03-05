using System;
using System.Collections.Generic;
using System.Text;

namespace VolPro.Core.WorkFlow
{
    public enum AuditStatus
    {
        待審核 = 0,
        審核通過 = 1,
        審核中 = 2,
        審核未通過 = 3,
        驳回 = 4,
        终止 = 5,
        反審 = 9,
        //预留審批流程草稿、待提交功能
        草稿 = 90,
        待提交 = 100
    }

    public enum AuditType
    {
        用户審批 = 1,
        角色審批 = 2,
        部门審批 = 3,
        提交人上级部门審批 = 4,
        提交人上级角色審批 = 5,
        提交人自己 = 6,
        //根據提交人的部门，找到部门下所有用户的角色
        提交人部门對應角色 = 7
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model.Enum
{
    public enum EnumContributionStatus
    {
        New = 0,// 新稿件
        Retreat = -3,// 退修
        Proof = 100,// 已发校样
        Employment = 200,// 录用稿件
        Manuscript = -100,// 退稿

        AuditedEn = 886, //英文专家已审
        ReAuditedEn = 887, //英文专家已复审

        Audited = 888, //专家已审
        ReAudited = 889, //专家已复审
        AuditedPart =-888, // 部分专家已审
        Reject = 999, //专家拒审
        Delete = -999 // 删除

    }
}

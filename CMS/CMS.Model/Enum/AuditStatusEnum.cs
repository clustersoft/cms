using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model.Enum
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStatusEnum
    {
        暂存=-1,

        未审核=0,

        审核通过=1,

        审核中=2,

        被退回=3,

        已删除=4
    }
}

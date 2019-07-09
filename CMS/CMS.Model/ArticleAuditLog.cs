using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 文章审核日志表
    /// </summary>
    [Table("ArticleAuditLogs")]
    public class ArticleAuditLog
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        /// <summary>
        /// 审核状态(-1:草稿; 0:未审核; 1:审核通过; 2:审核中; 3:被退回;4:删除的)
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 审核结果原因
        /// </summary>
        public string AuditReason { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public int AuditUser { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 审核人IP地址
        /// </summary>
        public string AuditIP { get; set; }

        public virtual Article Article { get; set; }
    }
}

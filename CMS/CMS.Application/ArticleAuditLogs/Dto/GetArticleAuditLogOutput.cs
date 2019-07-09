using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleAuditLogs.Dto
{
    public class GetArticleAuditLogOutput
    {
        public string AuditStatus { get; set; }

        public string AuditReason { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditTime { get; set; }
    }
}

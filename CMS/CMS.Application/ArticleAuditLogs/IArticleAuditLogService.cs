using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAuditLogs.Dto;
using CMS.Model;

namespace CMS.Application.ArticleAuditLogs
{
    public interface IArticleAuditLogService:IBaseService<ArticleAuditLog>
    {
        List<GetArticleAuditLogOutput> GetArticleAuditLogs(int id);
    }
}

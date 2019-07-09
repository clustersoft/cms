using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAuditLogs.Dto;
using CMS.Model;
using CMS.Model.Enum;

namespace CMS.Application.ArticleAuditLogs
{
    public class ArticleAuditLogService:BaseService<ArticleAuditLog>,IArticleAuditLogService
    {
        public ArticleAuditLogService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }

        public List<GetArticleAuditLogOutput> GetArticleAuditLogs(int id)
        {
            var list = (from a in db.ArticleAuditLogs
                join b in db.Users on a.AuditUser equals b.ID
                where a.ArticleID == id
                select new GetArticleAuditLogOutput
                {
                    AuditTime = a.AuditTime,
                    AuditReason = a.AuditReason,
                    AuditStatus = ((AuditStatusEnum) a.AuditStatus).ToString(),
                    AuditUser = b.UserName ?? ""
                }).AsNoTracking().ToList();
            return list;
        }
    }
}

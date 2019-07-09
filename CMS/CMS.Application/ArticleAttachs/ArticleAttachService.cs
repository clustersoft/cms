using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.ArticleAttachs
{
    public class ArticleAttachService:BaseService<ArticleAttach>,IArticleAttachService
    {
        public ArticleAttachService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}

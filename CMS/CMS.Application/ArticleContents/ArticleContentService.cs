using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.ArticleContents.Dto
{
    public class ArticleContentService:BaseService<ArticleContent>,IArticleContentService
    {
        public ArticleContentService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}

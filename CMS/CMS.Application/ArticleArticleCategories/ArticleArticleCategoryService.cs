using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.ArticleArticleCategories
{
    public class ArticleArticleCategoryService:BaseService<ArticleArticleCategory>,IArticleArticleCategoryService
    {
        public ArticleArticleCategoryService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}

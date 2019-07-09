using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetCategoryActionOutput
    {
        public List<GetCategoryActionData> List;

        public int IsAdmin { get; set; }
    }

    public class GetCategoryActionData
    {
        public int ArticleCategoryID { get; set; }

        public string ActionCode { get; set; }
    }
}

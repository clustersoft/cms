using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticlePageListOutput
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string Status { get; set; }

        public string CreateUserName { get; set; }

        public DateTime PubTime { get; set; }

        public int IsStickTop { get; set; }

        public string CoverImage { get; set; }

        public string[] SelfCategoryNames { get; set; }

        public int[] SelfCategoryIDs { get; set; }

        public string[] ParentCategoryNames { get; set; }

        public int[] ParentCategoryIDs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetArticeCategoryTreeListOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string Name { get; set; }

        public string RefNo { get; set; }

        public int OrderID { get; set; }

        public int AddArticlePermissions { get; set; }
    }
}

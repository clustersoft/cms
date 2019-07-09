using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetCategoryListOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string Name { get; set; }

        public string RefNo { get; set; }

        public int OrderID { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string ConvertImage { get; set; }

        public int AddArticlePermissions { get; set; }
    }
}

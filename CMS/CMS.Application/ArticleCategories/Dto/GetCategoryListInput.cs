using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetCategoryListInput
    {
        public int PageIndex { get; set; }

        public int ParentID { get; set; }

        public string Keywords { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetLoadCategoryListOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string CateName { get; set; }

        public int Layer { get; set; }
    }
}

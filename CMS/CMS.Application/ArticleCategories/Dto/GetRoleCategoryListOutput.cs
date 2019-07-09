using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetRoleCategoryListOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string CateName { get; set; }

        public string SelActions { get; set; }

        public int Layer { get; set; }
    }
}

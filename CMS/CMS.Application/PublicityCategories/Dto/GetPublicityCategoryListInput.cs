using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityCategories.Dto
{
    public class GetPublicityCategoryListInput
    {
        public int PageIndex { get; set; }

        public string Keyword { get; set; }

        public int PublicityTypesID { get; set; }
    }
}

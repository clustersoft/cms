using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityCategories.Dto
{
    public class GetPublicityCategoryListOutput
    {
        public int ID { get; set; }

        public int PublicityTypesID { get; set; }

        public string PublicityTypesName { get; set; }

        public string PublicityCategoryName { get; set; }

        public string Remark { get; set; }

        public int OrderID { get; set; }
    }
}

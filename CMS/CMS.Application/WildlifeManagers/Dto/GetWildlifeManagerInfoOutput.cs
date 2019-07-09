using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeManagers.Dto
{
    public class GetWildlifeManagerInfoOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int JQ_WildlifeCategoryID { get; set; }

        public string WildlifeCategoryName { get; set; }

        public string Introduce { get; set; }

        public int OrderID { get; set; }

        public string AttachUrl { get; set; }
    }
}

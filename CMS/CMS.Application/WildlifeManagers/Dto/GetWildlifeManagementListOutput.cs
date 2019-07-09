using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeManagers.Dto
{
    public class GetWildlifeManagementListOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string WildlifeCategoryID { get; set; }

        public int OrderID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Navgations.Dto
{
    public class GetNavgationListOutput
    {
        public int ID { get; set; }

        public int ParentId { get; set; }

        public string NavName { get; set; }

        public string NavTitle { get; set; }

        public string IconUrl { get; set; }

        public string LinkUrl { get; set; }

        public string ActionTypesName { get; set; }

        public int OrderID { get; set; }

        public int Layer { get; set; }

    }
}

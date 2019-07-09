using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Navgations.Dto
{
    public class GetNavgationOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string ParentName { get; set; }

        public string NavName { get; set; }

        public string NavTitle { get; set; }

        public string IconUrl { get; set; }

        public string LinkUrl { get; set; }

        public int OrderID { get; set; }

        public string ActionTypes { get; set; }
    }
}

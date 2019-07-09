using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Navgations.Dto
{
    public class GetLoadNavgationListOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string NavName { get; set; }

        public string NavTitle { get; set; }

        public string Actions { get; set; }

        public string ActionsName { get; set; }

        public int Layer { get; set; }
    }
}

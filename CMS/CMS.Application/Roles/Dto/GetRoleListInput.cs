using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Roles.Dto
{
    public class GetRoleListInput
    {
        public int PageIndex { get; set; }

        public string Keywords { get; set; }
    }
}

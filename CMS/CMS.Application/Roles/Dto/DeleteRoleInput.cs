using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Roles.Dto
{
    public class DeleteRoleInput
    {
        public int userID { get; set; }

        public string ids { get; set; }
    }
}

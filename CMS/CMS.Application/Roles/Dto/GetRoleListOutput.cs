using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Roles.Dto
{
    public class GetRoleListOutput
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public string Remark { get; set; }

        public int? OrderID { get; set; }
    }
}

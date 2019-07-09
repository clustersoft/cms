using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class GetUserOutput
    {
        public int ID { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string RoleNames { get; set; }

        public int OrderID { get; set; }
        
        public int DeptID { get; set; }

        public int Status { get; set; }

        public string AgreeIP { get; set; }

        public string UserSourceFrom { get; set; }

        public int Grade { get ; set; }

        public int Type { get; set; }
    }
}

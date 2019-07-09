using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class LoginOutput
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string UserSourceFrom { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string LastLoginIp { get; set; }

        public int Grade { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class ChangePwdInput
    {
        public int ID { get; set; }

        public string oldPwd { get; set; }

        public string newPwd { get; set; }
    }
}

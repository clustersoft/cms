using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class ChangePersonalInfoInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserSourceFrom { get; set; }

        public int ModifyUser { get; set; }
    }
}

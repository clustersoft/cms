using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Roles.Dto
{
    public class UpdateRoleInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string Remark { get; set; }

        public string Nav_SelActions { get; set; }

        public string Cate_SelActions { get; set; }

        [Required]
        public int ModifyUser { get; set; }

        [Required]
        public int OrderID { get; set; }
    }
}

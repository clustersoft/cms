using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeManagers.Dto
{
    public class DeleteWildlifeManagementInput
    {
        [Required]
        public string IDs { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}

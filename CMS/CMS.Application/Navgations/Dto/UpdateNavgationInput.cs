using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Navgations.Dto
{
    public class UpdateNavgationInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ParentID { get; set; }

        [Required]
        public string NavName { get; set; }

        [Required]
        public string NavTitle { get; set; }

        public string IconUrl { get; set; }

        public string LinkUrl { get; set; }

        [Required]
        public int OrderID { get; set; }

        public string ActionTypes { get; set; }

        [Required]
        public int ModifyUser { get; set; }
    }
}

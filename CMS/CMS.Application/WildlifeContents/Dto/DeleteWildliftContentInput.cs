using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeContents.Dto
{
    public class DeleteWildliftContentInput
    {
        [Required]
        public string IDs { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}

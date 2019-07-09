using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class ChangeStickTopInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int IsStickTop { get; set; }

        public int ModifyUser { get; set; }
    }
}

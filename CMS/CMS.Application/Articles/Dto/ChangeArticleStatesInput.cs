using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class ChangeArticleStatesInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int Status { get; set; }

        public int ModifyUser { get; set; }
    }
}

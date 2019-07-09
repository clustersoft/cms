using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleTopListInput
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int Top { get; set; }
    }
}

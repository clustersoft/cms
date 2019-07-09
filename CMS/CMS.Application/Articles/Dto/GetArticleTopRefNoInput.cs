using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleTopRefNoInput
    {
        [Required]
        public string RefNo { get; set; }

        [Required]
        public int Top { get; set; }
    }
}

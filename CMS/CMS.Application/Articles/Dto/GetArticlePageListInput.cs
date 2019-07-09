using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticlePageListInput
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int Pageindex { get; set; }

        [Required]
        public int PageSize{ get; set; }

        public string Keyword { get; set; }
    }
}

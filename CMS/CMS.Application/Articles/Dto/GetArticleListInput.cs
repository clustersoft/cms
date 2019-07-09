using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleListInput
    {
        [Required]
        public int ArticleCategorysID { get; set; }

        [Required]
        public int PageIndex { get; set; }

        public string Keywords { get; set; }

        public int? Status { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime? AddTimeStart { get; set; }

        public DateTime? AddTimeEnd { get; set; }
    }
}

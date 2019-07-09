using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class DeleteArticleCategoryInput
    {
        [Required]
        public int userID { get; set; }

        [Required]
        public string ids { get; set; }
    }
}

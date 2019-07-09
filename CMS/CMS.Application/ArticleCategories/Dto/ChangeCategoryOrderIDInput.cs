using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class ChangeCategoryOrderIDInput
    {
        [Required]
        public int OldID { get; set; }

        [Required]
        public int OldOrderID { get; set; }

        [Required]
        public int NewID { get; set; }

        [Required]
        public int NewOrderID { get; set; }
    }
}

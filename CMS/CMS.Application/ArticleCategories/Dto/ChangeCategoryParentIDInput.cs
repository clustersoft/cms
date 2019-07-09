using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleCategories.Dto
{
    public class ChangeCategoryParentIDInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ParentID { get; set; }
    }
}

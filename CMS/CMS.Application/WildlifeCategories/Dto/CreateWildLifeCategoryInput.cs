using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeCategories.Dto
{
    public class CreateWildLifeCategoryInput
    {
        [Required]
        public string CateName { get; set; }

        [Required]
        public int Type { get; set; }

        public int OrderID { get; set; }

        [Required]
        public int CreatUser { get; set; }
    }
}

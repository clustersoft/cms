using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeCategories.Dto
{
    public class GetWildLifeCategoryListInput
    {
        [Required]
        public int Pageindex { get; set; }

        public string Keywords { get; set; }
    }
}

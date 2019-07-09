using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityCategories.Dto
{
    public class UpdatePublicityCategoyrInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int PublicityTypesID { get; set; }

        [Required]
        public string PublicityCategoryName { get; set; }

        public string Remark { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ModifyUser { get; set; }
    }
}

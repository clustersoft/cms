using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.WildlifeManagers.Dto
{
    public class CreateWildlifeManagementInput
    {
        [Required]
        public int Type { get; set; }

        public string Name { get; set; }

        public int? WildlifemanagerID { get; set; }

        public string Introduce { get; set; }

        public int OrderID { get; set; }

        [Required]
        public int CreatUser { get; set; }

        public CreateObjAttachInput Attach { get; set; }

    }
}

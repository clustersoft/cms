using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ViewSpots.Dto
{
    public class CreateViewSpotInput
    {
        [Required]
        public string Name { get; set; }

        public string Jd { get; set; }

        public string Wd { get; set; }

        public double? Radius { get; set; }

        public string Introduce { get; set; }

        public string Charge { get; set; }

        public string OpeningTime { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int CreatUser { get; set; }

        public CreateObjAttachInput Attach { get; set; }
    }
}

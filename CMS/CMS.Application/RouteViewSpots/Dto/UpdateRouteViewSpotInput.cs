using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.RouteViewSpots.Dto
{
    public class UpdateRouteViewSpotInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int RouteID { get; set; }

        [Required]
        public int ViewSpotID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int EditUser { get; set; }
    }
}

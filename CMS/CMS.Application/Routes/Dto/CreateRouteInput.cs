using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Routes.Dto
{
    public class CreateRouteInput
    {
        [Required]
        public string RouteName { get; set; }

        public string RouteContent { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int CreatUser { get; set; }
    }
}

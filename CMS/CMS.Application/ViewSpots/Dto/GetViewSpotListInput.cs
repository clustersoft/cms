using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ViewSpots.Dto
{
    public class GetViewSpotListInput
    {
        [Required]
        public int PageIndex { get; set; }

        public string Keywords { get; set; }
    }
}

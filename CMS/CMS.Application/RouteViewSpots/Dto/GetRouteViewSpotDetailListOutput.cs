using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.RouteViewSpots.Dto
{
    public class GetRouteViewSpotDetailListOutput
    {
        public int ViewSpotID { get; set; }

        public string ViewSpotName { get; set; }

        public int OrderID { get; set; }

        public string JD { get; set; }

        public string WD { get; set; }

        public double Radius { get; set; }
    }
}

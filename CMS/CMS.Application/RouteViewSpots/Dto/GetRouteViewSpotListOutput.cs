using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.RouteViewSpots.Dto
{
    public class GetRouteViewSpotListOutput
    {
        public int ID { get; set; }

        public int RouteID { get; set; }

        public string RouteName{ get; set; }

        public int ViewSpotID { get; set; }

        public string ViewSpotName { get; set; }

        public int OrderID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Routes.Dto
{
    public class GetRouteOutput
    {
        public int ID { get; set; }

        public string RouteName { get; set; }

        public string RouteContent { get; set; }

        public string OrderID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.Routes
{
    public class RouteService:BaseService<Route>,IRouteService
    {
        public RouteService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}

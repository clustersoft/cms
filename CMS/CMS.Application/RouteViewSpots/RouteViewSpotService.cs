using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.RouteViewSpots.Dto;
using CMS.Model;

namespace CMS.Application.RouteViewSpots
{
    public class RouteViewSpotService : BaseService<RouteViewSpot>, IRouteViewSpotService
    {
        public RouteViewSpotService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<GetRouteViewSpotDetailListOutput> GetDetailList(int routeID)
        {
            var list = (from a in db.RouteViewSpots
                        join c in db.ViewSpots on a.ViewSpotID equals c.ID into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where a.RouteID == routeID
                        orderby a.OrderID
                        select new GetRouteViewSpotDetailListOutput
                        {
                            OrderID = a.OrderID,
                            ViewSpotID = a.ViewSpotID,
                            ViewSpotName = tt2.Name ?? "",
                            JD = tt2.Jd??"",
                            WD = tt2.Wd??"",
                            Radius = tt2.Radius??0
                        }).ToList();
            return list;
        }

        public List<GetRouteViewSpotListOutput> GetList(int RouteID)
        {
            var list = (from a in db.RouteViewSpots
                        join b in db.Routes on a.RouteID equals b.ID into t1
                        from tt in t1.DefaultIfEmpty()
                        join c in db.ViewSpots on a.ViewSpotID equals c.ID into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where a.RouteID == RouteID
                        orderby a.OrderID
                        select new GetRouteViewSpotListOutput
                        {
                            ID = a.ID,
                            OrderID = a.OrderID,
                            ViewSpotID = a.ViewSpotID,
                            ViewSpotName = tt2.Name ?? "",
                            RouteID = a.RouteID,
                            RouteName = tt.RouteName ?? ""
                        }).ToList();
            return list;
        }
    }
}

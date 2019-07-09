using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.RouteViewSpots.Dto;
using CMS.Model;

namespace CMS.Application.RouteViewSpots
{
    public interface IRouteViewSpotService:IBaseService<RouteViewSpot>
    {
        /// <summary>
        /// 景点路线列表
        /// </summary>
        /// <param name="RouteID"></param>
        /// <returns></returns>
        List<GetRouteViewSpotListOutput> GetList(int RouteID);

        /// <summary>
        /// 前台景点路线列表
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        List<GetRouteViewSpotDetailListOutput> GetDetailList(int routeID);
    }
}

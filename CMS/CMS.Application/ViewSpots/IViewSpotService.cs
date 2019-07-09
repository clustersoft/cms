using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ViewSpots.Dto;
using CMS.Model;

namespace CMS.Application.ViewSpots
{
    public interface IViewSpotService:IBaseService<ViewSpot>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ViewSpot AddViewSpot(CreateViewSpotInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool EditViewSpot(UpdateViewSpotInput input);

        /// <summary>
        /// 景点列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        List<GetViewSpotListOutput> GetList(int limit, int offset, out int total, GetViewSpotListInput input);

        /// <summary>
        /// 景点路线列表
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        List<GetViewRouteListOutput> GetRouteList(int routeID);

        /// <summary>
        /// 前台景点列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        List<GetViewListOutput> GetViewList(int limit, int offset, out int total,string keyWords);
    }
}

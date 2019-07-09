using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Base;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.Routes;
using CMS.Application.Routes.Dto;
using CMS.Application.RouteViewSpots;
using CMS.Application.RouteViewSpots.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/route")]
    public class RouteController : BaseApiController
    {
        private readonly IRouteService _routeService;
        private readonly IRouteViewSpotService _routeViewSpotService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private string _moduleName = "路线";

        public RouteController(IRouteService routeService, ILogService logService, 
            ISystemConfigurationService systemConfigurationService,IRouteViewSpotService routeViewSpotService)
        {
            _routeService = routeService;
            _routeViewSpotService = routeViewSpotService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
        }

        #region 前台
        [HttpGet]
        [Route("routelist")]
        public ResponseInfoModel RouteList([FromUri]GetObjFontListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = (input.Keywords ?? "").Trim();
                int pageSize =input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList = _routeService.GetPageList(limit, offset, out total,
                    a => string.IsNullOrEmpty(keywords) || a.RouteName.Contains(keywords), true,
                    a => a.OrderID).Select(s => new { s.ID, s.RouteName, s.OrderID }).ToList();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/routelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("routeinfo")]
        public ResponseInfoModel RouteInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var route = _routeService.Get(ID);
                var output = route.MapTo<GetRouteOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/routeinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("routedetaillist")]
        public ResponseInfoModel Routedetaillist(int RouteID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _routeViewSpotService.GetDetailList(RouteID);
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/routedetaillist", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region route
        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetRouteListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = (input.Keywords ?? "").Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList = _routeService.GetPageList(limit, offset, out total,
                    a => string.IsNullOrEmpty(keywords) || a.RouteName.Contains(keywords), true,
                    a => a.OrderID).MapTo<List<GetRouteListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public ResponseInfoModel Add([FromBody]CreateRouteInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var route = input.MapTo<Route>();
                route.CreateIP = IPHelper.GetIPAddress;
                route.CreatTime=DateTime.Now;

                var entity = _routeService.Insert(route);
                if (entity == null)
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.InsertFail;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Insert,
                        SourceType = _moduleName,
                        SourceID = entity.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.CreatUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/add", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ResponseInfoModel Edit([FromBody]UpdateRouteInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var route = _routeService.Get(input.ID);
                if (route == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                route = input.MapTo(route);

                route.EditTime=DateTime.Now;
                route.EditIP = IPHelper.GetIPAddress;

                if (!_routeService.Update(route))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.UpdateFail;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Update,
                        SourceType = _moduleName,
                        SourceID = input.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.EditUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/edit", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteWildlifeManagementInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (_routeViewSpotService.GetNoTrackingList(a => idInts.Contains(a.RouteID)).Any())
                {
                    throw new UserFriendlyException("有路线景点顺序不能删除");
                }

                if (!_routeService.Delete(a => idInts.Contains(a.ID)))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.DeleteFail;
                }
                else
                {
                    foreach (var id in idInts)
                    {
                        _logService.Insert(new Log()
                        {
                            ActionContent = LocalizationConst.Delete,
                            SourceType = _moduleName,
                            SourceID = id,
                            LogUserID = input.UserID,
                            LogTime = DateTime.Now,
                            LogIPAddress = IPHelper.GetIPAddress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel GetInfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var route = _routeService.Get(id);
                var output = route.MapTo<GetRouteOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region routeViewSpot
        [Authorize]
        [HttpGet]
        [Route("routespotlist")]
        public ResponseInfoModel RoutespotList(int RouteID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _routeViewSpotService.GetList(RouteID);
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/routespotlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addroutespot")]
        public ResponseInfoModel AddRoutespot([FromBody]CreateRouteViewSpotInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var route = input.MapTo<RouteViewSpot>();
                route.CreatTime = DateTime.Now;
                route.CreateIP = IPHelper.GetIPAddress;

                var entity = _routeViewSpotService.Insert(route);
                if (entity == null)
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.InsertFail;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Insert,
                        SourceType = _moduleName,
                        SourceID = entity.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.CreatUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/addroutespot", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editroutespot")]
        public ResponseInfoModel EditRoutespot([FromBody]UpdateRouteViewSpotInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var routeViewSpot = _routeViewSpotService.Get(input.ID);
                if (routeViewSpot == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                routeViewSpot = input.MapTo(routeViewSpot);
                routeViewSpot.EditTime = DateTime.Now;
                routeViewSpot.EditIP = IPHelper.GetIPAddress;

                if (!_routeViewSpotService.Update(routeViewSpot))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.UpdateFail;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Update,
                        SourceType = _moduleName,
                        SourceID = input.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.EditUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/editroutespot", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("deleteroutespot")]
        public ResponseInfoModel DeleteRoutespot([FromBody]DeleteWildlifeManagementInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (!_routeViewSpotService.Delete(a => idInts.Contains(a.ID)))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.DeleteFail;
                }
                else
                {
                    foreach (var id in idInts)
                    {
                        _logService.Insert(new Log()
                        {
                            ActionContent = LocalizationConst.Delete,
                            SourceType = _moduleName,
                            SourceID = id,
                            LogUserID = input.UserID,
                            LogTime = DateTime.Now,
                            LogIPAddress = IPHelper.GetIPAddress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/route/deleteroutespot", LocalizationConst.DeleteFail);
            }
            return json;
        }
        #endregion
    }
}

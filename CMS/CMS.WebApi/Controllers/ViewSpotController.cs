using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.Base;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.RouteViewSpots;
using CMS.Application.SystemConfigurations;
using CMS.Application.ViewSpotContents;
using CMS.Application.ViewSpotContents.Dto;
using CMS.Application.ViewSpots;
using CMS.Application.ViewSpots.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/viewspot")]
    public class ViewSpotController : BaseApiController
    {
        private readonly IViewSpotService _viewSpotService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IArticleAttachService _articleAttachService;
        private readonly IViewSpotContentService _viewSpotContentService;
        private readonly IRouteViewSpotService _routeViewSpotService;
        private string _moduleName = "景点";

        public ViewSpotController(IViewSpotService viewSpotService, ILogService logService,
            ISystemConfigurationService systemConfigurationService, IArticleAttachService articleAttachService,
            IViewSpotContentService ViewSpotContentService, IRouteViewSpotService routeViewSpotService)
        {
            _viewSpotService = viewSpotService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleAttachService = articleAttachService;
            _viewSpotContentService = ViewSpotContentService;
            _routeViewSpotService = routeViewSpotService;
        }

        #region viewspot

        #region 前台
        [HttpGet]
        [Route("viewlist")]
        public ResponseInfoModel ViewList([FromUri]GetObjFontListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                string keywords = input.Keywords ?? "".Trim();
                var outputList = _viewSpotService.GetViewList(limit, offset, out total, keywords);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/viewlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("viewinfo")]
        public ResponseInfoModel ViewInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var viewSpot = _viewSpotService.Get(ID);
                var output = viewSpot.MapTo<GetViewSpotInfoOutput>();
                if (output != null)
                {
                    var attach = _articleAttachService.Get(a => a.ModuleType == (int)AttachTypesEnum.景点附件 && a.ArticleGuid == viewSpot.FileID);
                    output.AttachUrl = attach == null ? "" :GetPublishUrl()+attach.AttachUrl;
                    json.Result = output;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/viewlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        #endregion

        [Authorize]
        [HttpPost]
        [Route("add")]
        public ResponseInfoModel Add([FromBody]CreateViewSpotInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _viewSpotService.AddViewSpot(input);
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
                DisposeUserFriendlyException(e, ref json, "api/viewspot/add", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ResponseInfoModel EditInfo([FromBody]UpdateViewSpotInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_viewSpotService.EditViewSpot(input))
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
                DisposeUserFriendlyException(e, ref json, "api/viewspot/edit", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetViewSpotListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList = _viewSpotService.GetList(limit, offset, out total, input);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("typelist")]
        public ResponseInfoModel TypeList(int ViewSpotID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _viewSpotContentService.GetNoTrackingList(a => a.ViewSpotID == ViewSpotID).Select(s => new { ID = s.ID, Type = ((ViewSpotContentTypesEnum)s.Type).ToString() });
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/typelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("routelist")]
        public ResponseInfoModel Routelist(int RouteID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _viewSpotService.GetRouteList(RouteID);
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/routelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteViewSpotInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (_viewSpotContentService.GetNoTrackingList(a => idInts.Contains(a.ViewSpotID)).Any())
                {
                    throw new UserFriendlyException("景点有详情信息不能删除");
                }

                if (_routeViewSpotService.GetNoTrackingList(a => idInts.Contains(a.ViewSpotID)).Any())
                {
                    throw new UserFriendlyException("已在景点路线中不能删除");
                }

                if (!_viewSpotService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/viewSpot/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var viewSpot = _viewSpotService.Get(id);
                var output = viewSpot.MapTo<GetViewSpotOutput>();
                if (output != null)
                {
                    output.Attach = _articleAttachService.Get(a => a.ModuleType == (int)AttachTypesEnum.景点附件 && a.ArticleGuid == viewSpot.FileID).MapTo<GetObjAttachOutput>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewSpot/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region ViewContent

        #region 前台
        [HttpGet]
        [Route("viewdetailinfo")]
        public ResponseInfoModel ViewDetailInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var list = _viewSpotContentService.GetInfo(ID);
                json.Result = list;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/viewdetailinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        #endregion

        [Authorize]
        [HttpPost]
        [Route("adddetail")]
        public ResponseInfoModel AddDetail([FromBody]CreateViewSpotContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _viewSpotContentService.Add(input);
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
                DisposeUserFriendlyException(e, ref json, "api/viewspot/adddetail", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("eidtdetail")]
        public ResponseInfoModel Eidtdetail([FromBody]UpdateViewSpotContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_viewSpotContentService.Edit(input))
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
                DisposeUserFriendlyException(e, ref json, "api/viewspot/eidtdetail", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("detaillist")]
        public ResponseInfoModel Detaillist(int ViewSpotID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _viewSpotContentService.GetNoTrackingList(a => a.ViewSpotID == ViewSpotID).MapTo<List<GetViewSpotContentListOutput>>();
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewspot/detaillist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("detaildelete")]
        public ResponseInfoModel DetailDelete([FromBody]DeleteViewSpotContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (!_viewSpotContentService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/viewSpot/detaildelete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getdetailinfo")]
        public ResponseInfoModel GetDetailInfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var viewSpotContent = _viewSpotContentService.Get(id);
                var output = viewSpotContent.MapTo<GetViewSpotContentOutput>();
                if (output != null)
                {
                    output.Attachs = _articleAttachService.GetNoTrackingList(a => a.ModuleType == (int)AttachTypesEnum.景点详情附件 && a.ArticleGuid == viewSpotContent.FileID).MapTo<List<GetObjAttachOutput>>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/viewSpot/getdetailinfo", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion
    }
}

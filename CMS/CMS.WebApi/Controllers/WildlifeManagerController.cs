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
using CMS.Application.SystemConfigurations;
using CMS.Application.WildlifeContents;
using CMS.Application.WildlifeContents.Dto;
using CMS.Application.WildlifeManagers;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/wildlifemanager")]
    public class WildlifeManagerController : BaseApiController
    {
        private readonly IWildlifeManagerService _wildlifeManagerService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IArticleAttachService _articleAttachService;
        private readonly IWildlifeContentService _wildlifeContentService;
        private string _moduleName = "动植物管理";

        public WildlifeManagerController(IWildlifeManagerService wildlifeManagerService, ILogService logService,
            ISystemConfigurationService systemConfigurationService, IArticleAttachService articleAttachService,
            IWildlifeContentService wildlifeContentService)
        {
            _wildlifeManagerService = wildlifeManagerService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleAttachService = articleAttachService;
            _wildlifeContentService=wildlifeContentService;
        }

        #region wildlifeManager

        #region 前台
        [HttpGet]
        [Route("wildlist")]
        public ResponseInfoModel WildList([FromUri]GetObjFontListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList = _wildlifeManagerService.GetList(limit, offset, out total, input.Keywords);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/wildlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("wildinfo")]
        public ResponseInfoModel WildInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var wild = _wildlifeManagerService.GetWildlifeManagerInfo(ID);
                json.Result = wild;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/wildinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        #endregion

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetWildlifeManagementListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.Pageindex - 1);
                int total;
                var outputList = _wildlifeManagerService.GetList(limit, offset, out total, input.Keywords);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public ResponseInfoModel Add([FromBody]CreateWildlifeManagementInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _wildlifeManagerService.Add(input);
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/add", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ResponseInfoModel Edit([FromBody]UpdateWildlifeManagementInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_wildlifeManagerService.Edit(input))
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/edit", LocalizationConst.UpdateFail);
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
                CheckModelState();

                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (_wildlifeContentService.GetNoTrackingList(a => idInts.Contains(a.WildlifeID)).Any())
                {
                    throw new UserFriendlyException("有详情信息的不能删除");
                }

                if (!_wildlifeManagerService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/delete", LocalizationConst.DeleteFail);
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
                var wildlifeManager = _wildlifeManagerService.Get(id);
                var output = wildlifeManager.MapTo<GetWildlifeManagementOutput>();
                if (output != null)
                {
                    output.Attach = _articleAttachService.Get(a => a.ModuleType == (int)AttachTypesEnum.动植物管理附件 && a.ArticleGuid == wildlifeManager.FileID).MapTo<GetObjAttachOutput>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region WildlifeContent

        #region 前台
        [HttpGet]
        [Route("wilddetailinfo")]
        public ResponseInfoModel WildDetailInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
               var output=_wildlifeContentService.GetWildlifeContentInfo(ID);
               json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/wilddetailinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        #endregion

        [Authorize]
        [HttpPost]
        [Route("adddetail")]
        public ResponseInfoModel AddDetail([FromBody]CreateWildlifeContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _wildlifeContentService.Add(input);
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/adddetail", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editdetail")]
        public ResponseInfoModel EditDetail([FromBody]UpdateWildlifeContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_wildlifeContentService.Edit(input))
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/editdetail", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("deletedetail")]
        public ResponseInfoModel DeleteDetail([FromBody]DeleteWildliftContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (!_wildlifeContentService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/deletedetail", LocalizationConst.DeleteFail);
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
                var wildlifeContent = _wildlifeContentService.Get(id);
                var output = wildlifeContent.MapTo<GetWildlifeContentOutput>();
                if (output != null)
                {
                    output.Attachs = _articleAttachService.GetNoTrackingList(a => a.ModuleType == (int)AttachTypesEnum.动植物管理详细附件 && a.ArticleGuid == wildlifeContent.FileID).MapTo<List<GetObjAttachOutput>>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/getdetailinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("detaillist")]
        public ResponseInfoModel DetailList(int WildlifemanagerID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var wildlifeContent = _wildlifeContentService.GetNoTrackingList(a=>a.WildlifeID== WildlifemanagerID)
                    .Select(s=>new GetWildliftContentListOutput
                    {
                        ID = s.ID,
                        Type = s.Type,
                        TypeName = ((WildlifeContentTypesEnum)s.Type).ToString()
                    });

                json.Result = wildlifeContent;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifemanager/detaillist", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion
    }
}

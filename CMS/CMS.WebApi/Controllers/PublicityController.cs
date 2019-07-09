using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.PublicityCategories;
using CMS.Application.PublicityCategories.Dto;
using CMS.Application.PublicityContents;
using CMS.Application.PublicityContents.Dto;
using CMS.Application.PublicityTypes;
using CMS.Application.PublicityTypes.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Application.Users.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/publicity")]
    public class PublicityController : BaseApiController
    {
        private readonly IPublicityContentService _PublicityContentService;
        private readonly ILogService _logService;
        private readonly IPublicityTypesService _publicityTypesService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IPublicityCategoryService _publicityCategoryService;
        private readonly IArticleAttachService _articleAttachService;
        private string _moduleName = "宣传";

        public PublicityController(IPublicityContentService publicityContentService, ILogService logService, IPublicityTypesService publicityTypesService,
            IPublicityCategoryService publicityCategoryService, ISystemConfigurationService systemConfigurationService, IArticleAttachService articleAttachService)
        {
            _PublicityContentService = publicityContentService;
            _logService = logService;
            _publicityTypesService = publicityTypesService;
            _publicityCategoryService = publicityCategoryService;
            _systemConfigurationService = systemConfigurationService;
            _articleAttachService = articleAttachService;
        }

        #region 前台
        [HttpGet]
        [Route("showlist")]
        public ResponseInfoModel Showlist([FromUri]GetPublicityShowListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object()};
            try
            {
                var outputList = _PublicityContentService.GetShowList(input);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/showlist", LocalizationConst.QueryFail);
            }
            return json;
        }


        [HttpGet]
        [Route("showListByName")]
        public ResponseInfoModel ShowListByName([FromUri]GetPublicityShowNameListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var publicity = _publicityCategoryService.Get(a => a.PublicityCategoryName == input.PublicityCateName);
                if (publicity == null)
                {
                    json.Result= new ListInfo() { List = new object[] { } };
                    return json;
                }
                else
                {
                    GetPublicityShowListInput newInput = new GetPublicityShowListInput()
                    {
                        PublicityTypesID = input.PublicityTypesID,
                        PublicityCateID = publicity.ID,
                        Top = input.Top
                    };
                    var outputList = _PublicityContentService.GetShowList(newInput);
                    json.Result = new ListInfo() { List = outputList };
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/showListByName", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region publicityTypes
        [Authorize]
        [HttpGet]
        [Route("positiontypelist")]
        public ResponseInfoModel Positiontypelist()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var list = _publicityTypesService.GetNoTrackingList(a => true).OrderBy(a => a.OrderID).OrderByDescending(a => a.ID).ToList();
                var outputList = list.MapTo<List<GetPublicityTypeListOutput>>();
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/positiontypelist", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        #region publicityCategory
        [Authorize]
        [HttpGet]
        [Route("positionlist")]
        public ResponseInfoModel Positionlist([FromUri]GetPublicityCategoryListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                string keyword = input.Keyword ?? "".Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;

                var list = _publicityCategoryService.GetPageList(limit, offset, out total,
                    a => (string.IsNullOrEmpty(keyword) || a.PublicityCategoryName.Contains(keyword)) &&
                        (input.PublicityTypesID == 0 || a.PublicityTypesID == input.PublicityTypesID), true,
                    a => a.OrderID).Include(a => a.PublicityType).ToList();
                var outputList = list.MapTo<List<GetPublicityCategoryListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/positionlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getpositionInfo")]
        public ResponseInfoModel GetpositionInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var position = _publicityCategoryService.Get(ID);
                var output = position.MapTo<GetPublicityCategoryOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/positionlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("selpositionlist")]
        public ResponseInfoModel Selpositionlist(int PublicityTypesID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var list = _publicityCategoryService.GetNoTrackingList(a => a.PublicityTypesID == PublicityTypesID)
                        .OrderBy(a => a.OrderID)
                        .OrderByDescending(a => a.CreateTime)
                        .ToList();
                var outputList = list.MapTo<List<GetSelCategoryOutput>>();
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/contentlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addpositionInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreatePublicityCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                var publicityCategory = input.MapTo<PublicityCategory>();
                publicityCategory.CreateTime = DateTime.Now;
                publicityCategory.CreateIP = IPHelper.GetIPAddress;

                var entity = _publicityCategoryService.Insert(publicityCategory);
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
                        LogUserID = input.CreateUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/addpositionInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editpositionInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdatePublicityCategoyrInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var publicityCategory = _publicityCategoryService.Get(input.ID);
                if (publicityCategory == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }

                publicityCategory = input.MapTo(publicityCategory);
                publicityCategory.ModifyTime = DateTime.Now;
                publicityCategory.ModifyIP = IPHelper.GetIPAddress;

                if (!_publicityCategoryService.Update(publicityCategory))
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
                        LogUserID = input.ModifyUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/editpositionInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("deleteposition")]
        public ResponseInfoModel Delete([FromBody]DeleteUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.ids);

                if (_PublicityContentService.GetNoTrackingList(a => idInts.Contains(a.PublicityCategoryID)).Any())
                {
                    throw new UserFriendlyException("位置下有链接不可删除");
                }

                if (!_publicityCategoryService.Delete(a => idInts.Contains(a.ID)))
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
                            LogUserID = input.userID,
                            LogTime = DateTime.Now,
                            LogIPAddress = IPHelper.GetIPAddress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/deleteposition", LocalizationConst.DeleteFail);
            }
            return json;
        }
        #endregion

        #region PublicityContent
        [Authorize]
        [HttpGet]
        [Route("getcontentInfo")]
        public ResponseInfoModel GetcontentInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var content = _PublicityContentService.Get(ID);
                var output = content.MapTo<GetPublicityContentOutput>();
                if (output != null)
                {
                    output.Attach = _articleAttachService.Get(a => a.ArticleGuid == content.AttachGuid && a.ModuleType == 2).MapTo<GetObjAttachOutput>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/getcontentInfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addcontentInfo")]
        public ResponseInfoModel AddcontentInfo([FromBody]CreatePublicityContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _PublicityContentService.AddInfo(input);
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
                        LogUserID = input.CreateUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/addpositionInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editcontentInfo")]
        public ResponseInfoModel EditcontentInfo([FromBody]UpdatePublicityContentInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_PublicityContentService.EditInfo(input))
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
                        LogUserID = input.ModifyUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/editcontentInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("contentlist")]
        public ResponseInfoModel Contentlist([FromUri]GetPublicityContentListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList = _PublicityContentService.GetContentList(limit, offset, out total, input);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/contentlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("deletecontent")]
        public ResponseInfoModel Deletecontent([FromBody]DeleteUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.ids);

                if (!_PublicityContentService.Delete(a => idInts.Contains(a.ID)))
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
                            LogUserID = input.userID,
                            LogTime = DateTime.Now,
                            LogIPAddress = IPHelper.GetIPAddress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/publicity/deletecontent", LocalizationConst.DeleteFail);
            }
            return json;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Application.Templates;
using CMS.Application.Templates.Dto;
using CMS.Application.Users.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/template")]
    public class TemplateController : BaseApiController
    {
        private readonly ITemplateService _templateService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IArticleAttachService _articleAttachService;
        private string _moduleName = "模板";

        public TemplateController(ITemplateService templateService, ILogService logService,
       ISystemConfigurationService systemConfigurationService,IArticleAttachService articleAttachService)
        {
            _templateService = templateService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleAttachService = articleAttachService;
        }

        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var template = _templateService.Get(ID);
                var attach = _articleAttachService.Get(a => a.ArticleGuid == template.Guid);
                var output = template.MapTo<GetTemplateOutput>();
                output.Attach = attach.MapTo<GetTemplateAttachOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/template/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateTemplateInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var entity = _templateService.Addinfo(input);

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
                DisposeUserFriendlyException(e, ref json, "api/template/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateTemplateInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_templateService.Editinfo(input))
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
                DisposeUserFriendlyException(e, ref json, "api/template/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                int[] idInts = ConvertStringToIntArr(input.ids);

                if (!_templateService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/template/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetTemplateListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = (input.Keyword ?? "").Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var templateList = _templateService.GetPageList(limit, offset, out total,a=>string.IsNullOrEmpty(keywords) ||a.Name.Contains(keywords),true,a=>a.OrderID);
                var outputList = templateList.MapTo<List<GetTemplateListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList }; ;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/template/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("selectlist")]
        public ResponseInfoModel Selectlist()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var templateList = _templateService.GetNoTrackingList(a => true).ToList();
                var outputList = templateList.MapTo<List<GetTemplateSelectListOutput>>();
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/template/selectlist", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

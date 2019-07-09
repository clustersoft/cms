using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.Logs.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/logs")]
    public class LogController : BaseApiController
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly ILogService _logService;
        private string _moduleName = "日志";

        public LogController(ISystemConfigurationService systemConfigurationService, ILogService logService)
        {
            _systemConfigurationService = systemConfigurationService;
            _logService = logService;
        }

        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo([FromUri]GetLogInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keyword = (input.Keyword ?? "").Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.pageIndex - 1);
                int total;
                var output =_logService.GetPageList(limit, offset, out total, a =>(string.IsNullOrEmpty(keyword) ||(a.User.UserName.Contains(keyword) ||a.ActionContent.Contains(keyword)))&&(input.LogUserID==0||a.LogUserID == input.LogUserID), false,a => a.LogTime).Include(a=>a.User).ToList();

                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = output.MapTo<List<GetLogOutput>>() }; ;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/log/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetLogListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keyword = (input.Keyword ?? "").Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.pageIndex - 1);
                int total;
                var output = _logService.GetPageList(limit, offset, out total, a => (string.IsNullOrEmpty(keyword) || (a.User.UserName.Contains(keyword) || a.ActionContent.Contains(keyword)))&&(input.LogUserID == 0 || a.LogUserID == input.LogUserID), false, a => a.LogTime).Include(a => a.User).ToList();

                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = output.MapTo<List<GetLogListOutput>>() }; ;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/log/list", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

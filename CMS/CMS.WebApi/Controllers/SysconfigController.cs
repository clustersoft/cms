using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Application.Users.Dto;
using CMS.Model;
using CMS.Util;
using WebGrease;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/sysconfig")]
    public class SysconfigController : BaseApiController
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly ILogService _logService;
        private string _moduleName = "系统配置";

        public SysconfigController(ISystemConfigurationService systemConfigurationService,ILogService logService)
        {
            _systemConfigurationService = systemConfigurationService;
            _logService = logService;
        }

        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var output = _systemConfigurationService.FirstOrDefault();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/sysconfig/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]SystemConfiguration input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                if (!_systemConfigurationService.Update(input))
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
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/sysconfig/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }
    }
}

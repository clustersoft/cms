using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.LeaveMessages;
using CMS.Application.LeaveMessages.Dto;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/advice")]
    public class AdviceController : BaseApiController
    {
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly ILeaveMessageService _leaveMessageService;
        private string _moduleName = "建议";

        public AdviceController(ILeaveMessageService leaveMessageService, ILogService logService, ISystemConfigurationService systemConfigurationService)
        {
            _leaveMessageService = leaveMessageService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
        }

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetLeaveMessageListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var roleList = _leaveMessageService.GetPageList(limit, offset, out total, a => string.IsNullOrEmpty((input.Keyword ?? "").Trim()) || (a.Name.Contains((input.Keyword??"").Trim())||a.Contents.Contains(input.Keyword ?? "".Trim())), false, a => a.LeaveTime).ToList();
                var list = roleList.MapTo<List<GetLeaveMessageListOuput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = list };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/advice/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpPost]
        [Route("add")]
        public ResponseInfoModel AddInfo([FromBody]CreateLeaveMessageInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var output = _leaveMessageService.AddInfo(input);
                if (output == null)
                {
                    json.Success = 0;
                    json.Result = 1;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Insert,
                        SourceType = _moduleName,
                        SourceID = output.ID,
                        LogTime = DateTime.Now,
                        LogUserID = 0,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/advice/add", LocalizationConst.InsertFail);
            }
            return json;
        }
    }
}

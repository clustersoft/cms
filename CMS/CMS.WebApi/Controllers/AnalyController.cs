using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.VisitRecords;
using CMS.Model;
using CMS.Util;
using CMS.WebApi.Filter;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/analy")]
    public class AnalyController : BaseApiController
    {
        private readonly IVisitRecordService _visitRecordService;
        private string _moduleName = "建议";

        public AnalyController(IVisitRecordService visitRecordService)
        {
            _visitRecordService = visitRecordService;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var list = _visitRecordService.GetAnalyList(StartDate, EndDate);
                json.Result = list;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("getInfo")]
        [WebApiOutputCache(60, 30, false)]
        public ResponseInfoModel GetInfo()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetInfo();
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/getInfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("NRCountlist")]
        public ResponseInfoModel NRCountlist(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetNrCountList(StartDate,EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/NRCountlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("NRIPlist")]
        public ResponseInfoModel NRIPList(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetNrIPList(StartDate, EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/NRIPlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("NRlist")]
        public ResponseInfoModel NRList(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetNrList(StartDate, EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/NRCountlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("LMlist")]
        public ResponseInfoModel LMList(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetLMlist(StartDate, EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/LMlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("LMtbCountlist")]
        public ResponseInfoModel LMtbCountlist(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetLMtbCountlist(StartDate, EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/LMtbCountlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("LMtbIPlist")]
        public ResponseInfoModel LMtbIPlist(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outPutList = _visitRecordService.GetLMtbIPList(StartDate, EndDate);
                json.Result = outPutList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/LMtbIPlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("AnalyStartDate")]
        public ResponseInfoModel AnalyStartDate()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var output = _visitRecordService.MinDateTime();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/analy/AnalyStartDate", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

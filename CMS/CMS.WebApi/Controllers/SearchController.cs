using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Articles;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/search")]
    public class SearchController : BaseApiController
    {
        private readonly IArticleService _articleService;
        private string _moduleName = "发布统计";

        public SearchController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Route("fblist")]
        public ResponseInfoModel Fblist(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                json.Result = _articleService.GetFBList(StartDate, EndDate);
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/search/fblist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("sourelist")]
        public ResponseInfoModel Sourelist(DateTime? StartDate, DateTime? EndDate)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                json.Result = _articleService.GetSoureList(StartDate, EndDate);
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/search/sourelist", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

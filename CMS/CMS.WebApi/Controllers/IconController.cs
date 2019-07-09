using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.IconLists;
using CMS.Application.Navgations.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/icon")]
    public class IconController : BaseApiController
    {
        private readonly XIIconListService _iconListService;

        public IconController(XIIconListService iconListService)
        {
            _iconListService = iconListService;
        }


        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputlist = _iconListService.GetNoTrackingList(a => true);
                json.Result = outputlist;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/icon/list", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

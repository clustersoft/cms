using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.Navgations;
using CMS.Application.Navgations.Dto;
using CMS.Application.Users;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/permission")]
    public class PermissionController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly INavgationService _navgationService;
        private string _moduleName = "权限";

        public PermissionController(IUserService userService,INavgationService navgationService)
        {
            _userService = userService;
            _navgationService = navgationService;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List(int UserID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                List<Navgation> list=new List<Navgation>();
                var user = _userService.Get(UserID);
                if (user == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }

                if (user.Type == 1)
                {
                    list = _navgationService.GetNoTrackingList(a => true).OrderBy(a => a.OrderID).ToList();
                }
                else
                {
                    string[] codes = _userService.GetPermNavCodes(UserID);
                    list =_navgationService.GetNoTrackingList(a => codes.Contains(a.NavName)).OrderBy(a => a.OrderID).ToList();
                }
                var outputlist = list.MapTo<List<GetPermissionNavListOutput>>();
                json.Result = outputlist;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/permission/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("navshowlist")]
        public ResponseInfoModel Navshowlist(int UserID,string NavCode)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var user = _userService.Get(UserID);
                if (user == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                } 
                var outputlist = _userService.GetNavCode(UserID, NavCode);
                json.Result = outputlist;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/permission/navshowlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("categoryAction")]
        public ResponseInfoModel CategoryAction(int UserID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var user = _userService.Get(UserID);
                if (user == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                var outputlist = _userService.GetCategoryAction(UserID);
                json.Result = outputlist;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/permission/categoryAction", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

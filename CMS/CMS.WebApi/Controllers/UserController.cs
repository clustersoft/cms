using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Application.Users;
using CMS.Application.Users.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private string _moduleName = "用户";

        public UserController(IUserService userService, ILogService logService, ISystemConfigurationService systemConfigurationService)
        {
            _userService = userService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
        }

        [HttpPost]
        [Route("login")]
        public ResponseInfoModel Login([FromBody]LoginInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var user = _userService.Login(input);
                if (user == null)
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.LoginFail;
                }
                else
                {
                    json.Result = user;
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Login,
                        SourceType = _moduleName,
                        SourceID = user.ID,
                        LogTime = DateTime.Now,
                        LogUserID = user.ID,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/login", LocalizationConst.LoginFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("changePwd")]
        public ResponseInfoModel ChangePwd([FromBody]ChangePwdInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_userService.ChangePassWord(input))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.ChangePwdFail;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/changePwd", LocalizationConst.ChangePwdFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int[] idInts = ConvertStringToIntArr(input.RoleIDS);
                var user = input.MapTo<User>();
                user.CreateTime = DateTime.Now;
                user.CreateIP = IPHelper.GetIPAddress;

                var entity = _userService.AddInfo(user,idInts);
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
                DisposeUserFriendlyException(e, ref json, "api/user/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                int[] idInts =ConvertStringToIntArr(input.RoleIDS);

                if (!_userService.editInfo(input,idInts))
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
                        LogUserID=input.ModifyUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetUserListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = (input.keyword ?? "").Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.pageIndex - 1);
                int total;
                List<User> userList = _userService.GetPageList(limit, offset, out total, a =>string.IsNullOrEmpty(keywords) ||(a.UserName.Contains(keywords) ||a.LoginName.Contains(keywords)), 
                    true, a => a.OrderID).Include(a=>a.UserRoles).ToList();
                var outputList = userList.MapTo<List<GetUserListOutPut>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount =(int)Math.Ceiling((decimal)total /pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("selectlist")]
        public ResponseInfoModel Selectlist()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var list = _userService.GetNoTrackingList(a => true).Select(s=>new GetSelectListOutput() {ID = s.ID,UserName = s.UserName}).ToList();
                json.Result = list;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/selectlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("valid")]
        public ResponseInfoModel Valid(string loginName)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgation = _userService.Get(a => a.LoginName == loginName);
                if (navgation != null)
                {
                    json.Success = 0;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/valid", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteUserInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.ids);
                if (!_userService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/user/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var user = _userService.GetInclude(id);
                var output = user.MapTo<GetUserOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/user/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("changePersonalInfo")]
        public ResponseInfoModel ChangePersonalInfo([FromBody]ChangePersonalInfoInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var user = _userService.Get(input.ID);
                if (user == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                user = input.MapTo(user);
                user.ModifyTime=DateTime.Now;
                user.ModifyIP = IPHelper.GetIPAddress;

                if (!_userService.Update(user))
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
                DisposeUserFriendlyException(e, ref json, "api/user/changePersonalInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }
    }
}

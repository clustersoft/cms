using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Actions.Dto;
using CMS.Application.ArticleCategories;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.Navgations;
using CMS.Application.Navgations.Dto;
using CMS.Application.Roles;
using CMS.Application.Roles.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Application.UserRoles;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/role")]
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly INavgationService _navgationService;
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly IActionService _actionService;
        private readonly IUserRoleService _userRoleService;
        private string _moduleName = "角色";

        public RoleController(IRoleService roleService, ILogService logService,ISystemConfigurationService systemConfigurationService,
            INavgationService navgationService,IArticleCategoryService articleCategoryService,IActionService actionService,IUserRoleService userRoleService)
        {
            _navgationService = navgationService;
            _roleService = roleService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleCategoryService=articleCategoryService;
            _actionService = actionService;
            _userRoleService = userRoleService;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetRoleListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var roleList = _roleService.GetPageList(limit, offset, out total, a => string.IsNullOrEmpty(input.Keywords) || (a.RoleName.Contains(input.Keywords)), true, a => a.OrderID).ToList();
                var list = roleList.MapTo<List<GetRoleListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize,list = list };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("roleslist")]
        public ResponseInfoModel Roleslist()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                var roleList = _roleService.GetNoTrackingList(a=>true).OrderBy(a=>a.OrderID).ThenByDescending(a=>a.CreateTime).ToList();
                var outputList = roleList.MapTo<List<GetRoleListListOutput>>();
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/roleslist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("firstloadInfo")]
        public ResponseInfoModel FirstloadInfo()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
               var navList=_navgationService.GetNoTrackingList(a => true).OrderBy(a=>a.OrderID).ToList();
               var categoryList= _articleCategoryService.GetList(a => true).OrderBy(a=>a.OrderID).ToList();
               var outputNavList = navList.MapTo<List<GetLoadNavgationListOutput>>();

                var actionList = _actionService.GetNoTrackingList(a => true);
                Dictionary<string, string> actionDictionary = new Dictionary<string, string>();
                foreach (var cmsAction in actionList)
                {
                    actionDictionary.Add(cmsAction.ActionCode, cmsAction.ActionName);
                }

                foreach (var getLoadNavgationListOutput in outputNavList)
                {
                    getLoadNavgationListOutput.ActionsName = ConvertToName(actionDictionary,getLoadNavgationListOutput.Actions);
                }
         
                var outputCategoryList = categoryList.MapTo< List<GetLoadCategoryListOutput>>();

                List<GetLoadNavgationListOutput> navnewList = new List<GetLoadNavgationListOutput>();
                GetChild(outputNavList, navnewList, 0, 0);

                List<GetLoadCategoryListOutput> categorynewList = new List<GetLoadCategoryListOutput>();
                GetChild(outputCategoryList, categorynewList, 0, 0);

                json.Result=new GetLoadListOutput() {navlist = navnewList, categorylist = categorynewList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/firstloadInfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        private string ConvertToName(Dictionary<string, string> actionDictionary,string actionTypes)
        {
            var arr = actionTypes.Split(',').Select(a => actionDictionary[a]);
            return string.Join(",", arr);
        }

        [HttpGet]
        [Route("getInfo")]
        public ResponseInfoModel GetInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var role=_roleService.Get(ID);
                var navlist = _navgationService.GetList(a => true).OrderBy(a => a.OrderID).MapTo<List<GetRoleNavgationListOutput>>();
                var categoryList = _articleCategoryService.GetList(a => true).OrderBy(a => a.OrderID).MapTo< List<GetRoleCategoryListOutput>>();

                var actionList = _actionService.GetList(a => true);
                Dictionary<string, string> actionDictionary = new Dictionary<string, string>();
                foreach (var cmsAction in actionList)
                {
                    actionDictionary.Add(cmsAction.ActionCode, cmsAction.ActionName);
                }

                foreach (var navgationListOutput in navlist)
                {
                    navgationListOutput.ActionsName = ConvertToName(actionDictionary, navgationListOutput.Actions);
                    navgationListOutput.SelActions = _roleService.GetSelectNavActions(ID, navgationListOutput.NavName);
                }

                foreach (var categoryListOutput in categoryList)
                {
                    categoryListOutput.SelActions = _roleService.GetSelCategoryActions(ID, categoryListOutput.ID);
                }

                List<GetRoleNavgationListOutput> navnewList = new List<GetRoleNavgationListOutput>();
                GetChild(navlist, navnewList, 0, 0);

                List<GetRoleCategoryListOutput> catenewList = new List<GetRoleCategoryListOutput>();
                GetChild(categoryList, catenewList, 0, 0);

                if (role != null)
                {
                    var output = role.MapTo<GetRoleOutput>();
                    output.Navlist = navnewList;
                    output.Categorylist = catenewList;
                    json.Result = output;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/getInfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateRoleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var role = _roleService.Addinfo(input);
                if (role == null)
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
                        SourceID = role.ID,
                        LogTime = DateTime.Now,
                        LogUserID = role.ID,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateRoleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_roleService.EditInfo(input))
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
                        LogUserID = input.ID,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteRoleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.ids);
                if (_userRoleService.GetList(a=> idInts.Contains(a.RoleID)).Any())
                {
                    throw new UserFriendlyException("角色下有用户不可删除");
                }

                if (!_roleService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/role/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [HttpGet]
        [Route("valid")]
        public ResponseInfoModel Valid(string RoleName)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgation = _roleService.Get(a => a.RoleName == RoleName);
                if (navgation != null)
                {
                    json.Success = 0;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/role/valid", LocalizationConst.QueryFail);
            }
            return json;
        }

        private void GetChild(List<GetLoadNavgationListOutput> oldList, List<GetLoadNavgationListOutput> newList, int parentId, int layer)
        {
            layer++;
            var list = oldList.Where(a => a.ParentID == parentId);
            foreach (var b in list)
            {
                b.Layer = layer;
                newList.Add(b);
                this.GetChild(oldList, newList, b.ID, layer);
            }
        }

        private void GetChild(List<GetLoadCategoryListOutput> oldList, List<GetLoadCategoryListOutput> newList, int parentId, int layer)
        {
            layer++;
            var list = oldList.Where(a => a.ParentID == parentId);
            foreach (var b in list)
            {
                b.Layer = layer;
                newList.Add(b);
                this.GetChild(oldList, newList, b.ID, layer);
            }
        }

        private void GetChild(List<GetRoleNavgationListOutput> oldList, List<GetRoleNavgationListOutput> newList, int parentId, int layer)
        {
            layer++;
            var list = oldList.Where(a => a.ParentID== parentId);
            foreach (var b in list)
            {
                b.Layer = layer;
                newList.Add(b);
                this.GetChild(oldList, newList, b.ID, layer);
            }
        }

        private void GetChild(List<GetRoleCategoryListOutput> oldList, List<GetRoleCategoryListOutput> newList, int parentId, int layer)
        {
            layer++;
            var list = oldList.Where(a => a.ParentID== parentId);
            foreach (var b in list)
            {
                b.Layer = layer;
                newList.Add(b);
                this.GetChild(oldList, newList, b.ID, layer);
            }
        }
    }
}

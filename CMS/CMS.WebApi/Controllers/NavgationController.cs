using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Actions.Dto;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.Navgations;
using CMS.Application.Navgations.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/navgation")]
    public class NavgationController : BaseApiController
    {
        private readonly INavgationService _navgationService;
        private readonly ILogService _logService;
        private readonly IActionService _actionService;

        private string _moduleName = "导航";

        public NavgationController(INavgationService navgationService, ILogService logService, ISystemConfigurationService systemConfigurationService,IActionService actionService)
        {
            _navgationService = navgationService;
            _logService = logService;
            _actionService = actionService;
        }

        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgationList = _navgationService.GetNoTrackingList(a =>true).OrderBy(a=>a.OrderID).ToList();
            
                var outputList = navgationList.MapTo<List<GetNavgationListOutput>>();

                var actionList = _actionService.GetNoTrackingList(a => true);
                Dictionary<string, string> actionDictionary = new Dictionary<string, string>();
                foreach (var cmsAction in actionList)
                {
                    actionDictionary.Add(cmsAction.ActionCode, cmsAction.ActionName);
                }

                foreach (var output in outputList)
                {
                    output.ActionTypesName = ConvertToName(actionDictionary,output.ActionTypesName);
                }
                List<GetNavgationListOutput> newList = new List<GetNavgationListOutput>();
                GetChild(outputList, newList, 0,0);
                json.Result = newList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        private string ConvertToName(Dictionary<string, string> actionDictionary,string actionTypes)
        {

            var arr = actionTypes.Split(',').Select(a => actionDictionary[a]);
            return string.Join(",", arr);
        }

        private void GetChild(List<GetNavgationListOutput> oldList, List<GetNavgationListOutput> newList, int parentId,int layer)
        {
            layer++;
            var list=oldList.Where(a=>a.ParentId==parentId);
            foreach (var b in list)
            {
                b.Layer = layer;
                newList.Add(b);
                this.GetChild(oldList, newList, b.ID, layer);
            }
        }

        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel Getinfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgation = _navgationService.Get(id);
                var output = navgation.MapTo<GetNavgationOutput>();
                if (output.ParentID == 0)
                {
                    output.ParentName = "无上级栏目";
                }
                else
                {
                    var parent = _navgationService.Get(output.ParentID);
                    output.ParentName = parent == null ? "无上级栏目" : parent.NavName;
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("actionslist")]
        public ResponseInfoModel Actionslist()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _actionService.GetNoTrackingList(a => true);
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/actionslist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("valid")]
        public ResponseInfoModel Valid(string NavName)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgation = _navgationService.Get(a => a.NavName == NavName);
                if (navgation != null)
                {
                    json.Success = 0;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/valid", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateNavgationInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (_navgationService.Get(a => a.NavName == input.NavName) != null)
                {
                    throw new UserFriendlyException(LocalizationConst.IDCodeRepeat);
                }

                var navgation = input.MapTo<Navgation>();
                navgation.CreateTime=DateTime.Now;
                navgation.CreateIP = IPHelper.GetIPAddress;
                if (_navgationService.Insert(navgation) == null)
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
                        SourceID = navgation.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.CreateUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateNavgationInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (_navgationService.Get(a => a.NavName == input.NavName&&a.ID!=input.ID) != null)
                {
                    throw new UserFriendlyException(LocalizationConst.IDCodeRepeat);
                }

                var navgation = _navgationService.Get(input.ID);
                if (navgation == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                navgation = input.MapTo(navgation);
                navgation.ModifyTime = DateTime.Now;
                navgation.ModifyIP = IPHelper.GetIPAddress;

                if (!_navgationService.Update(navgation))
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
                DisposeUserFriendlyException(e, ref json, "api/navgation/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteNavgationInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);

                if (_navgationService.GetList(a => idInts.Contains(a.ParentId)).Any())
                {
                    throw new UserFriendlyException("存在下级不能删除");
                }

                if (!_navgationService.Delete(a => idInts.Contains(a.ID)))
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
                            LogUserID = input.UserID,
                            LogTime = DateTime.Now,
                            LogIPAddress = IPHelper.GetIPAddress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/navgation/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }
    }
}

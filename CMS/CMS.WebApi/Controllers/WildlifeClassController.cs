using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.Base;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Application.WildlifeCategories;
using CMS.Application.WildlifeCategories.Dto;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/wildlifeclass")]
    public class WildlifeClassController : BaseApiController
    {
        private readonly IWildlifeCategoryService _wildlifeCategoryService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private string _moduleName = "动植物分类";

        public WildlifeClassController(IWildlifeCategoryService wildlifeCategoryService, ILogService logService,
            ISystemConfigurationService systemConfigurationService)
        {
            _wildlifeCategoryService = wildlifeCategoryService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
        }

        #region 前台
        [HttpGet]
        [Route("classlist")]
        public ResponseInfoModel ClassList([FromUri]GetObjFontListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = input.Keywords ?? "".Trim();
                int pageSize = input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                var outputList =
                    _wildlifeCategoryService.GetPageList(limit, offset, out total,
                        a => string.IsNullOrEmpty(keywords) || a.CateName.Contains(keywords),
                        true, a => a.OrderID).Select(s => new
                    {
                        s.ID,
                        s.CateName,
                        Type = ((WildlifeManagerTypesEnum) s.Type).ToString(),
                        s.OrderID
                    }).ToList();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/classlist", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetWildLifeCategoryListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                string keywords = input.Keywords ?? "".Trim();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.Pageindex - 1);
                int total;
                var outputList = _wildlifeCategoryService.GetPageList(limit, offset, out total,a=>string.IsNullOrEmpty(keywords) ||a.CateName.Contains(keywords),
                    true,a=>a.OrderID).ToList().MapTo<List<GetWildLifeCategoryListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public ResponseInfoModel Add([FromBody]CreateWildLifeCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                var wildlifeCategory = input.MapTo<WildlifeCategory>();
                wildlifeCategory.CreateIP = IPHelper.GetIPAddress;
                wildlifeCategory.CreatTime=DateTime.Now;

                var entity = _wildlifeCategoryService.Insert(wildlifeCategory);
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
                        LogUserID = input.CreatUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/add", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public ResponseInfoModel Edit([FromBody]UpdateWildLifeCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var wildlifeCategory = _wildlifeCategoryService.Get(input.ID);
                if (wildlifeCategory==null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }
                wildlifeCategory = input.MapTo(wildlifeCategory);

                wildlifeCategory.EditTime=DateTime.Now;
                wildlifeCategory.EditIP = IPHelper.GetIPAddress;

                if (!_wildlifeCategoryService.Update(wildlifeCategory))
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
                        LogUserID = input.EditUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/edit", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteWildlifeManagementInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.IDs);


                if (!_wildlifeCategoryService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getinfo")]
        public ResponseInfoModel GetInfo(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var wildlifeManager = _wildlifeCategoryService.Get(id);
                var output = wildlifeManager.MapTo<GetWildLifeCategoryOutput>();
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("selectlist")]
        public ResponseInfoModel SelectList(int type)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputlist = _wildlifeCategoryService.GetNoTrackingList(a => a.Type == type).OrderBy(a=>a.OrderID).Select(s=>new {ID=s.ID, CateName=s.CateName} ).ToList();
                json.Result = outputlist;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/wildlifeclass/selectlist", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

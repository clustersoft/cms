using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Application.ArticleArticleCategories;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.ArticleCategories;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/category")]
    public class ArticleCategoryController : BaseApiController
    {
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IArticleAttachService _articleAttachService;
        private readonly IArticleArticleCategoryService _articleArticleCategoryService;
        private string _moduleName = "文章栏目";

        public ArticleCategoryController(IArticleCategoryService articleCategoryService, ILogService logService,
            ISystemConfigurationService systemConfigurationService,IArticleAttachService articleAttachService,
            IArticleArticleCategoryService articleArticleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleAttachService = articleAttachService;
            _articleArticleCategoryService = articleArticleCategoryService;
        }

        #region 前台  
        [HttpGet]
        [Route("Frontlist")]
        public ResponseInfoModel Frontlist(int ParentID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object()};
            try
            {
                var outputList = _articleCategoryService.FrontlistByParent(ParentID);
                json.Result =new ListInfo() {List = outputList }; ;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/Frontlist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("listbyrefno")]
        public ResponseInfoModel ListByRefNo(string RefNo)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var parent = _articleCategoryService.Get(a => a.RefNo == RefNo&&a.State==0);

                if (parent == null)
                {
                   json.Result=new ListInfo() { List = new object[] { } };
                    return json;
                }
                else
                {
                    int cateID = parent.ID;
                    var outputList = _articleCategoryService.FrontlistByParent(cateID);
                    json.Result = new ListInfo() { List = outputList };
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/listbyrefno", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("getcateinfo")]
        public ResponseInfoModel GetCateInfo(string RefNo)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _articleCategoryService.FrontlistByRefNo(RefNo);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/getcateinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("getcateinfobyid")]
        public ResponseInfoModel Getcateinfobyid(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _articleCategoryService.FrontlistById(id);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/getcateinfobyid", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("getcrumbs")]
        public ResponseInfoModel GetCrumbs(int id)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _articleCategoryService.GetCrumbs(id);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/getcrumbs", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion

        [Authorize]
        [HttpGet]
        [Route("treelist")]
        public ResponseInfoModel Treelist()
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                var categoryList = _articleCategoryService.GetNoTrackingList(a => true).OrderBy(a => a.OrderID).ToList();
                var outputList = categoryList.MapTo<List<GetCategoryTreeListOutput>>();

                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/treelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetCategoryListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total;
                List<ArticleCategory> categoryList = _articleCategoryService.GetPageList(limit, offset, out total,a => a.ParentID == input.ParentID
                &&(string.IsNullOrEmpty((input.Keywords ?? "").Trim()) ||a.Name.Contains((input.Keywords ?? "").Trim())),true,a=>a.OrderID).ToList();
                var outputList = categoryList.MapTo<List<GetCategoryListOutput>>();
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize),pageSize = pageSize, list = outputList }; ;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("getInfo")]
        public ResponseInfoModel GetInfo(int ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                var category = _articleCategoryService.Get(a => a.ID == ID);
                var output = category.MapTo<GetCategoryOutput>();

                if (output != null)
                {
                    var parent = _articleCategoryService.Get(a => a.ID == output.ParentID);
                    output.ParentName = parent == null ? "" : parent.Name;

                    var attach = _articleAttachService.Get(a => a.ArticleGuid == category.Guid);
                    output.Attach = attach.MapTo<GetObjAttachOutput>();
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/getInfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                var category = _articleCategoryService.Addinfo(input);
                if (category == null)
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.InsertFail;
                }
                else
                {
                    _logService.Insert(new Log()
                    {
                        ActionContent = LocalizationConst.Delete,
                        SourceType = _moduleName,
                        SourceID = category.ID,
                        LogUserID = category.CreateUser,
                        LogTime = DateTime.Now,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() {Success = 1, Result = new object()};
            try
            {
                if (!_articleCategoryService.Editinfo(input))
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
                DisposeUserFriendlyException(e, ref json, "api/category/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteArticleCategoryInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                int[] idInts = ConvertStringToIntArr(input.ids);

                //if (_articleCategoryService.GetList(a=>idInts.Contains(a.ParentID)).Any())
                //{
                //    throw new UserFriendlyException("栏目下有文章不可删除");
                //}

                if (_articleArticleCategoryService.GetNoTrackingList(a=>idInts.Contains(a.ArticleCategorysID)).Any())
                {
                    throw new UserFriendlyException("栏目下有文章不可删除");
                }

                if (!_articleCategoryService.Delete(a => idInts.Contains(a.ID)))
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
                DisposeUserFriendlyException(e, ref json, "api/category/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("valid")]
        public ResponseInfoModel Valid(string Name,int? ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var category = _articleCategoryService.Get(a => a.Name == Name&&(!ID.HasValue||a.ID!=ID));
                if (category != null)
                {
                    json.Success = 0;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/valid", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("novalid")]
        public ResponseInfoModel Novalid(string RefNo, int? ID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var navgation = _articleCategoryService.Get(a => a.RefNo == RefNo&&(!ID.HasValue||a.ID!=ID));
                if (navgation != null)
                {
                    json.Success = 0;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/novalid", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("changeOrderID")]
        public ResponseInfoModel ChangeOrderID([FromBody]ChangeCategoryOrderIDInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_articleCategoryService.ChangeOrderID(input))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.UpdateFail;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/changeOrderID", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("changeParentID")]
        public ResponseInfoModel ChangeParentID([FromBody]ChangeCategoryParentIDInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var category = _articleCategoryService.Get(input.ID);
                if (category== null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }

                category.ParentID = input.ParentID;

                var parentList = _articleCategoryService.GetList(a => a.ParentID == input.ParentID).Select(a=>a.OrderID);
                int OrderID = parentList.Any() ? parentList.Max() + 1 : 1;
                category.OrderID = OrderID;

                if (!_articleCategoryService.Update(category))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.UpdateFail;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/category/changeParentID", LocalizationConst.UpdateFail);
            }
            return json;
        }
    }
}

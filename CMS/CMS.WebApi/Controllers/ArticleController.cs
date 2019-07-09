using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.ArticleAuditLogs;
using CMS.Application.ArticleCategories;
using CMS.Application.ArticleContents;
using CMS.Application.Articles;
using CMS.Application.Articles.Dto;
using CMS.Application.Extension;
using CMS.Application.Logs;
using CMS.Application.SystemConfigurations;
using CMS.Application.VisitRecords.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/article")]
    public class ArticleController : BaseApiController
    {
        private readonly IArticleService _articleService;
        private readonly ILogService _logService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly IArticleAttachService _articleAttachService;
        private readonly IArticleContentService _articleContentService;
        private readonly IArticleAuditLogService _articleAuditLogService;
        private string _moduleName = "文章";

        public ArticleController(IArticleService articleService, ILogService logService, ISystemConfigurationService systemConfigurationService,
            IArticleCategoryService articleCategoryService, IArticleAttachService articleAttachService,
            IArticleContentService articleContentService, IArticleAuditLogService articleAuditLogService)
        {
            _articleService = articleService;
            _logService = logService;
            _systemConfigurationService = systemConfigurationService;
            _articleCategoryService = articleCategoryService;
            _articleAttachService = articleAttachService;
            _articleContentService = articleContentService;
            _articleAuditLogService = articleAuditLogService;
        }

        #region 前台
        [HttpGet]
        [Route("toplist")]
        public ResponseInfoModel Toplist([FromUri]GetArticleTopListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var articleCategoryIds = _articleCategoryService.ChildStateInts(input.CategoryID);
                var outputList = _articleService.GetTopList(input, articleCategoryIds);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/toplist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("slidelist")]
        public ResponseInfoModel TopSlideList([FromUri]GetArticleTopListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var articleCategoryIds = _articleCategoryService.ChildStateInts(input.CategoryID);
                var outputList = _articleService.GetTopSlideList(input.Top, articleCategoryIds);
                json.Result = new ListInfo() { List = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/slidelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("slidelistbyrefno")]
        public ResponseInfoModel TopSlideListByRefNo([FromUri]GetTopSlideListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                var cate = _articleCategoryService.Get(a => a.RefNo == input.RefNo);
                json.Result = new ListInfo() { };
                if (cate != null)
                {
                    var articleCategoryIds = _articleCategoryService.ChildStateInts(cate.ID);
                    var outputList = _articleService.GetTopSlideList(input.Top, articleCategoryIds);
                    json.Result = new ListInfo() { List = outputList };
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/slidelistbyrefno", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("pagelist")]
        public ResponseInfoModel Pagelist([FromUri]GetArticlePageListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                int pageSize = input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.Pageindex - 1);
                int total = 0;

                var articleCategoryIds = _articleCategoryService.ChildStateInts(input.CategoryID);

                var outputList = _articleService.GetPageList(limit, offset, out total, input,
                    articleCategoryIds, input.CategoryID);

                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/pagelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("toplistbyrefno")]
        public ResponseInfoModel TopListByRefno([FromUri]GetArticleTopRefNoInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var articleCategory = _articleCategoryService.Get(a => a.RefNo == input.RefNo);
                if (articleCategory == null)
                {
                    json.Result = new ListInfo() { List = new object[] { } };
                    return json;
                }
                else
                {
                    int cateId = articleCategory.ID;
                    var articleCategoryIds = _articleCategoryService.ChildStateInts(cateId);
                    var outputList = _articleService.GetTopRefnoList(input.Top, articleCategoryIds, cateId);
                    json.Result = new ListInfo() { List = outputList };
                }

            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/toplistbyrefno", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("toplistbyrefno2")]
        public ResponseInfoModel TopListByRefno2([FromUri]GetArticleTopRefNoInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var articleCategory = _articleCategoryService.Get(a => a.RefNo == input.RefNo);
                if (articleCategory == null)
                {
                    json.Result = new ListInfo() { List = new object[] { } };
                    return json;
                }
                else
                {
                    int cateId = articleCategory.ID;

                    int[] articleCategoryIds = { cateId };
                    var outputList = _articleService.GetTopRefnoList(input.Top, articleCategoryIds.ToArray(), cateId);
                    json.Result = new ListInfo() { List = outputList };
                }

            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/toplistbyrefno2", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("lookdetail")]
        public ResponseInfoModel Lookdetail([FromUri]CreateVisitRecordInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var output = _articleService.LookDetail(input.id, input.ClientScreen, input.VisitUrl, input.ReferUrl, input.CategoryId);
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/lookdetail", LocalizationConst.QueryFail);
            }
            return json;
        }

        [HttpGet]
        [Route("searcharticles")]
        public ResponseInfoModel SearCharticles([FromUri]GetArticleSerachListInPut input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = input.PageSize;
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total = 0;
                var outputList = _articleService.SerachList(limit, offset, out total, input.Keyword, input.SearchType);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/searcharticles", LocalizationConst.QueryFail);
            }
            return json;
        }
        #endregion


        [HttpGet]
        [Route("treelist")]
        public ResponseInfoModel Treelist(int userID)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var outputList = _articleCategoryService.GetTreeList(userID);
                json.Result = outputList;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/treelist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("list")]
        public ResponseInfoModel List([FromUri]GetArticleListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total = 0;
                var articleCategoryIds = _articleCategoryService.ChildInts(input.ArticleCategorysID);
                var outputList = _articleService.GetArticleList(limit, offset, out total, input, articleCategoryIds);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/list", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("shtotallist")]
        public ResponseInfoModel Shtotallist(int pageIndex, int LogUser)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (pageIndex - 1);
                int total = 0;
                var outputList = _articleService.GetShTotalList(limit, offset, out total, LogUser);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/shtotallist", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("userlist")]
        public ResponseInfoModel Userlist([FromUri]GetArticleUserListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                int pageSize = _systemConfigurationService.GetPageSize();
                int limit = pageSize;
                int offset = pageSize * (input.PageIndex - 1);
                int total = 0;
                var outputList = _articleService.GetUserList(limit, offset, out total, input);
                json.Result = new PagingInfo() { totalCount = total, pageCount = (int)Math.Ceiling((decimal)total / pageSize), pageSize = pageSize, list = outputList.MapTo<List<GetArticleListOutput>>() };
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/userlist", LocalizationConst.QueryFail);
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
                var article = _articleService.GetInclude(id);
                var output = article.MapTo<GetArticleOutput>();
                if (output != null)
                {
                    var content = _articleContentService.Get(a => a.ArticleID == id);
                    output.Content = content == null ? "" : content.ArticleContents;
                    output.PictureAttach = _articleAttachService.Get(a => a.ArticleGuid == output.Guid && a.ModuleType == 5).MapTo<GetObjAttachOutput>();
                    output.AttachLists = _articleAttachService.GetNoTrackingList(a => a.ArticleGuid == output.Guid && a.ModuleType == 1).ToList().MapTo<List<GetArticleAttachOutput>>();
                    output.ArticleAuditList = _articleAuditLogService.GetArticleAuditLogs(id);
                }
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/getinfo", LocalizationConst.QueryFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("addInfo")]
        public ResponseInfoModel AddInfo([FromBody]CreateArticleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();
                var output = _articleService.AddInfo(input);
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
                        LogUserID = input.CreateUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/addInfo", LocalizationConst.InsertFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editInfo")]
        public ResponseInfoModel EditInfo([FromBody]UpdateArticleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_articleService.EditInfo(input))
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
                DisposeUserFriendlyException(e, ref json, "api/article/editInfo", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("editInfopause")]
        public ResponseInfoModel EditInfoPause([FromBody]UpdateArticleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_articleService.EditInfoPause(input))
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
                DisposeUserFriendlyException(e, ref json, "api/article/editInfopause", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ResponseInfoModel Delete([FromBody]DeleteArticleInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                int[] idInts = ConvertStringToIntArr(input.ids);

                if (!_articleService.Delete(idInts))
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
                DisposeUserFriendlyException(e, ref json, "api/article/delete", LocalizationConst.DeleteFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("changeStickTop")]
        public ResponseInfoModel ChangeStickTop([FromBody]ChangeStickTopInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                var article = _articleService.Get(input.ID);
                if (article == null)
                {
                    throw new UserFriendlyException(LocalizationConst.NoExist);
                }

                article.IsStickTop = input.IsStickTop;

                if (!_articleService.Update(article))
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
                DisposeUserFriendlyException(e, ref json, "api/article/changeStickTop", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("ChangeStatus")]
        public ResponseInfoModel ChangeStatus([FromBody]ChangeArticleStatesInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_articleService.ChangeStates(input))
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
                DisposeUserFriendlyException(e, ref json, "api/article/ChangeStatus", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpPost]
        [Route("ChangeStatesList")]
        public ResponseInfoModel ChangeStatusList([FromBody]ChangeArticleStatesListInput input)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                CheckModelState();

                if (!_articleService.ChangeStatesList(input))
                {
                    json.Success = 0;
                    json.Result = LocalizationConst.UpdateFail;
                }
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/ChangeStatesList", LocalizationConst.UpdateFail);
            }
            return json;
        }

        [Authorize]
        [HttpGet]
        [Route("shlist")]
        public ResponseInfoModel shlist(int LogUser)
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                var output = _articleService.GetShlist(LogUser);

                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/article/shlist", LocalizationConst.QueryFail);
            }
            return json;
        }
    }
}

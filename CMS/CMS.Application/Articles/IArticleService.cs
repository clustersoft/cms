using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Articles.Dto;
using CMS.Model;

namespace CMS.Application.Articles
{
    public interface IArticleService:IBaseService<Article>
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <param name="articleCategorieids"></param>
        /// <returns></returns>
        List<GetArticleListOutput> GetArticleList(int limit, int offset, out int total, GetArticleListInput input,
            int[] articleCategorieids);

        Article GetInclude(int id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Article AddInfo(CreateArticleInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool EditInfo(UpdateArticleInput input);

        /// <summary>
        /// 暂存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool EditInfoPause(UpdateArticleInput input);

        /// <summary>
        /// 发布统计列表
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        GetArticleFBListOutput GetFBList(DateTime? StartDate, DateTime? EndDate);

        /// <summary>
        /// 消息来源统计列表
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        GetArticleSoureListOutput GetSoureList(DateTime? StartDate, DateTime? EndDate);

        /// <summary>
        /// 获取用户创建文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        List<GetArticleListOutput> GetUserList(int limit, int offset, out int total, GetArticleUserListInput input);

        /// <summary>
        /// 当前用户文章审核列表
        /// </summary>
        /// <param name="LogUser"></param>
        /// <returns></returns>
        GetShlistOutput GetShlist(int LogUser);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool Delete(int[] ids);

        /// <summary>
        /// 文章审核列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="LogUser"></param>
        /// <returns></returns>
        List<GetShTotalList> GetShTotalList(int limit, int offset, out int total,int LogUser);

        /// <summary>
        /// 批量改变状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool ChangeStatesList(ChangeArticleStatesListInput input);

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool ChangeStates(ChangeArticleStatesInput input);

        /// <summary>
        /// 前台栏目前几条列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<GetArticleTopOutput> GetTopList(GetArticleTopListInput input, int[] ids);

        /// <summary>
        /// 前台文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<GetArticlePageListOutput> GetPageList(int limit, int offset, out int total, GetArticlePageListInput input,
            int[] ids,int cateId);

        /// <summary>
        /// 前台获取栏目前几条列表
        /// </summary>
        /// <param name="top"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<GetArticleTopRefOutput> GetTopRefnoList(int top, int[] ids,int cateId);

        /// <summary>
        /// 前台访问 加入VisitRecords日志的操作，访问量+1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ClientScreen"></param>
        /// <param name="VisitUrl"></param>
        /// <param name="ReferUrl"></param>
        /// <returns></returns>
        GetArticleFrontOutput LookDetail(int id, string ClientScreen, string VisitUrl, string ReferUrl,int? cateId);

        /// <summary>
        /// 前台搜索
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="Keyword"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        List<GetArticleSerachListOutput> SerachList(int limit, int offset, out int total, string Keyword, int SearchType);

        /// <summary>
        /// 获取幻灯片
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<GetTopSlideListOutput> GetTopSlideList(int Top, int[] ids);
    }
}

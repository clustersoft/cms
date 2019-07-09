using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.ArticleCategories.Dto;
using CMS.Model;

namespace CMS.Application.ArticleCategories
{
    public interface IArticleCategoryService:IBaseService<ArticleCategory>
    {
        /// <summary>
        /// 递归获取子栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<ArticleCategory> GetSonID(int parentId);

        /// <summary>
        /// 改变排序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool ChangeOrderID(ChangeCategoryOrderIDInput input);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ArticleCategory Addinfo(CreateCategoryInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Editinfo(UpdateCategoryInput input);

        /// <summary>
        /// 树形菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GetCategoryTreeListOutput> GetTreeList(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<GetCategoryListOutput> FrontlistByParent(int parentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RefNo"></param>
        /// <returns></returns>
        List<GetCategoryListOutput> FrontlistByRefNo(string RefNo);

        

        /// <summary>
        /// 递归获取子栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int[] ChildInts(int id);

        /// <summary>
        /// 递归获取子栏目（不包含本身）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int[] ChildIntsNoSelf(int id);

        List<GetCategoryListOutput> FrontlistById(int Id);

        /// <summary>
        /// 递归获取子栏目(过滤隐藏栏目)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int[] ChildStateInts(int id);

        int[] ParentInts(int id);

        int[] ParentIntsNoSelf(int id);

        List<GetCategoryCrumbOutput> GetCrumbs(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Navgations.Dto;
using CMS.Application.Users.Dto;
using CMS.Model;

namespace CMS.Application.Users
{
    public interface IUserService:IBaseService<User>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        LoginOutput Login(LoginInput input);

        /// <summary>
        /// 改密码
        /// </summary>
        /// <param name="inpt"></param>
        /// <returns></returns>
        bool ChangePassWord(ChangePwdInput inpt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetInclude(int id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        User AddInfo(User user, int[] roleIds);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        bool editInfo(UpdateUserInput input, int[] roleIds);

        /// <summary>
        /// 获取文章权限ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int[] GetPermCategoryIds(int id);

        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string[] GetPermNavCodes(int id);

        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="navCode"></param>
        /// <returns></returns>
        GetNavCodeOutput GetNavCode(int userID, string navCode);

        /// <summary>
        /// 获取文章权限码
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        GetCategoryActionOutput GetCategoryAction(int userID);
    }
}

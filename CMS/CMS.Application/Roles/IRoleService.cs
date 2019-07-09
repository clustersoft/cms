using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Roles.Dto;
using CMS.Model;

namespace CMS.Application.Roles
{
    public interface IRoleService:IBaseService<Role>
    {
        /// <summary>
        /// 获取文章栏目权限
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        string GetSelCategoryActions(int ID,int categoryID);

        /// <summary>
        /// 获取导航权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="navCode"></param>
        /// <returns></returns>
        string GetSelectNavActions(int id, string navCode);

        /// <summary>
        /// 新增角色和权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Role Addinfo(CreateRoleInput input);

        /// <summary>
        /// 编辑角色和权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool EditInfo(UpdateRoleInput input);
    }
}

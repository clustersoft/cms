using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;

namespace CMS.Application.WildlifeManagers
{
    public interface IWildlifeManagerService:IBaseService<WildlifeManagement>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        WildlifeManagement Add(CreateWildlifeManagementInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Edit(UpdateWildlifeManagementInput input);

        /// <summary>
        /// 获取动植物管理列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        List<GetWildlifeManagementListOutput> GetList(int limit, int offset, out int total
            , string keywords);

        /// <summary>
        /// 获取动植物管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GetWildlifeManagerInfoOutput GetWildlifeManagerInfo(int id);
    }
}

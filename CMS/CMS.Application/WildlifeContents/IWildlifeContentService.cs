using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.WildlifeContents.Dto;
using CMS.Model;

namespace CMS.Application.WildlifeContents
{
    public interface IWildlifeContentService:IBaseService<WildlifeContent>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        WildlifeContent Add(CreateWildlifeContentInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Edit(UpdateWildlifeContentInput input);

        /// <summary>
        /// 获取动植物管理详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<GetWildlifeContentInfoOutput> GetWildlifeContentInfo(int id);
    }
}

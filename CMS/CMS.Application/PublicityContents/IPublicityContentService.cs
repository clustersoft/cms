using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.PublicityContents.Dto;
using CMS.Model;

namespace CMS.Application.PublicityContents
{
    public interface IPublicityContentService : IBaseService<PublicityContent>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PublicityContent AddInfo(CreatePublicityContentInput input);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool EditInfo(UpdatePublicityContentInput input);

        /// <summary>
        /// 前台宣传展示列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<GetPublicityShowListOutput> GetShowList(GetPublicityShowListInput input);

        /// <summary>
        /// 宣传内容列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        List<GetPublicityContentListOutput> GetContentList(int limit, int offset, out int total,
            GetPublicityContentListInput input);
    }
}

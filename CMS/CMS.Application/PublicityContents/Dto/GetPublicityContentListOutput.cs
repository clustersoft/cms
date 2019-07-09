using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityContents.Dto
{
    public class GetPublicityContentListOutput
    {
        public int ID { get; set; }

        /// <summary>
        ///宣传内容名称
        /// </summary>
        public string PublicityName { get; set; }

        /// <summary>
        ///宣传栏目表的ID
        /// </summary>
        public int PublicityCategoryID { get; set; }

        public string PublicityCategoryName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string AttachUrl { get; set; }

        /// <summary>
        /// 链接类型：0直接链接，1间接链接（跳转）
        /// </summary>
        public int NavType { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string NavUrl { get; set; }


        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 发布时间类型（0:默认，1：自定义）
        /// </summary>
        public int PublishType { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 过期时间类型（0:默认，1：自定义）
        /// </summary>
        public int ExpiredType { get; set; }

        /// <summary>
        /// 是否显示（0：否，1：是）
        /// </summary>
        public int ShowType { get; set; }

        public int OrderID { get; set; }


        public string Remark { get; set; }
    }
}

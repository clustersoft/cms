using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 文章表
    /// </summary>
    [Table("Articles")]
    public class Article
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否直链
        /// </summary>
        public int LinkType { get; set; }

        /// <summary>
        /// 直链路径
        /// </summary>
        public string LinkPath { get; set; }

        /// <summary>
        /// 封面图片Guid
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容来源
        /// </summary>
        public string ContentSource { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsStickTop { get; set; }

        /// <summary>
        /// 对应附件表GUID
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 发布时间类型
        /// </summary>
        public int PubTimeType { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PubTime { get; set; }

        /// <summary>
        /// 到期时间类型
        /// </summary>
        public int ExpiredTimeType { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 允许浏览的IP段
        /// </summary>
        public string AgreeIPField { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewNums { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUser { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int? ModifyUser { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        [ForeignKey("ArticlesID")]
        public virtual ICollection<ArticleArticleCategory> ArticleArticleCategories { get; set; }
    }
}

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
    /// 文章栏目表
    /// </summary>
    [Table("ArticleCategory")]
    public class ArticleCategory
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 父级节点ID
        /// </summary>
        public int ParentID { get; set; }

        public string Guid { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 栏目标签
        /// </summary>
        public string RefNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 是否直连 0否，1是
        /// </summary>
        public int LinkType { get; set; }

        /// <summary>
        /// 直链路径
        /// </summary>
        public string LinkPath { get; set; }

        /// <summary>
        /// 添加文章权限(0：不能添加，1：可以添加）
        /// </summary>
        public int AddArticlePermissions { get; set; }

        /// <summary>
        /// 是否菜单栏目
        /// </summary>
        public int BeCategory { get; set; }

        /// <summary>
        /// 是否使用（0：不使用，1：使用）
        /// </summary>
        public int State { get; set; }

        public int? TemplateId { get; set; }

        public string TemplatePreview { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUser { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int? ModifyUser { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        [ForeignKey("ArticleCategorysID")]
        public virtual ICollection<ArticleArticleCategory> ArticleArticleCategories { get; set; }
    }
}

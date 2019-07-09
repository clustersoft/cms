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
    /// 文章附件表
    /// </summary>
    [Table("ArticleAttach")]
    public class ArticleAttach
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 文章表Guid
        /// </summary>
        public string ArticleGuid { get; set; }

        public string HashValue { get; set; }

        /// <summary>
        /// 附件存放路径
        /// </summary>
        public string AttachUrl { get; set; }

        /// <summary>
        /// 附件原始名称
        /// </summary>
        public string AttachName { get; set; }

        /// <summary>
        /// 附件保存名称
        /// </summary>
        public string AttachNewName { get; set; }

        /// <summary>
        ///附件排序
        /// </summary>
        public int AttachIndex { get; set; }

        /// <summary>
        /// 附件类型（0：非图片，1：图片）
        /// </summary>
        public int AttachType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附件后缀
        /// </summary>
        public string AttachFormat { get; set; }

        /// <summary>
        /// 附件大小
        /// </summary>
        public int AttachBytes { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int CreateUser { get; set; }

        public int? ModifyUser { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        //1文章 附件
        //2图片链接
        //3模板 封面图片
        //4文章分类 封面图片 
        //5文章 封面图片
        public int ModuleType { get; set; }
    }
}

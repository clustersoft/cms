using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [Table("SystemConfiguration")]
    public class SystemConfiguration
    {
        public int ID { get; set; }

        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageSizes { get; set; }

        /// <summary>
        /// 上传路径
        /// </summary>
        public string Uploadpath { get; set; }

        /// <summary>
        /// 上传图片格式
        /// </summary>
        public string ImgFormat { get; set; }

        /// <summary>
        /// 上传附件格式
        /// </summary>
        public string AttachFormat { get; set; }

        /// <summary>
        /// 图片最大大小
        /// </summary>
        public int MaxImgKB { get; set; }

        /// <summary>
        /// 附件最大大小
        /// </summary>
        public int MaxAttachKB { get; set; }

        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int MaxResolutionHeight { get; set; }

        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int MaxResolutionWidth { get; set; }

        /// <summary>
        /// 是否裁剪
        /// </summary>
        public int IsCutImg { get; set; }

        public string VideoFormat { get; set; }

        public string SpeakFormat { get; set; }

        public string Title { get; set; }
    }
}

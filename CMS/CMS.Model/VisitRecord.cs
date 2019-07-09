using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 访问记录表
    /// </summary>
    [Table("VisitRecords")]
    public class VisitRecord
    {
        public int ID { get; set; }

        /// <summary>
        /// 访问Url
        /// </summary>
        public string VisitUrl { get; set; }

        /// <summary>
        /// 来源URL
        /// </summary>
        public string ReferUrl { get; set; }

        /// <summary>
        /// 访问人IP
        /// </summary>
        public string VisitIp { get; set; }

        /// <summary>
        /// 访问人时间
        /// </summary>
        public DateTime VisitTime { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 内容ID
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// 客户端所在IP地址段
        /// </summary>
        public int ClientAreaId { get; set; }

        /// <summary>
        /// 客户端屏幕分辨率
        /// </summary>
        public string ClientScreen { get; set; }

        /// <summary>
        /// 客户端系统
        /// </summary>
        public string ClientSystem { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string ClientBrowser { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
    }
}

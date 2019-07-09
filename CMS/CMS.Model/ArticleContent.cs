using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 文章内容表
    /// </summary>
    [Table("ArticleContents")]
    public class ArticleContent
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        public string ArticleContents { get; set; }

        public virtual Article Article { get; set; }
    }
}

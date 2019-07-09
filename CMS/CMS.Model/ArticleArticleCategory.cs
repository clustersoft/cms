using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    [Table("Article_Category_Link")]
    public class ArticleArticleCategory
    {
        [Key]
        public int ID { get; set; }

        [Column("ArticlesID")]
        public int ArticlesID { get; set; }

        [Column("ArticleCategorysID")]
        public int ArticleCategorysID { get; set; }

        //public DateTime CreateTime { get; set; }

        //public int CreateUser { get; set; }

        //public DateTime? ModifyTime { get; set; }

        //public int? ModifyUser { get; set; }

        public virtual Article Article { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }
    }
}

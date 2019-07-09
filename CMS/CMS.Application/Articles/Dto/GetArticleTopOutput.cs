using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleTopOutput
    {
        public int ID { get; set; }

        public string Guid { get; set; }

        public string Title { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string CoverImage { get; set; }

        public string Summary { get; set; }

        public string ContentSource { get; set; }

        public int IsStickTop { get; set; }

        public DateTime PubTime { get; set; }

        public int OrderID { get; set; }

        public string AgreeIPField { get; set; }

        public string Content { get; set; }

        public int ViewNums { get; set; }

    }
}

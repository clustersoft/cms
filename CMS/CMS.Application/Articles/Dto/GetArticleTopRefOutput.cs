using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleTopRefOutput
    {
        public int ID { get; set; }

        public string Guid { get; set; }

        public string Title { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string CoverImage { get; set; }

        public string Summary { get; set; }

        public string ContentSource { get; set; }

        public string[] CategoryNames { get; set; }

        public int[] CategoryIDs { get; set; }

        public int IsStickTop { get; set; }

        public DateTime PubTime { get; set; }

        public int OrderID { get; set; }

        public string AgreeIPField { get; set; }

        public string Content { get; set; }

        public int ViewNums { get; set; }
    }
}

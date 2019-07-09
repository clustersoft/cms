using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleSerachListOutput
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string Status { get; set; }

        public string CreateUserName { get; set; }

        public DateTime PubTime { get; set; }

        public int IsStrickTop { get; set; }
    }
}

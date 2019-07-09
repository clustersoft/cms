using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetArticleFBListOutput
    {
        public List<GetArticleDataList> list { get; set; }

        public int fbSum { get; set; }

        public int shSum { get; set; }

        public int wshSum { get; set; }
    }

    public class GetArticleDataList
    {
        public int 审核 { get; set; }

        public int 未审核 { get; set; }

        public string 发布者 { get; set; }

        public int 发布数量 { get; set; }
    }
}

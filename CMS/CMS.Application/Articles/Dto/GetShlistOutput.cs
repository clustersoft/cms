using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetShlistOutput
    {
        public int TotalArticle { get; set; }

        public int Totalwsh { get; set; }

        public List<GetShListData> List { get; set; }
    }

    public class GetShListData
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string CategoryNames { get; set; }

        public string CategoryIDs { get; set; }

        public string Status { get; set; }

        public string CreateUserName { get; set; }

        public DateTime PubTime { get; set; }
    }
}

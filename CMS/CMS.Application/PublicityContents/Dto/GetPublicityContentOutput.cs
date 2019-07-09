using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.PublicityContents.Dto
{
    public class GetPublicityContentOutput
    {
        public int ID { get; set; }

        public string PublicityName { get; set; }

        public int PublicityCategoryID { get; set; }

        public int NavType { get; set; }

        public string NavUrl { get; set; }

        public DateTime PublishTime { get; set; }

        public int PublishType { get; set; }

        public DateTime ExpiredTime { get; set; }

        public int ExpiredType { get; set; }

        public int ShowType { get; set; }

        public int OrderID { get; set; }

        public string Remark { get; set; }

        public GetObjAttachOutput Attach { get; set; }
    }
}

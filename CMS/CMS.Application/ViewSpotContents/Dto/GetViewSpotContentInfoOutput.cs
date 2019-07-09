using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ViewSpotContents.Dto
{
    public class GetViewSpotContentInfoOutput
    {
        public int ID { get; set; }

        public int ViewSpotID { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }

        public List<GetObjAttachOutput> Attachs { get; set; }
    }
}

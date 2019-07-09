using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.WildlifeContents.Dto
{
    public class CreateWildlifeContentInput
    {
        public string Content { get; set; }

        public int Type { get; set; }

        public int WildlifemanagerID { get; set; }

        public int CreatUser { get; set; }

        public List<CreateObjAttachInput>  Attachs { get; set; }
    }
}

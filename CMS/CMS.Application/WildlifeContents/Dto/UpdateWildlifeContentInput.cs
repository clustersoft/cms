using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.WildlifeContents.Dto
{
    public class UpdateWildlifeContentInput
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public int Type { get; set; }

        public int WildlifemanagerID { get; set; }

        public int EditUser { get; set; }

        public List<UpdateArticleAttachInput>  Attachs { get; set; }
    }
}

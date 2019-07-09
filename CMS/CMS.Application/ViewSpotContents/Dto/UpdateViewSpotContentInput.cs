using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ViewSpotContents.Dto
{
    public class UpdateViewSpotContentInput
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ViewSpotID { get; set; }

        [Required]
        public int Type { get; set; }

        public string Content { get; set; }

        [Required]
        public int EditUser { get; set; }

        public List<UpdateArticleAttachInput>  Attachs { get; set; }
    }
}

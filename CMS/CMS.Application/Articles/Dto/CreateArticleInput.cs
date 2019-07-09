using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.Articles.Dto
{
    public class CreateArticleInput
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public string CoverImage { get; set; }

        public string Summary { get; set; }

        public string ContentSource { get; set; }

        [Required]
        public int IsStickTop { get; set; }

        [Required]
        public int PubTimeType { get; set; }

        public DateTime PubTime { get; set; }

        [Required]
        public DateTime ExpiredTime { get; set; }

        public int ExpiredTimeType { get; set; }

        public int OrderID { get; set; }

        public int Status { get; set; }

        public string AgreeIPField { get; set; }

        [Required]
        public int CreateUser { get; set; }

        [Required]
        public string CategoryIDs { get; set; }

        public string Content { get; set; }

        public List<CreateObjAttachInput> ArticleAttachs{ get; set; }

        public CreateObjAttachInput PictureAttach { get; set; }
    }
}

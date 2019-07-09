using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.Articles.Dto
{
    public class UpdateArticleInput
    {
        [Required]
        public int ID { get; set; }

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
        public int ExpiredTimeType { get; set; }

        public DateTime ExpiredTime { get; set; }

        public int OrderID { get; set; }

        public int Status { get; set; }

        public string AgreeIPField { get; set; }

        [Required]
        public int ModifyUser { get; set; }

        public string CategoryIDs { get; set; }

        public string Content { get; set; }

        public string ArticleAudit { get; set; }

        public List<UpdateArticleAttachInput> UpdateArticleAttachs{get;set;}

        public UpdateObjAttachInput PictureAttach { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ArticleCategories.Dto
{
    public class CreateCategoryInput
    {
        [Required]
        public int ParentID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RefNo { get; set; }

        public string Remark { get; set; }

        [Required]
        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        [Required]
        public int AddArticlePermissions { get; set; }

        public int BeCategory { get; set; }

        [Required]
        public int State { get; set; }

        public int? TemplateId { get; set; }

        public string TemplatePreview { get; set; }

        [Required]
        public int CreateUser { get; set; }

        public CreateObjAttachInput Attach { get; set; }

    }
}

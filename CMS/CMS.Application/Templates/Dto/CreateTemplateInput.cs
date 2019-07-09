using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.Templates.Dto
{
    public class CreateTemplateInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int UseAble { get; set; }

        public int OrderID { get; set; }

        [Required]
        public int CreateUser { get; set; }

        public string Path { get; set; }

        public string Image { get; set; }

        public CreateObjAttachInput Attach { get; set; }
    }
}

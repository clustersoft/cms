using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ViewSpotContents.Dto
{
    public class CreateViewSpotContentInput
    {
        [Required]
        public int ViewSpotID { get; set; }

        [Required]
        public int Type { get; set; }

        public string Content { get; set; }

        [Required]
        public int CreatUser { get; set; }

        public List<CreateObjAttachInput>  Attachs { get; set; }
    }
}

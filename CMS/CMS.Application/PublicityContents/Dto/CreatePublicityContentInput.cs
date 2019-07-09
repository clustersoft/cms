using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.PublicityContents.Dto
{
    public class CreatePublicityContentInput
    {
        [Required]
        public string PublicityName { get; set; }

        [Required]
        public int PublicityCategoryID { get; set; }

        [Required]
        public int NavType { get; set; }

        [Required]
        public string NavUrl { get; set; }

        [Required]
        public DateTime PublishTime { get; set; }

        [Required]
        public int PublishType { get; set; }

        [Required]
        public DateTime ExpiredTime { get; set; }

        [Required]
        public int ExpiredType { get; set; }

        [Required]
        public int ShowType { get; set; }

        [Required]
        public int OrderID { get; set; }

        public string Remark { get; set; }

        [Required]
        public int CreateUser { get; set; }

        //1图片 0文字
        public int PublicityTypesID { get; set; }

        public CreatePublicityContentAttachInput Attach { get; set; }
    }
}

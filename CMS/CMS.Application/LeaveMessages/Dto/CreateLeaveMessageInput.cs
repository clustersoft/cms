using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.LeaveMessages.Dto
{
    public class CreateLeaveMessageInput
    {
        [Required]
        public int LeaveType { get; set; }

        [Required]
        public string Contents { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        public CreateObjAttachInput Attach { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.LeaveMessages.Dto
{
    public class GetLeaveMessageListInput
    {
        [Required]
        public int PageIndex { get; set; }

        public string Keyword { get; set; }
    }
}

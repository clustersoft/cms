using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Logs.Dto
{
    public class GetLogInput
    {
        [Required]
        public int pageIndex { get; set; }

        public string Keyword { get; set; }

        [Required]
        public int LogUserID { get; set; }
    }
}

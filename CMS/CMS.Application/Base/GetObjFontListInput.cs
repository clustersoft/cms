using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Base
{
    public class GetObjFontListInput
    {
        [Required]
        public int PageIndex { get; set; }

        [Required]
        public int PageSize { get; set; }

        public string Keywords { get; set; }
    }
}

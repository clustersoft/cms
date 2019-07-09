using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Templates.Dto
{
    public class GetTemplateListInput
    {

        [Required]
        public int PageIndex { get; set; }

        public string Keyword { get; set; }
    }
}

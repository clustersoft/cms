using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.Templates.Dto
{
    public class GetTemplateOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public int OrderID { get; set; }

        public int Useable { get; set; }

        public GetTemplateAttachOutput Attach { get; set; }
    }
}

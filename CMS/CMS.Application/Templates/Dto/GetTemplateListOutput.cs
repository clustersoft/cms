using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Templates.Dto
{
    public class GetTemplateListOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Useable { get; set; }
    }
}

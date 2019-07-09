using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Articles.Dto
{
    public class GetTreeOutput
    {
        public string Name { get; set; }

        public List<GetTreeOutput> children { get; set;}
    }
}

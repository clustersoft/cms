using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Navgations.Dto
{
    public class GetNavCodeOutput
    {
        public string NavCode { get; set; }

        public string ActionCode { get; set; }

        public int IsAdmin { get; set; }
    }
}

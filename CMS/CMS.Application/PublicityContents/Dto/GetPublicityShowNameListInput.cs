using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityContents.Dto
{
    public class GetPublicityShowNameListInput
    {
        public int PublicityTypesID { get; set; }

        public string PublicityCateName { get; set; }

        public int Top { get; set; }
    }
}

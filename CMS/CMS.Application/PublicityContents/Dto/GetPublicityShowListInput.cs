using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityContents.Dto
{
    public class GetPublicityShowListInput
    {
        public int Top { get; set; }

        public int PublicityTypesID { get; set; }

        public int PublicityCateID { get; set; }
    }
}

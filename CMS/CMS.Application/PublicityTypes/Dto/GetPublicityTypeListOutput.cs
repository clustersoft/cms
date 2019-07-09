using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityTypes.Dto
{
    public class GetPublicityTypeListOutput
    {
        public int ID { get; set; }

        public string TypeName { get; set; }

        public string Remark { get; set; }
    }
}

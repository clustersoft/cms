using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.PublicityContents.Dto
{
    public class GetPublicityShowListOutput
    {
        public int ID { get; set; }

        public string PublicityName { get; set; }

        public string PublicityCategoryName { get; set; }

        public string AttachUrl { get; set; }

        public int NavType { get; set; }

        public string NavUrl { get; set; }

        public DateTime PublishTime { get; set; }

        public int OrderID { get; set; }

        public string Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ViewSpots.Dto
{
    public class GetViewListOutput
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Jd { get; set; }

        public string Wd { get; set; }

        public double Radius { get; set; }

        public string AttachUrl { get; set; }

        public int OrderID { get; set; }
    }
}

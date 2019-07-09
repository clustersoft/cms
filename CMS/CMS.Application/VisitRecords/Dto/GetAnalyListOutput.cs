using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.VisitRecords.Dto
{
    public class GetAnalyListOutput
    {
        public int VisitCount { get; set; }

        public int VisitIP { get; set; }

        public string VisitTime { get; set; }

        public string VisitDay { get; set; }
    }
}

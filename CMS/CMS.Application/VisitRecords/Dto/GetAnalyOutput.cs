using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.VisitRecords.Dto
{
    public class GetAnalyOutput
    {
        public int TodayCount { get; set; }

        public int TodayIP { get; set; }

        public int yestodayCount { get; set; }

        public int yestodayIP { get; set; }

        public int averageCount { get; set; }

        public int averageIP { get; set; }

        public int TotalCount { get; set; }

        public int TotalIP { get; set; }

        public int MonthCount { get; set; }

        public int MonthIP { get; set; }

        public string hapendayCount { get; set; }

        public string HapendayIP { get; set; }

        public int HighestCount { get; set; }

        public int HighestIP { get; set; }
    }
}

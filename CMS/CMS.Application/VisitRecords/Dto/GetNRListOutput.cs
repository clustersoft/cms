using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.VisitRecords.Dto
{
    public class GetNRListOutput
    {
        public string 内容名称 { get; set; }

        public int 访问量 { get; set; }

        public string 访问量百分比 { get; set; }

        public int IP数 { get; set; }

        public string IP数百分比 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Logs.Dto
{
    public class GetLogOutput
    {
        public int ID { get; set; }

        public string ActionContent { get; set; }

        public string LogUser { get; set; }

        public DateTime LogTime { get; set; }
    }
}

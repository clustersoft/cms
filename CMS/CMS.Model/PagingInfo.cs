using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class PagingInfo
    {
        public int totalCount { get; set; }

        public int pageCount { get; set; }

        public int pageSize { get; set; }

        public object list { get; set; }
    }
}

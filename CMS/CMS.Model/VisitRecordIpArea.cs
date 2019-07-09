using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 访问客户端IP位置分布
    /// </summary>
    [Table("VisitRecordIpAreas")]
    public class VisitRecordIpArea
    {
        public int ID { get; set; }

        public int IP_Start { get; set; }

        public int IP_End { get; set; }

        public int IP_Province { get; set; }

        public int IP_City { get; set; }
    }
}

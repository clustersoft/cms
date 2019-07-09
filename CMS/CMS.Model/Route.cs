using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    [Table("JQ_Route")]
    public class Route
    {
        public int ID { get; set; }

        public string RouteName { get; set; }

        public string Content { get; set; }

        public int? OrderID { get; set; }

        public int CreatUser { get; set; }

        public DateTime CreatTime { get; set; }

        public int? EditUser { get; set; }

        public DateTime? EditTime { get; set; }

        public string CreateIP { get; set; }

        public string EditIP { get; set; }

    }
}

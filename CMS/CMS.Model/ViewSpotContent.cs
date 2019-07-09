using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    [Table("JQ_ViewSpotContent")]
    public class ViewSpotContent
    {
        public int ID { get; set; }

        [Column("JQ_ViewSpotID")]
        public int ViewSpotID { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        public string FileID { get; set; }

        public int CreatUser { get; set; }

        public DateTime CreatTime { get; set; }

        public int? EditUser { get; set; }

        public DateTime? EditTime { get; set; }

        public string CreateIP { get; set; }

        public string EditIP { get; set; }
    }
}

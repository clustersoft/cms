using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.VisitRecords.Dto
{
    public class CreateVisitRecordInput
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string ClientScreen { get; set; }

        [Required]
        public string VisitUrl { get; set; }

        public int? CategoryId { get; set; }

        public string ReferUrl { get; set; }
    }
}

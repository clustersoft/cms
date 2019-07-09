using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.LeaveMessages.Dto
{
    public class GetLeaveMessageListOuput
    {
        public int ID { get; set; }

        public string LeaveType { get; set; }

        public string Contents { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public DateTime LeaveTime { get; set; }
    }
}

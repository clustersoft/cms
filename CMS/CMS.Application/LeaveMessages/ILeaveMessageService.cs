using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.LeaveMessages.Dto;
using CMS.Model;

namespace CMS.Application.LeaveMessages
{
    public  interface ILeaveMessageService:IBaseService<LeaveMessage>
    {
        LeaveMessage AddInfo(CreateLeaveMessageInput input);
    }
}

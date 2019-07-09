using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Extension;
using CMS.Application.LeaveMessages.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.LeaveMessages
{
    public class LeaveMessageService:BaseService<LeaveMessage>,ILeaveMessageService
    {
        public LeaveMessageService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }

        public LeaveMessage AddInfo(CreateLeaveMessageInput input)
        {
            string guid = Guid.NewGuid().ToString();

            var leaveMessage = input.MapTo<LeaveMessage>();
            leaveMessage.LeaveTime=DateTime.Now;
            leaveMessage.Guid = guid;

            db.LeaveMessages.Add(leaveMessage);

            if (input.Attach != null)
            {
                db.ArticleAttaches.Add(new ArticleAttach()
                {
                    HashValue = input.Attach.HashValue,
                    ArticleGuid = guid,
                    AttachName = input.Attach.AttachName,
                    AttachNewName = input.Attach.AttachNewName,
                    AttachUrl = input.Attach.AttachUrl,
                    AttachFormat = input.Attach.AttachFormat,
                    AttachIndex = 1,
                    AttachBytes = input.Attach.AttachBytes,
                    AttachType = input.Attach.AttachType,
                    CreateTime = DateTime.Now,
                    CreateUser = 0,
                    CreateIP = IPHelper.GetIPAddress,
                    ModuleType = (int)AttachTypesEnum.留言附件
                });
            }

            return db.SaveChanges() > 0 ? leaveMessage : null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Templates.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.Templates
{
    public class TemplateService : BaseService<Template>, ITemplateService
    {
        public TemplateService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Template Addinfo(CreateTemplateInput input)
        {
            string guid= Guid.NewGuid().ToString();
            var template = input.MapTo<Template>();
            template.CreateTime = DateTime.Now;
            template.CreateIP = IPHelper.GetIPAddress;
            template.Guid = guid;
            
            db.Templates.Add(template);

            var attach = input.Attach.MapTo<ArticleAttach>();
            if (attach != null)
            {
                attach.ModuleType = (int)AttachTypesEnum.模板图片;
                attach.ArticleGuid = guid;
                attach.CreateTime = DateTime.Now;
                attach.CreateIP = IPHelper.GetIPAddress;
                attach.CreateUser = input.CreateUser;
                db.ArticleAttaches.Add(attach);
            }
            return db.SaveChanges() > 0 ? template : null;
        }



        public bool Editinfo(UpdateTemplateInput input)
        {
            var template = db.Templates.Find(input.ID);
            if (template == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }
            template = input.MapTo(template);
            template.ModifyTime=DateTime.Now;
            template.ModifyIP = IPHelper.GetIPAddress;         

            if (!input.Attach.ID.HasValue|| input.Attach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == template.Guid&&a.ModuleType == (int)AttachTypesEnum.模板图片);
                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.Attach.ID.HasValue && input.Attach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.Attach.HashValue,
                        ArticleGuid = template.Guid,
                        AttachName = input.Attach.AttachName,
                        AttachNewName = input.Attach.AttachNewName,
                        AttachUrl = input.Attach.AttachUrl,
                        AttachFormat = input.Attach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.Attach.AttachBytes,
                        AttachType = input.Attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.模板图片,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }

            db.Entry(template).State = EntityState.Modified;

            return db.SaveChanges() > 0;
        }
    }
}

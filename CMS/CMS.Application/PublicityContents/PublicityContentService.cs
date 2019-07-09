using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.PublicityContents.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.PublicityContents
{
    public class PublicityContentService : BaseService<PublicityContent>,IPublicityContentService
    {
        public PublicityContentService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }

        public PublicityContent AddInfo(CreatePublicityContentInput input)
        {
            string guid = Guid.NewGuid().ToString();

            var content = new PublicityContent()
            {
                PublicityName = input.PublicityName.Trim(),
                PublicityCategoryID = input.PublicityCategoryID,
                NavType = input.NavType,
                NavUrl = input.NavUrl.Trim(),
                PublishTime = input.PublishTime,
                PublishType = input.PublishType,
                ExpiredTime = input.ExpiredTime,
                ExpiredType = input.ExpiredType,
                ShowType = input.ShowType,
                OrderID = input.OrderID,
                Remark = input.Remark,
                AttachGuid = guid,
                CreateUser = input.CreateUser,
                CreateTime = DateTime.Now,
                CreateIP = IPHelper.GetIPAddress
                
            };

            db.PublicityContents.Add(content);

            if (input.PublicityTypesID == 1)
            {
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
                        ModuleType = (int)AttachTypesEnum.宣传图片,
                        CreateTime = DateTime.Now,
                        CreateUser = input.CreateUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }
            return db.SaveChanges() > 0 ? content : null;
        }

        public bool EditInfo(UpdatePublicityContentInput input)
        {

            var content = db.PublicityContents.Find(input.ID);
            content.PublicityName = input.PublicityName.Trim();
            content.PublicityCategoryID = input.PublicityCategoryID;
            content.NavType = input.NavType;
            content.NavUrl = input.NavUrl.Trim();
            content.PublishTime = input.PublishTime;
            content.PublishType = input.PublishType;
            content.ExpiredTime = input.ExpiredTime;
            content.ExpiredType = input.ExpiredType;
            content.ShowType = input.ShowType;
            content.OrderID = input.OrderID;
            content.Remark = input.Remark;
            content.ModifyUser = input.ModifyUser;
            content.ModifyTime = DateTime.Now;
            content.ModifyIP = IPHelper.GetIPAddress;

            if (!input.Attach.ID.HasValue|| input.Attach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == content.AttachGuid&&a.ModuleType==(int)AttachTypesEnum.宣传图片);

                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.Attach.ID.HasValue&&input.Attach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.Attach.HashValue,
                        ArticleGuid = content.AttachGuid,
                        AttachName = input.Attach.AttachName,
                        AttachNewName = input.Attach.AttachNewName,
                        AttachUrl = input.Attach.AttachUrl,
                        AttachFormat = input.Attach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.Attach.AttachBytes,
                        AttachType = input.Attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.宣传图片,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }
            db.Entry(content).State=EntityState.Modified;

            return db.SaveChanges() > 0;
        }

        public List<GetPublicityContentListOutput> GetContentList(int limit, int offset, out int total,GetPublicityContentListInput input)
        {
            string keyword = (input.Keyword ?? "").Trim();
            var temp = from a in db.PublicityContents
                       join b in db.ArticleAttaches on a.AttachGuid equals b.ArticleGuid into t1
                       from tt1 in t1.DefaultIfEmpty()
                       join c in db.PublicityCategories on a.PublicityCategoryID equals c.ID
                       where (string.IsNullOrEmpty(keyword) ||a.PublicityName.Contains(keyword))&&
                             c.PublicityTypesID == input.PublicityTypesID &&
                             (input.PublicityCateID == 0 || a.PublicityCategoryID == input.PublicityCateID)
                       orderby a.OrderID, a.PublishTime descending
                       select new
                       {
                           a,
                           tt1.AttachUrl,
                           c.PublicityCategoryName
                       };

            total = temp.Count();
            var list = temp.Skip(offset).Take(limit).Select(s => new GetPublicityContentListOutput()
            {
                AttachUrl = s.AttachUrl ?? "",
                ID = s.a.ID,
                NavType = s.a.NavType,
                NavUrl = s.a.NavUrl ?? "",
                OrderID = s.a.OrderID,
                PublicityCategoryName = s.PublicityCategoryName ?? "",
                PublicityCategoryID = s.a.PublicityCategoryID,
                Remark = s.a.Remark ?? "",
                PublishTime = s.a.PublishTime,
                PublicityName = s.a.PublicityName ?? "",
                ExpiredTime = s.a.ExpiredTime,
                ExpiredType = s.a.ExpiredType,
                ShowType = s.a.ShowType,
                PublishType = s.a.PublishType
            }).AsNoTracking().ToList();
            return list;
        }

        public List<GetPublicityShowListOutput> GetShowList(GetPublicityShowListInput input)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];

            var temp = from a in db.PublicityContents
                    join b in db.ArticleAttaches on a.AttachGuid equals b.ArticleGuid into t1
                    from tt1 in t1.DefaultIfEmpty()
                    join c in db.PublicityCategories on a.PublicityCategoryID equals c.ID
                    where a.PublishTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now &&
                          c.PublicityTypesID == input.PublicityTypesID &&
                          (input.PublicityCateID == 0 || a.PublicityCategoryID == input.PublicityCateID)
                    orderby a.OrderID,a.PublishTime descending 
                    select new
                    {
                        a,
                        tt1.AttachUrl,
                        c.PublicityCategoryName
                    };
            if (input.Top != 0)
            {
                temp = temp.Skip(0).Take(input.Top);
            }

            var list = temp.Select(s => new GetPublicityShowListOutput()
            {
                AttachUrl =string.IsNullOrEmpty(s.AttachUrl)? "": publishUrl+ s.AttachUrl,
                ID = s.a.ID,
                NavType = s.a.NavType,
                NavUrl = s.a.NavUrl ?? "",
                OrderID = s.a.OrderID,
                PublicityCategoryName = s.PublicityCategoryName?? "",
                Remark = s.a.Remark??"",
                PublishTime = s.a.PublishTime,
                PublicityName = s.a.PublicityName ?? ""
            }).AsNoTracking().ToList();

            return list;
        }
    }
}

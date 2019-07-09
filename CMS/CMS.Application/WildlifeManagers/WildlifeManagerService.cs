using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Extension;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.WildlifeManagers
{
    public class WildlifeManagerService : BaseService<WildlifeManagement>, IWildlifeManagerService
    {
        public WildlifeManagerService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public WildlifeManagement Add(CreateWildlifeManagementInput input)
        {
            string guid = Guid.NewGuid().ToString();
            var wildlifeManagement = input.MapTo<WildlifeManagement>();
            wildlifeManagement.CreatTime = DateTime.Now;
            wildlifeManagement.CreateIP = IPHelper.GetIPAddress;
            wildlifeManagement.FileID = guid;

            db.WildlifeManagements.Add(wildlifeManagement);

            if (input.Attach != null && !string.IsNullOrEmpty(input.Attach.HashValue))
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
                    CreateUser = input.CreatUser,
                    CreateIP = IPHelper.GetIPAddress,
                    ModuleType = (int)AttachTypesEnum.动植物管理附件
                });
            }

            return db.SaveChanges() > 0 ? wildlifeManagement : null;
        }

        public bool Edit(UpdateWildlifeManagementInput input)
        {
            var wildlifeManagement = db.WildlifeManagements.Find(input.ID);
            wildlifeManagement = input.MapTo(wildlifeManagement);
            wildlifeManagement.EditTime = DateTime.Now;
            wildlifeManagement.EditIP = IPHelper.GetIPAddress;

            db.Entry<WildlifeManagement>(wildlifeManagement).State = EntityState.Modified;

            if (!input.Attach.ID.HasValue || input.Attach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ModuleType == (int)AttachTypesEnum.动植物管理附件 && a.ArticleGuid == wildlifeManagement.FileID);

                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.Attach.ID.HasValue && input.Attach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.Attach.HashValue,
                        ArticleGuid = wildlifeManagement.FileID,
                        AttachName = input.Attach.AttachName,
                        AttachNewName = input.Attach.AttachNewName,
                        AttachUrl = input.Attach.AttachUrl,
                        AttachFormat = input.Attach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.Attach.AttachBytes,
                        AttachType = input.Attach.AttachType,
                        CreateTime = DateTime.Now,
                        CreateUser = input.EditUser,
                        CreateIP = IPHelper.GetIPAddress,
                        ModuleType = (int)AttachTypesEnum.动植物管理附件
                    });
                }
            }
            return db.SaveChanges() > 0;
        }

        public List<GetWildlifeManagementListOutput> GetList(int limit, int offset, out int total, string keywords)
        {
            keywords = (keywords ?? "").Trim();
            var temp = from a in db.WildlifeManagements
                       join b in db.WildlifeCategories on a.WildlifeCategoryID equals b.ID into tt1
                       from tt in tt1.DefaultIfEmpty()
                       where string.IsNullOrEmpty(keywords) || a.Name.Contains(keywords)
                       select new
                       {
                           WildlifeManagement = a,
                           cateName = tt.CateName ?? ""
                       };

            total = temp.Count();

            var list = temp.OrderBy(x => x.WildlifeManagement.OrderID).Skip(offset).Take(limit).AsNoTracking().Select(s => new GetWildlifeManagementListOutput
            {
                ID = s.WildlifeManagement.ID,
                OrderID = s.WildlifeManagement.OrderID,
                Name = s.WildlifeManagement.Name,
                Type = ((WildlifeManagerTypesEnum)s.WildlifeManagement.Type).ToString(),
                WildlifeCategoryID = s.cateName
            }).ToList();

            return list;
        }

        public GetWildlifeManagerInfoOutput GetWildlifeManagerInfo(int id)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];
            var temp = (from a in db.WildlifeManagements
                join b in db.WildlifeCategories on a.WildlifeCategoryID equals b.ID into t1
                from tt in t1.DefaultIfEmpty()
                join c in db.ArticleAttaches on a.FileID equals c.ArticleGuid into t2
                from tt2 in t2.DefaultIfEmpty()
                where a.ID == id
                select new GetWildlifeManagerInfoOutput
                {
                    ID = a.ID,
                    AttachUrl =string.IsNullOrEmpty(tt2.AttachUrl)? "": publishUrl+ tt2.AttachUrl,
                    Introduce = a.Introduce,
                    JQ_WildlifeCategoryID = a.WildlifeCategoryID ?? 0,
                    WildlifeCategoryName = tt.CateName ?? "",
                    Name = a.Name,
                    OrderID = a.OrderID,
                    Type = ((WildlifeManagerTypesEnum) a.Type).ToString()
                }).FirstOrDefault();
            return temp;
        }
    }
}

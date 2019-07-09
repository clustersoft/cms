using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Extension;
using CMS.Application.ViewSpots.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.ViewSpots
{
    public class ViewSpotService : BaseService<ViewSpot>, IViewSpotService
    {
        public ViewSpotService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public ViewSpot AddViewSpot(CreateViewSpotInput input)
        {
            string guid = Guid.NewGuid().ToString();

            var ViewSpot = input.MapTo<ViewSpot>();
            ViewSpot.CreatTime = DateTime.Now;
            ViewSpot.CreateIP = IPHelper.GetIPAddress;
            ViewSpot.FileID = guid;
            db.ViewSpots.Add(ViewSpot);

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
                    ModuleType = (int)AttachTypesEnum.景点附件
                });
            }

            return db.SaveChanges() > 0 ? ViewSpot : null;
        }

        public bool EditViewSpot(UpdateViewSpotInput input)
        {
            var viewSpot = db.ViewSpots.Find(input.ID);

            if (viewSpot == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            viewSpot = input.MapTo(viewSpot);
            viewSpot.EditTime = DateTime.Now;
            viewSpot.EditIP = IPHelper.GetIPAddress;

            if (!input.Attach.ID.HasValue || input.Attach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == viewSpot.FileID && a.ModuleType == (int)AttachTypesEnum.景点附件);

                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.Attach.ID.HasValue && input.Attach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.Attach.HashValue,
                        ArticleGuid = viewSpot.FileID,
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
                        ModuleType = (int)AttachTypesEnum.景点附件
                    });
                }
            }

            db.Entry<ViewSpot>(viewSpot).State = EntityState.Modified;

            return db.SaveChanges() > 0;
        }

        public List<GetViewSpotListOutput> GetList(int limit, int offset, out int total, GetViewSpotListInput input)
        {
            string keywords = (input.Keywords ?? "").Trim();
            var temp = db.ViewSpots.Where(a => string.IsNullOrEmpty(keywords) || a.Name.Contains(keywords));

            total = temp.Count();

            var list = temp.OrderBy(a=>a.OrderID).ThenByDescending(a=>a.CreatTime).Skip(offset).Take(limit).Select(s => new GetViewSpotListOutput()
            {
                ID = s.ID,
                Name = s.Name ?? "",
                OrderID = s.OrderID
            }).AsNoTracking().ToList();

            return list;
        }

        public List<GetViewRouteListOutput> GetRouteList(int routeID)
        {
            int[] ids = (from a in db.ViewSpots
                join b in db.RouteViewSpots on a.ID equals b.ViewSpotID
                where b.RouteID == routeID
                select a.ID).ToArray();

            var list = db.ViewSpots.Where(a => !ids.Contains(a.ID)).OrderBy(a => a.OrderID).AsNoTracking().Select(s=>new GetViewRouteListOutput()
            {
                ID = s.ID,
                Name = s.Name,
                OrderID = s.OrderID
            }).ToList();

            return list;
        }

        public List<GetViewListOutput> GetViewList(int limit, int offset, out int total, string keyWords)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];
            var temp = from a in db.ViewSpots
                        join b in db.ArticleAttaches on a.FileID equals b.ArticleGuid into t1
                        from tt in t1.DefaultIfEmpty()
                        where string.IsNullOrEmpty(keyWords)||a.Name.Contains(keyWords)
                        select new GetViewListOutput
                        {
                            ID = a.ID,
                            OrderID = a.OrderID,
                            Name = a.Name,
                            Jd = a.Jd,
                            Wd = a.Wd,
                            Radius = a.Radius ?? 0,
                            AttachUrl = string.IsNullOrEmpty(tt.AttachUrl)?"": publishUrl+ tt.AttachUrl,
                        };

            total = temp.Count();

            var list = temp.OrderBy(a => a.OrderID).Skip(offset).Take(limit).AsNoTracking().ToList();

            return list;
        }
    }
}

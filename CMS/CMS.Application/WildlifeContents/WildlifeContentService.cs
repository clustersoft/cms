using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.Extension;
using CMS.Application.WildlifeContents.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.WildlifeContents
{
    public class WildlifeContentService:BaseService<WildlifeContent>,IWildlifeContentService
    {
        public WildlifeContentService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public WildlifeContent Add(CreateWildlifeContentInput input)
        {
            string guid = Guid.NewGuid().ToString();
            var wildlifeContent = input.MapTo<WildlifeContent>();
            wildlifeContent.CreateIP = IPHelper.GetIPAddress;
            wildlifeContent.CreatTime=DateTime.Now;
            wildlifeContent.FileID = guid;

            db.WildlifeContents.Add(wildlifeContent);

            int index = 1;

            if (input.Attachs != null)
            {
                foreach (var attach in input.Attachs)
                {
                    if (attach != null)
                    {
                        db.ArticleAttaches.Add(new ArticleAttach()
                        {
                            HashValue = attach.HashValue,
                            ArticleGuid = guid,
                            AttachName = attach.AttachName,
                            AttachNewName = attach.AttachNewName,
                            AttachUrl = attach.AttachUrl,
                            AttachFormat = attach.AttachFormat,
                            AttachIndex = index++,
                            AttachBytes = attach.AttachBytes,
                            AttachType = attach.AttachType,
                            CreateTime = DateTime.Now,
                            CreateUser = input.CreatUser,
                            CreateIP = IPHelper.GetIPAddress,
                            ModuleType = (int)AttachTypesEnum.动植物管理详细附件
                        });
                    }
                }
            }

            return db.SaveChanges() > 0 ? wildlifeContent : null;
        }

        public bool Edit(UpdateWildlifeContentInput input)
        {
            var wildlifeContent = db.WildlifeContents.Find(input.ID);
            if (wildlifeContent == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            wildlifeContent = input.MapTo(wildlifeContent);
            wildlifeContent.EditIP = IPHelper.GetIPAddress;
            wildlifeContent.EditTime=DateTime.Now;

            db.Entry<WildlifeContent>(wildlifeContent).State=EntityState.Modified;

            var attachs = db.ArticleAttaches.Where(a => a.ArticleGuid == wildlifeContent.FileID && a.ModuleType == (int)AttachTypesEnum.动植物管理详细附件);

            //为空全删除
            if (input.Attachs == null || !input.Attachs.Any())
            {
                if (attachs.Any())
                {
                    db.ArticleAttaches.RemoveRange(attachs);
                }
            }
            else
            {
                List<int> selectIds = input.Attachs.Select(a => a.ID).ToList();
                List<int> nowIDs = attachs.Select(a => a.ID).ToList();
                List<int> deleteIDs = nowIDs.Except(selectIds).ToList();
                if (deleteIDs.Any())
                {
                    var deleteList = db.ArticleAttaches.Where(a => deleteIDs.Contains(a.ID));
                    if (deleteList.Any())
                    {
                        db.ArticleAttaches.RemoveRange(deleteList);
                    }
                }

                var aticlemax =
                    db.ArticleAttaches.Where(
                            a => a.ArticleGuid == wildlifeContent.FileID && a.ModuleType == (int) AttachTypesEnum.动植物管理详细附件)
                        .Select(a => a.AttachIndex);

                int index = aticlemax.Any() ? aticlemax.Max() + 1 : 1;

                foreach (var attach in input.Attachs.Where(a => a.ID == 0))
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = attach.HashValue,
                        ArticleGuid = wildlifeContent.FileID,
                        AttachName = attach.AttachName,
                        AttachNewName = attach.AttachNewName,
                        AttachUrl = attach.AttachUrl,
                        AttachFormat = attach.AttachFormat,
                        AttachIndex = index++,
                        AttachBytes = attach.AttachBytes,
                        AttachType = attach.AttachType,
                        ModuleType = (int) AttachTypesEnum.动植物管理详细附件,
                        CreateTime = DateTime.Now,
                        CreateUser = input.EditUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }
            return db.SaveChanges() > 0;
        }

        public List<GetWildlifeContentInfoOutput>  GetWildlifeContentInfo(int id)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];
            var list = (from a in db.WildlifeContents
                        where a.WildlifeID==id
                select new GetWildlifeContentInfoOutput
                {
                    ID = a.ID,
                    Content = a.Content,
                    Type = a.Type,
                    TypeName = ((WildlifeContentTypesEnum) a.Type).ToString(),
                    Attachs =
                        db.ArticleAttaches.Where(s => s.ArticleGuid == a.FileID).Select(x => new GetObjAttachOutput()
                        {
                            AttachIndex = x.AttachIndex,
                            AttachName = x.AttachName,
                            AttachNewName = x.AttachNewName,
                            AttachUrl =string.IsNullOrEmpty(x.AttachUrl)?"": publishUrl+ x.AttachUrl,
                            ID = x.ID
                        }).ToList()
                }).AsNoTracking().ToList();
            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.Extension;
using CMS.Application.ViewSpotContents.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;

namespace CMS.Application.ViewSpotContents
{
    public class ViewSpotContentService:BaseService<ViewSpotContent>,IViewSpotContentService
    {
        public ViewSpotContentService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public ViewSpotContent Add(CreateViewSpotContentInput input)
        {
            string guid = Guid.NewGuid().ToString();

            var viewSpotContent = input.MapTo<ViewSpotContent>();
            viewSpotContent.CreatTime=DateTime.Now;
            viewSpotContent.CreateIP = IPHelper.GetIPAddress;
            viewSpotContent.FileID = guid;

            db.ViewSpotContents.Add(viewSpotContent);

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
                            ModuleType = (int)AttachTypesEnum.景点详情附件
                        });
                    }
                }
            }
            return db.SaveChanges() > 0 ? viewSpotContent : null;
        }

        public bool Edit(UpdateViewSpotContentInput input)
        {
            var viewSpotContent = db.ViewSpotContents.Find(input.ID);

            if (viewSpotContent == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            viewSpotContent = input.MapTo(viewSpotContent);
            viewSpotContent.EditTime=DateTime.Now;
            viewSpotContent.EditIP = IPHelper.GetIPAddress;

            var attachs = db.ArticleAttaches.Where(a => a.ArticleGuid == viewSpotContent.FileID && a.ModuleType == (int)AttachTypesEnum.景点详情附件);

            //为空全删除
            if (input.Attachs==null||!input.Attachs.Any())
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

                var aticlemax = db.ArticleAttaches.Where(a => a.ArticleGuid == viewSpotContent.FileID && a.ModuleType == (int)AttachTypesEnum.景点详情附件).Select(a => a.AttachIndex);

                int index = aticlemax.Any() ? aticlemax.Max() + 1 : 1;

                foreach (var attach in input.Attachs.Where(a => a.ID == 0))
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = attach.HashValue,
                        ArticleGuid = viewSpotContent.FileID,
                        AttachName = attach.AttachName,
                        AttachNewName = attach.AttachNewName,
                        AttachUrl = attach.AttachUrl,
                        AttachFormat = attach.AttachFormat,
                        AttachIndex = index++,
                        AttachBytes = attach.AttachBytes,
                        AttachType = attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.景点详情附件,
                        CreateTime = DateTime.Now,
                        CreateUser = input.EditUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }

            db.Entry<ViewSpotContent>(viewSpotContent).State=EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public List<GetViewSpotContentInfoOutput> GetInfo(int id)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];
            var list = (from a in db.ViewSpotContents
                        where a.ViewSpotID==id
                select new GetViewSpotContentInfoOutput
                {
                    ID = a.ID,
                    ViewSpotID = a.ViewSpotID,
                    Type = ((ViewSpotContentTypesEnum) a.Type).ToString(),
                    Content = a.Content,
                    Attachs =
                        db.ArticleAttaches.Where(s => s.ArticleGuid == a.FileID).Select(x => new GetObjAttachOutput
                        {
                            ID = x.ID,
                            AttachUrl =string.IsNullOrEmpty(x.AttachUrl)?"": publishUrl+ x.AttachUrl,
                            AttachIndex = x.AttachIndex,
                            AttachNewName = x.AttachNewName,
                            AttachName = x.AttachName
                        }).ToList()
                }).AsNoTracking().ToList();
            return list;
        }
    }
}

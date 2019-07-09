using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.Articles.Dto;
using CMS.Application.Extension;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;
using EntityFramework.Extensions;

namespace CMS.Application.Articles
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        public ArticleService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Article AddInfo(CreateArticleInput input)
        {
            string guid = Guid.NewGuid().ToString();

            var article = new Article()
            {
                Title = input.Title.Trim(),
                LinkType = input.LinkType,
                LinkPath = input.LinkPath,
                CoverImage = input.CoverImage,
                Summary = input.Summary,
                ContentSource = input.ContentSource,
                IsStickTop = input.IsStickTop,
                PubTime = input.PubTime,
                PubTimeType = input.PubTimeType,
                ExpiredTime = input.ExpiredTime,
                ExpiredTimeType = input.ExpiredTimeType,
                OrderID = input.OrderID,
                Status = input.Status,
                AgreeIPField = input.AgreeIPField,
                CreateTime = DateTime.Now,
                CreateIP = IPHelper.GetIPAddress,
                CreateUser = input.CreateUser,
                Guid = guid,
            };

            db.Articles.Add(article);

            int[] idInts = string.IsNullOrEmpty(input.CategoryIDs) ? new int[] { } : Array.ConvertAll<string, int>(input.CategoryIDs.Split(','), delegate (string s) { return int.Parse(s); });

            foreach (var idInt in idInts)
            {
                db.ArticleArticleCategories.Add(new ArticleArticleCategory()
                {
                    Article = article,
                    ArticleCategorysID = idInt
                });
            }

            //附件
            int index = 1;

            if (input.ArticleAttachs != null)
            {
                foreach (var attach in input.ArticleAttachs)
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
                            CreateUser = input.CreateUser,
                            CreateIP = IPHelper.GetIPAddress,
                            ModuleType = (int)AttachTypesEnum.文章附件
                        });
                    }
                }
            }


            if (input.PictureAttach != null)
            {
                db.ArticleAttaches.Add(new ArticleAttach()
                {
                    HashValue = input.PictureAttach.HashValue,
                    ArticleGuid = guid,
                    AttachName = input.PictureAttach.AttachName,
                    AttachNewName = input.PictureAttach.AttachNewName,
                    AttachUrl = input.PictureAttach.AttachUrl,
                    AttachFormat = input.PictureAttach.AttachFormat,
                    AttachIndex = 1,
                    AttachBytes = input.PictureAttach.AttachBytes,
                    AttachType = input.PictureAttach.AttachType,
                    CreateTime = DateTime.Now,
                    CreateUser = input.CreateUser,
                    CreateIP = IPHelper.GetIPAddress,
                    ModuleType = (int)AttachTypesEnum.文章图片
                });
            }

            db.ArticleContents.Add(new ArticleContent() { Article = article, ArticleContents = input.Content });

            //审核记录
            var ArticleAuditLog = new ArticleAuditLog()
            {
                Article = article,
                AuditIP = IPHelper.GetIPAddress,
                AuditReason = "",
                AuditStatus = input.Status,
                AuditTime = DateTime.Now,
                AuditUser = input.CreateUser
            };

            db.ArticleAuditLogs.Add(ArticleAuditLog);

            return db.SaveChanges() > 0 ? article : null;
        }

        public bool EditInfo(UpdateArticleInput input)
        {
            var article = db.Articles.Find(input.ID);

            if (article == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            var attachs = db.ArticleAttaches.Where(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章附件);

            var articleCategoryList = db.ArticleArticleCategories.Where(a => a.ArticlesID == input.ID);
            db.ArticleArticleCategories.RemoveRange(articleCategoryList);

            int[] idInts = string.IsNullOrEmpty(input.CategoryIDs) ? new int[] { } : Array.ConvertAll<string, int>(input.CategoryIDs.Split(','), delegate (string s) { return int.Parse(s); });

            foreach (var idInt in idInts)
            {
                db.ArticleArticleCategories.Add(new ArticleArticleCategory()
                {
                    Article = article,
                    ArticleCategorysID = idInt
                });
            }

            //为空全删除
            if (input.UpdateArticleAttachs == null || !input.UpdateArticleAttachs.Any())
            {
                if (attachs.Any())
                {
                    db.ArticleAttaches.RemoveRange(attachs);
                }
            }
            else
            {
                List<int> selectIds = input.UpdateArticleAttachs.Select(a => a.ID).ToList();
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

                var aticlemax = db.ArticleAttaches.Where(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章附件).Select(a => a.AttachIndex);

                int index = aticlemax.Any() ? aticlemax.Max() + 1 : 1;

                foreach (var attach in input.UpdateArticleAttachs.Where(a => a.ID == 0))
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = attach.HashValue,
                        ArticleGuid = article.Guid,
                        AttachName = attach.AttachName,
                        AttachNewName = attach.AttachNewName,
                        AttachUrl = attach.AttachUrl,
                        AttachFormat = attach.AttachFormat,
                        AttachIndex = index++,
                        AttachBytes = attach.AttachBytes,
                        AttachType = attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.文章附件,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }

            article.ModifyUser = input.ModifyUser;
            article.ModifyIP = IPHelper.GetIPAddress;
            article.ModifyTime = DateTime.Now;
            article.Title = input.Title.Trim();
            article.Summary = input.Summary;
            article.AgreeIPField = input.AgreeIPField;
            article.CoverImage = input.CoverImage;
            article.ContentSource = input.ContentSource;
            article.PubTime = input.PubTime;
            article.ExpiredTime = input.ExpiredTime;
            article.LinkPath = input.LinkPath;
            article.LinkType = input.LinkType;
            article.IsStickTop = input.IsStickTop;
            article.PubTimeType = input.PubTimeType;
            article.ExpiredTimeType = input.ExpiredTimeType;
            article.OrderID = input.OrderID;
            article.Status = input.Status;


            if (!input.PictureAttach.ID.HasValue || input.PictureAttach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章图片);
                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.PictureAttach.ID.HasValue && input.PictureAttach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.PictureAttach.HashValue,
                        ArticleGuid = article.Guid,
                        AttachName = input.PictureAttach.AttachName,
                        AttachNewName = input.PictureAttach.AttachNewName,
                        AttachUrl = input.PictureAttach.AttachUrl,
                        AttachFormat = input.PictureAttach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.PictureAttach.AttachBytes,
                        AttachType = input.PictureAttach.AttachType,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress,
                        ModuleType = (int)AttachTypesEnum.文章图片
                    });
                }
            }

            var content = db.ArticleContents.FirstOrDefault(a => a.ArticleID == input.ID);
            if (content != null)
            {
                content.ArticleContents = input.Content;
            }

            db.Entry<Article>(article).State = EntityState.Modified;

            //全部添加记录
            var ArticleAuditLog = new ArticleAuditLog()
            {
                ArticleID = input.ID,
                AuditIP = IPHelper.GetIPAddress,
                AuditReason = input.ArticleAudit,
                AuditStatus = input.Status,
                AuditTime = DateTime.Now,
                AuditUser = input.ModifyUser
            };

            db.ArticleAuditLogs.Add(ArticleAuditLog);

            return db.SaveChanges() > 0;
        }

        public bool EditInfoPause(UpdateArticleInput input)
        {
            var article = db.Articles.Find(input.ID);

            if (article == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            var attachs = db.ArticleAttaches.Where(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章附件);

            var articleCategoryList = db.ArticleArticleCategories.Where(a => a.ArticlesID == input.ID);
            db.ArticleArticleCategories.RemoveRange(articleCategoryList);

            int[] idInts = string.IsNullOrEmpty(input.CategoryIDs) ? new int[] { } : Array.ConvertAll<string, int>(input.CategoryIDs.Split(','), delegate (string s) { return int.Parse(s); });

            foreach (var idInt in idInts)
            {
                db.ArticleArticleCategories.Add(new ArticleArticleCategory()
                {
                    Article = article,
                    ArticleCategorysID = idInt
                });
            }

            //为空全删除
            if (input.UpdateArticleAttachs == null || !input.UpdateArticleAttachs.Any())
            {
                if (attachs.Any())
                {
                    db.ArticleAttaches.RemoveRange(attachs);
                }
            }
            else
            {
                List<int> selectIds = input.UpdateArticleAttachs.Select(a => a.ID).ToList();
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

                var aticlemax = db.ArticleAttaches.Where(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章附件).Select(a => a.AttachIndex);

                int index = aticlemax.Any() ? aticlemax.Max() + 1 : 1;

                foreach (var attach in input.UpdateArticleAttachs.Where(a => a.ID == 0))
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = attach.HashValue,
                        ArticleGuid = article.Guid,
                        AttachName = attach.AttachName,
                        AttachNewName = attach.AttachNewName,
                        AttachUrl = attach.AttachUrl,
                        AttachFormat = attach.AttachFormat,
                        AttachIndex = index++,
                        AttachBytes = attach.AttachBytes,
                        AttachType = attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.文章附件,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }

            article.ModifyUser = input.ModifyUser;
            article.ModifyIP = IPHelper.GetIPAddress;
            article.ModifyTime = DateTime.Now;
            article.Title = input.Title.Trim();
            article.Summary = input.Summary;
            article.AgreeIPField = input.AgreeIPField;
            article.CoverImage = input.CoverImage;
            article.ContentSource = input.ContentSource;
            article.PubTime = input.PubTime;
            article.ExpiredTime = input.ExpiredTime;
            article.LinkPath = input.LinkPath;
            article.LinkType = input.LinkType;
            article.IsStickTop = input.IsStickTop;
            article.PubTimeType = input.PubTimeType;
            article.ExpiredTimeType = input.ExpiredTimeType;
            article.OrderID = input.OrderID;
            article.Status = input.Status;


            if (!input.PictureAttach.ID.HasValue || input.PictureAttach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == article.Guid && a.ModuleType == (int)AttachTypesEnum.文章图片);
                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.PictureAttach.ID.HasValue && input.PictureAttach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.PictureAttach.HashValue,
                        ArticleGuid = article.Guid,
                        AttachName = input.PictureAttach.AttachName,
                        AttachNewName = input.PictureAttach.AttachNewName,
                        AttachUrl = input.PictureAttach.AttachUrl,
                        AttachFormat = input.PictureAttach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.PictureAttach.AttachBytes,
                        AttachType = input.PictureAttach.AttachType,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress,
                        ModuleType = (int)AttachTypesEnum.文章图片
                    });
                }
            }

            var content = db.ArticleContents.FirstOrDefault(a => a.ArticleID == input.ID);
            if (content != null)
            {
                content.ArticleContents = input.Content;
            }

            db.Entry<Article>(article).State = EntityState.Modified;

            //暂存记录
            var ArticleAuditLog = new ArticleAuditLog()
            {
                ArticleID = input.ID,
                AuditIP = IPHelper.GetIPAddress,
                AuditReason = input.ArticleAudit,
                AuditStatus = -1,
                AuditTime = DateTime.Now,
                AuditUser = input.ModifyUser
            };

            db.ArticleAuditLogs.Add(ArticleAuditLog);

            return db.SaveChanges() > 0;
        }

        public List<GetArticleListOutput> GetArticleList(int limit, int offset, out int total, GetArticleListInput input, int[] articleCategorieids)
        {
            string keywords = (input.Keywords ?? "").Trim();
            var user = db.Users.Find(input.UserID);

            if (user == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            //有审核权限栏目
            var arr = (from a in db.ArticleCategories
                       join b in db.RoleArticleCategoryActions on a.ID equals b.ArticleCategoryID into t1
                       from tt1 in t1.DefaultIfEmpty()
                       join c in db.UserRoles on tt1.RoleID equals c.RoleID into t2
                       from tt2 in t2.DefaultIfEmpty()
                       where tt1.ActionCode.ToLower() == "audit" && tt2.UserID == input.UserID && articleCategorieids.Contains(a.ID)
                       select a.ID).ToArray();

            var list = new List<GetArticleListOutput>() { };

            if (user.Grade >= 254)
            {
                var temp = from a in db.Articles
                           .Include(x => x.ArticleArticleCategories)
                           join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                           from tt in t1.DefaultIfEmpty()
                           join c in db.Users on a.CreateUser equals c.ID into t2
                           from tt2 in t2.DefaultIfEmpty()
                           where articleCategorieids.Contains(tt.ArticleCategorysID) && (!input.Status.HasValue || a.Status == input.Status.Value)
                                 && (!input.AddTimeStart.HasValue || a.PubTime >= input.AddTimeStart.Value) &&
                                 (!input.AddTimeEnd.HasValue || a.PubTime <= input.AddTimeEnd.Value)
                                 && (string.IsNullOrEmpty(keywords) || a.Title.Contains(keywords)) &&
                                 a.Status != (int)AuditStatusEnum.已删除
                           select new
                           {
                               article = a,
                               userName = tt2 == null ? "" : tt2.UserName
                           };

                total = temp.Distinct().Count();
                temp = temp.Distinct().OrderByDescending(a => a.article.IsStickTop).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking();

                list = temp.ToList().Select(a => new GetArticleListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    CreateUserName = a.userName ?? "",
                    Status = ((AuditStatusEnum)a.article.Status).ToString(),
                    CategoryIDs = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategorysID)),
                    CategoryNames = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name)),
                    IsStickTop = a.article.IsStickTop
                }).ToList();
            }
            else
            {
                var temp = (from a in db.Articles
                            .Include(x => x.ArticleArticleCategories)
                            join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                            from tt in t1.DefaultIfEmpty()
                            join c in db.Users on a.CreateUser equals c.ID into t2
                            from tt2 in t2.DefaultIfEmpty()
                            where articleCategorieids.Contains(tt.ArticleCategorysID) && (!input.Status.HasValue || a.Status == input.Status.Value)
                                  && (!input.AddTimeStart.HasValue || a.PubTime >= input.AddTimeStart.Value) &&
                                  (!input.AddTimeEnd.HasValue || a.PubTime <= input.AddTimeEnd.Value)
                                  && (string.IsNullOrEmpty(keywords) || a.Title.Contains(keywords)) &&
                                  a.Status != (int)AuditStatusEnum.已删除
                                  && (arr.Contains(tt.ArticleCategorysID) || a.CreateUser == input.UserID)
                            select new
                            {
                                article = a,
                                userName = tt2 == null ? "" : tt2.UserName
                            });

                total = temp.Distinct().Count();
                temp = temp.Distinct().OrderByDescending(a => a.article.IsStickTop).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking();

                list = temp.ToList().Select(a => new GetArticleListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    CreateUserName = a.userName ?? "",
                    Status = ((AuditStatusEnum)a.article.Status).ToString(),
                    CategoryIDs = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategorysID)),
                    CategoryNames = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name)),
                    IsStickTop = a.article.IsStickTop
                }).ToList();
            }

            return list;
        }

        public List<GetShTotalList> GetShTotalList(int limit, int offset, out int total, int LogUser)
        {
            int[] wsharr = { (int)AuditStatusEnum.审核中, (int)AuditStatusEnum.未审核, (int)AuditStatusEnum.被退回 };

            var user = db.Users.Find(LogUser);
            if (user == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }
            var cateArr = (from a in db.ArticleCategories
                           join b in db.RoleArticleCategoryActions on a.ID equals b.ArticleCategoryID
                           join c in db.UserRoles on b.RoleID equals c.RoleID
                           where c.UserID == LogUser && b.ActionCode == "audit"
                           select a.ID).ToArray();

            var temp = (from a in db.Articles
                        join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                        from tt in t1.DefaultIfEmpty()
                        join c in db.Users on a.CreateUser equals c.ID into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where wsharr.Contains(a.Status) && (user.Grade >= 254 || user.Type == 1 || cateArr.Contains(tt.ArticleCategorysID))
                        select new
                        {
                            article = a,
                            userName = tt2 == null ? "" : tt2.UserName
                        });

            total = temp.Count();
            temp = temp.OrderByDescending(a => a.article.IsStickTop).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking();

            var list = temp.ToList().Select(a => new GetShTotalList()
            {
                ID = a.article.ID,
                Title = a.article.Title ?? "",
                PubTime = a.article.PubTime,
                CreateUserName = a.userName ?? "",
                Status = ((AuditStatusEnum)a.article.Status).ToString(),
                CategoryIDs = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.ID)),
                CategoryNames = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name))
            }).ToList();
            return list;
        }

        public GetArticleFBListOutput GetFBList(DateTime? StartDate, DateTime? EndDate)
        {
            int sh = (int)AuditStatusEnum.审核通过;
            int[] wsharr = { (int)AuditStatusEnum.审核中, (int)AuditStatusEnum.未审核, (int)AuditStatusEnum.被退回 };
            int[] fbarr =
            {
                (int) AuditStatusEnum.审核通过, (int) AuditStatusEnum.审核中,
                (int) AuditStatusEnum.未审核,(int) AuditStatusEnum.被退回
            };
            var tempList = from a in db.Articles
                           join b in db.Users on a.CreateUser equals b.ID
                           where (!StartDate.HasValue || a.PubTime >= StartDate) && (!EndDate.HasValue || a.PubTime <= EndDate)
                           select new
                           {
                               a,
                               b.UserName
                           };
            var list = tempList.GroupBy(a => a.UserName).OrderByDescending(s => s.Where(a => fbarr.Contains(a.a.Status)).Select(a => a.a.ID).Count()).
            Select(s => new GetArticleDataList()
            {
                审核 = s.Where(x => x.a.Status == sh).Select(a => a.a.ID).Count(),
                未审核 = s.Where(x => wsharr.Contains(x.a.Status)).Select(a => a.a.ID).Count(),
                发布数量 = s.Where(a => fbarr.Contains(a.a.Status)).Select(a => a.a.ID).Count(),
                发布者 = s.Key.ToString(),
            }).AsNoTracking();

            var outputList = new GetArticleFBListOutput()
            {
                fbSum = tempList.Count(a => fbarr.Contains(a.a.Status)),
                shSum = tempList.Count(a => a.a.Status == sh),
                wshSum = tempList.Count(a => wsharr.Contains(a.a.Status)),
                list = list.ToList()
            };

            return outputList;
        }

        public Article GetInclude(int id)
        {
            var article = db.Articles.Where(a => a.ID == id).Include(a => a.ArticleArticleCategories).FirstOrDefault();
            return article;
        }

        public bool Delete(int[] ids)
        {
            var removeList = db.Articles.Where(a => ids.Contains(a.ID));
            if (removeList.Any())
            {
                db.Articles.RemoveRange(removeList);

                var removeAList = db.ArticleArticleCategories.Where(a => ids.Contains(a.ArticlesID));
                if (removeAList.Any())
                {
                    db.ArticleArticleCategories.RemoveRange(removeAList);
                }

                var removeCList = db.ArticleContents.Where(a => ids.Contains(a.ArticleID));
                if (removeCList.Any())
                {
                    db.ArticleContents.RemoveRange(removeCList);
                }
            }
            return db.SaveChanges() > 0;
        }

        public GetShlistOutput GetShlist(int LogUser)
        {
            int[] wsharr = { (int)AuditStatusEnum.审核中, (int)AuditStatusEnum.未审核, (int)AuditStatusEnum.被退回 };
            int TotalArticle;
            var user = db.Users.Find(LogUser);
            if (user == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            //有审核权限栏目
            var cateArr = (from a in db.ArticleCategories
                           join b in db.RoleArticleCategoryActions on a.ID equals b.ArticleCategoryID
                           join c in db.UserRoles on b.RoleID equals c.RoleID
                           where c.UserID == LogUser && b.ActionCode == "audit"
                           select a.ID
            ).ToArray();

            if (user.Grade >= 254)
            {
                TotalArticle = db.Articles.Count(a => a.Status != (int) AuditStatusEnum.已删除);
            }
            else
            {
                TotalArticle = (from a in db.Articles
                    join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t2
                    from tt2 in t2.DefaultIfEmpty()
                    where
                    a.Status != (int) AuditStatusEnum.已删除 &&(a.CreateUser == LogUser ||
                    cateArr.Contains(tt2.ArticleCategorysID))
                    select a.ID).Distinct().Count();
            }
                
            var tempList = from a in db.Articles
                            join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t2
                            from tt2 in t2.DefaultIfEmpty()
                            join c in db.Users on a.CreateUser equals c.ID into t1
                            from tt in t1.DefaultIfEmpty()
                            where wsharr.Contains(a.Status) && (user.Grade >= 254 || user.Type == 1 || cateArr.Contains(tt2.ArticleCategorysID))
                            select new
                            {
                                article = a,
                                userName = tt == null ? "" : tt.UserName
                            };
            int Totalwsh = tempList.Count();

            var output = new GetShlistOutput()
            {
                TotalArticle = TotalArticle,
                Totalwsh = Totalwsh,
                List = tempList.OrderByDescending(a => a.article.PubTime).Skip(0).Take(10).AsNoTracking().ToList().Select(a => new GetShListData()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    CreateUserName = a.userName ?? "",
                    Status = ((AuditStatusEnum)a.article.Status).ToString(),
                    CategoryIDs = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.ID)),
                    CategoryNames = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name))
                }).ToList()
            };
            return output;
        }

        public GetArticleSoureListOutput GetSoureList(DateTime? StartDate, DateTime? EndDate)
        {
            int sh = (int)AuditStatusEnum.审核通过;
            int[] wsharr = { (int)AuditStatusEnum.审核中, (int)AuditStatusEnum.未审核, (int)AuditStatusEnum.被退回 };
            int[] fbarr =
{
                (int) AuditStatusEnum.审核通过, (int) AuditStatusEnum.审核中,
                (int) AuditStatusEnum.未审核,(int) AuditStatusEnum.被退回
            };
            var tempList = from a in db.Articles
                           where (!StartDate.HasValue || a.PubTime >= StartDate) && (!EndDate.HasValue || a.PubTime <= EndDate) && !string.IsNullOrEmpty(a.ContentSource)
                           select a;

            var list = tempList.GroupBy(a => a.ContentSource).OrderByDescending(s => s.Where(x => fbarr.Contains(x.Status)).Select(a => a.ID).Count()).
            Select(s => new GetArticleSoureDataList()
            {
                审核 = s.Where(x => x.Status == sh).Select(a => a.ID).Count(),
                未审核 = s.Where(x => wsharr.Contains(x.Status)).Select(a => a.ID).Count(),
                发布数量 = s.Where(x => fbarr.Contains(x.Status)).Select(a => a.ID).Count(),
                消息来源 = s.Key.ToString(),

            }).AsNoTracking();

            var outputList = new GetArticleSoureListOutput()
            {
                fbSum = tempList.Count(a => fbarr.Contains(a.Status)),
                shSum = tempList.Count(a => a.Status == sh),
                wshSum = tempList.Count(a => wsharr.Contains(a.Status)),
                list = list.ToList()
            };
            return outputList;
        }

        public List<GetArticleListOutput> GetUserList(int limit, int offset, out int total, GetArticleUserListInput input)
        {
            string keywords = (input.Keywords ?? "").Trim();
            var temp = (from a in db.Articles
                        join c in db.Users on a.CreateUser equals c.ID into t1
                        from tt in t1.DefaultIfEmpty()
                        where (!input.Status.HasValue || a.Status == input.Status.Value) && (!input.AddTimeStart.HasValue || a.PubTime >= input.AddTimeStart.Value)
                        && (!input.AddTimeEnd.HasValue || a.PubTime >= input.AddTimeEnd.Value) && (!input.UserID.HasValue || a.CreateUser == input.UserID.Value)
                        && (string.IsNullOrEmpty(keywords) || a.Title.Contains(keywords))
                        select new
                        {
                            article = a,
                            userName = tt == null ? "" : tt.UserName
                        });

            total = temp.Count();
            temp = temp.Distinct().OrderBy(a => a.article.OrderID).Skip(offset).Take(limit).AsNoTracking();

            var list = temp.ToList().Select(a => new GetArticleListOutput()
            {
                ID = a.article.ID,
                Title = a.article.Title ?? "",
                PubTime = a.article.PubTime,
                CreateUserName = a.userName ?? "",
                Status = ((AuditStatusEnum)a.article.Status).ToString(),
                CategoryIDs = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.ID)),
                CategoryNames = string.Join(",", a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name))
            }).ToList();
            return list;
        }

        public bool ChangeStatesList(ChangeArticleStatesListInput input)
        {
            int[] idInts = string.IsNullOrEmpty(input.Ids) ? new int[] { } : Array.ConvertAll<string, int>(input.Ids.Split(','), delegate (string s) { return int.Parse(s); });
            var list = db.Articles.Where(a => idInts.Contains(a.ID));
            foreach (var article in list)
            {
                //已经是删除的物理删除
                if (article.Status == (int) AuditStatusEnum.已删除)
                {
                    db.Entry<Article>(article).State = EntityState.Deleted;

                    db.Logs.Add(new Log()
                    {
                        ActionContent = LocalizationConst.Delete,
                        SourceType = "文章",
                        SourceID = article.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.ModifyUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
                else
                {
                    article.Status = input.Status;
                    db.Entry<Article>(article).State = EntityState.Modified;

                    if (input.Status == (int)AuditStatusEnum.已删除)
                    {
                        foreach (var id in idInts)
                        {
                            var ArticleAuditLog = new ArticleAuditLog()
                            {
                                ArticleID = id,
                                AuditIP = IPHelper.GetIPAddress,
                                AuditReason = "",
                                AuditStatus = input.Status,
                                AuditTime = DateTime.Now,
                                AuditUser = input.ModifyUser
                            };

                            db.ArticleAuditLogs.Add(ArticleAuditLog);
                        }
                    }

                    db.Logs.Add(new Log()
                    {
                        ActionContent = LocalizationConst.Update,
                        SourceType = "文章",
                        SourceID = article.ID,
                        LogTime = DateTime.Now,
                        LogUserID = input.ModifyUser,
                        LogIPAddress = IPHelper.GetIPAddress,
                    });
                }
            }

            return db.SaveChanges() > 0;
        }

        public bool ChangeStates(ChangeArticleStatesInput input)
        {
            var article = db.Articles.Find(input.ID);
            if (article == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            article.Status = input.Status;

            db.Entry<Article>(article).State = EntityState.Modified;

            var ArticleAuditLog = new ArticleAuditLog()
            {
                ArticleID = input.ID,
                AuditIP = IPHelper.GetIPAddress,
                AuditReason = "",
                AuditStatus = input.Status,
                AuditTime = DateTime.Now,
                AuditUser = input.ModifyUser
            };

            db.ArticleAuditLogs.Add(ArticleAuditLog);

            return db.SaveChanges() > 0;
        }

        public List<GetArticleTopOutput> GetTopList(GetArticleTopListInput input, int[] ids)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];

            var artArr = (from a in db.Articles
                          join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                          from tt in t1.DefaultIfEmpty()
                          where
                          ids.Contains(tt.ArticleCategorysID) && a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now &&
                          a.Status == (int)AuditStatusEnum.审核通过
                          select a).Distinct();



            var temp = from a in artArr
                       join b in db.ArticleAttaches on new { guid = a.Guid, type = 5 } equals new { guid = b.ArticleGuid, type = b.ModuleType } into t1
                       from tt in t1.DefaultIfEmpty()
                       join c in db.ArticleContents on a.ID equals c.ArticleID into t2
                       from tt1 in t2.DefaultIfEmpty()
                       select new
                       {
                           article = a,
                           content = tt1.ArticleContents ?? "",
                           image = tt.AttachUrl ?? ""
                       };

            var list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(0).Take(input.Top).AsNoTracking().ToList().Select(a => new GetArticleTopOutput()
            {
                ID = a.article.ID,
                Title = a.article.Title ?? "",
                PubTime = a.article.PubTime,
                IsStickTop = a.article.IsStickTop,
                Content = a.content ?? "",
                AgreeIPField = a.article.AgreeIPField ?? "",
                ContentSource = a.article.ContentSource ?? "",
                CoverImage = string.IsNullOrEmpty(a.image)?"": publishUrl+ a.image,
                OrderID = a.article.OrderID,
                Guid = a.article.Guid,
                LinkType = a.article.LinkType,
                LinkPath = a.article.LinkPath ?? "",
                ViewNums = a.article.ViewNums,
                Summary = a.article.Summary ?? ""

            }).ToList();
            return list;
        }

        public List<GetTopSlideListOutput> GetTopSlideList(int Top, int[] ids)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];

            var artArr = (from a in db.Articles
                          join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                          from tt in t1.DefaultIfEmpty()
                          where
                          ids.Contains(tt.ArticleCategorysID) && a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now &&
                          a.Status == (int)AuditStatusEnum.审核通过
                          select a).Distinct();



            var temp = from a in artArr
                       join b in db.ArticleAttaches on new { guid = a.Guid, type = 5 } equals new { guid = b.ArticleGuid, type = b.ModuleType }
                       select new
                       {
                           article = a,
                           image = b.AttachUrl ?? ""
                       };

            var list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(0).Take(Top).AsNoTracking().ToList().Select(a => new GetTopSlideListOutput()
            {
                ID = a.article.ID,
                CoverImage = string.IsNullOrEmpty(a.image) ? "" : publishUrl + a.image

            }).ToList();
            return list;
        }



        public List<GetArticlePageListOutput> GetPageList(int limit, int offset, out int total, GetArticlePageListInput input, int[] ids,int cateId)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];

            string keyword = (input.Keyword ?? "").Trim();

            var artArr = (from a in db.Articles
                          join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                          from tt in t1.DefaultIfEmpty()
                          where
                          ids.Contains(tt.ArticleCategorysID) && a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now &&
                          a.Status == (int)AuditStatusEnum.审核通过
                          && (string.IsNullOrEmpty(keyword) || a.Title.Contains(keyword))
                          select a).Distinct();

            var temp = from a in artArr
                       join b in db.ArticleAttaches on new { guid = a.Guid, type = 5 } equals new { guid = b.ArticleGuid, type = b.ModuleType } into t1
                       from tt in t1.DefaultIfEmpty()
                       join c in db.ArticleContents on a.ID equals c.ArticleID into t2
                       from tt1 in t2.DefaultIfEmpty()
                       join d in db.Users on a.CreateUser equals d.ID into t3
                       from tt2 in t3.DefaultIfEmpty()
                       select new
                       {
                           article = a,
                           content = tt1.ArticleContents ?? "",
                           userName = tt2.UserName ?? "",
                           CoverImage=tt.AttachUrl
                       };

            total = temp.Count();

            var list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking().ToList()
                .Select(a => new GetArticlePageListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    IsStickTop = a.article.IsStickTop,
                    Status = ((AuditStatusEnum)a.article.Status).ToString(),
                    CreateUserName = a.userName ?? "",
                    LinkType = a.article.LinkType,
                    LinkPath = a.article.LinkPath ?? "",
                    Summary = a.article.Summary??"",
                    CoverImage=string.IsNullOrEmpty(a.CoverImage)?"": publishUrl+ a.CoverImage,
                    SelfCategoryIDs =  a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.ID).ToArray(),
                    SelfCategoryNames =  a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name).ToArray(),
                    ParentCategoryIDs = a.article.ArticleArticleCategories.Where(m=>m.ArticleCategory.ParentID != 0).Select(b => b.ArticleCategory.ParentID).ToArray(),
                    ParentCategoryNames =db.ArticleCategories.Where(x=>(from c in db.ArticleCategories
                                                                                        join n in db.ArticleArticleCategories on c.ID equals n.ArticleCategorysID
                                                                                        where n.ArticlesID == a.article.ID
                                                                                        select c.ParentID).Contains(x.ID)).Select(s=>s.Name).ToArray()
                }).ToList();

            return list;
        }

        public List<GetArticleTopRefOutput> GetTopRefnoList(int top, int[] ids,int cateId)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];

            var artArr = (from a in db.Articles
                          join b in db.ArticleArticleCategories on a.ID equals b.ArticlesID into t1
                          from tt in t1.DefaultIfEmpty()
                          where
                          ids.Contains(tt.ArticleCategorysID) && a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now &&
                          a.Status == (int)AuditStatusEnum.审核通过
                          select a).Distinct();

            var temp = from a in artArr
                       join b in db.ArticleAttaches on new {guid= a.Guid,type=5}  equals new {guid= b.ArticleGuid,type=b.ModuleType}  into t1
                       from tt in t1.DefaultIfEmpty()
                       join c in db.ArticleContents on a.ID equals c.ArticleID into t2
                       from tt1 in t2.DefaultIfEmpty()
                       select new
                       {
                           article = a,
                           content = tt1.ArticleContents ?? "",
                           image=tt.AttachUrl??""
                       };

            var list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(0).Take(top).AsNoTracking().ToList()
                .Select(a => new GetArticleTopRefOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    IsStickTop = a.article.IsStickTop,
                    Content = a.content ?? "",
                    AgreeIPField = a.article.AgreeIPField ?? "",
                    ContentSource = a.article.ContentSource ?? "",
                    CoverImage = string.IsNullOrEmpty(a.image) ? "" : publishUrl + a.image,
                    OrderID = a.article.OrderID,
                    Guid = a.article.Guid,
                    LinkType = a.article.LinkType,
                    LinkPath = a.article.LinkPath ?? "",
                    ViewNums = a.article.ViewNums,
                    Summary = a.article.Summary ?? "",
                    CategoryIDs =  a.article.ArticleArticleCategories.Select(b => b.ArticleCategorysID).ToArray(),
                    CategoryNames = a.article.ArticleArticleCategories.Select(b => b.ArticleCategory.Name).ToArray()
                }).ToList();

            return list;
        }

        public GetArticleFrontOutput LookDetail(int id, string ClientScreen, string VisitUrl, string ReferUrl,int? cateId)
        {
            string publishUrl = ConfigurationManager.AppSettings["PulishAddres"];
            int lcateId;

            var output = (from a in db.Articles
                          join b in db.ArticleContents on a.ID equals b.ArticleID into t1
                          from tt in t1.DefaultIfEmpty()
                          join c in db.ArticleAttaches on new { guid = a.Guid, type = 5 } equals new { guid = c.ArticleGuid, type = c.ModuleType } into t2
                          from tt2 in t2.DefaultIfEmpty()
                          where a.ID == id && a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now
                          select new GetArticleFrontOutput
                          {
                              ID = a.ID,
                              AgreeIPField = a.AgreeIPField ?? "",
                              Title = a.Title??"",
                              PubTime = a.PubTime,
                              AttachLists = db.ArticleAttaches.Where(x => x.ArticleGuid == a.Guid&&x.ModuleType==1).Select(s => new GetObjAttachOutput()
                              {
                                  AttachIndex = s.AttachIndex,
                                  AttachName = s.AttachName,
                                  AttachNewName = s.AttachNewName,
                                  AttachUrl = string.IsNullOrEmpty(s.AttachUrl) ? "" : publishUrl + s.AttachUrl,
                                  ID = s.ID
                              }).ToList(),
                              Content = tt.ArticleContents ?? "",
                              ContentSource = a.ContentSource ?? "",
                              CoverImage = string.IsNullOrEmpty(tt2.AttachUrl)?"": publishUrl+ tt2.AttachUrl,
                              Guid = a.Guid,
                              IsStickTop = a.IsStickTop,
                              LinkType = a.LinkType,
                              LinkPath = a.LinkPath ?? "",
                              ViewNums = a.ViewNums,
                              OrderID = a.OrderID,
                              Summary = a.Summary??""
                          }).FirstOrDefault();

            if (output != null)
            {
                //访问量
                var artcile = db.Articles.Find(id);
                artcile.ViewNums += 1;
                db.Entry(artcile).State = EntityState.Modified;

                if (cateId.HasValue)
                {
                    lcateId = cateId.Value;
                }
                else
                {
                     lcateId = db.ArticleArticleCategories.Where(a => a.ArticlesID == id)
         .Select(s => s.ArticleCategorysID)
         .FirstOrDefault();
                }
                var Category = db.ArticleCategories.Find(lcateId);
                output.ParentCategoryId = Category == null?0: Category.ParentID;
                var ParentCategory= db.ArticleCategories.Find(output.ParentCategoryId);
                output.ParentCategoryName= ParentCategory == null ? "" : ParentCategory.Name;
                output.ParentCategoryRefNo= ParentCategory == null ? "" : ParentCategory.RefNo;
                output.SelfCategoryId = lcateId;
                output.SelfCategoryName = Category == null ? "": Category.Name;
                output.SelfCategoryRefNo= Category == null ? "" : Category.RefNo;

                //访问记录
                var visitRecord = new VisitRecord()
                {
                    VisitIp = IPHelper.GetIPAddress,
                    VisitTime = DateTime.Now,
                    ArticleId = id,
                    CategoryId = lcateId,
                    ClientBrowser = IPHelper.GetBrowser(),
                    ClientSystem = IPHelper.GetOSVersion(),
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month,
                    Day = DateTime.Now.Day,
                    VisitUrl = VisitUrl,
                    ReferUrl = ReferUrl,
                    ClientScreen = ClientScreen
                };

                db.VisitRecords.Add(visitRecord);

                db.SaveChanges();
            }
            return output;
        }

        public List<GetArticleSerachListOutput> SerachList(int limit, int offset, out int total, string Keyword, int SearchType)
        {
            Keyword = (Keyword ?? "").Trim();
            List<GetArticleSerachListOutput> list = new List<GetArticleSerachListOutput>();

            total = 0;

            if (SearchType == 1 || SearchType == 0)//1只搜标题
            {
                var temp = from a in db.Articles
                           join b in db.Users on a.CreateUser equals b.ID into t1
                           from tt in t1.DefaultIfEmpty()
                           where a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now && a.Status == (int)AuditStatusEnum.审核通过 && (string.IsNullOrEmpty(Keyword) || a.Title.Contains(Keyword))
                           select new
                           {
                               article = a,
                               userName = tt.UserName ?? ""
                           };

                total = temp.Count();

                list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking().ToList()
                .Select(a => new GetArticleSerachListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    LinkType = a.article.LinkType,
                    LinkPath = a.article.LinkPath ?? "",
                    CreateUserName = a.userName ?? "",
                    IsStrickTop = a.article.IsStickTop,
                    Status = ((AuditStatusEnum)a.article.Status).ToString()
                }).ToList();
                return list;
            }
            else if (SearchType == 2)//2只搜内容
            {
                var temp = from a in db.Articles
                           join b in db.Users on a.CreateUser equals b.ID into t1
                           from tt in t1.DefaultIfEmpty()
                           join c in db.ArticleContents on a.ID equals c.ArticleID into t2
                           from tt2 in t2.DefaultIfEmpty()
                           where a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now && a.Status == (int)AuditStatusEnum.审核通过
                           && (string.IsNullOrEmpty(Keyword) || tt2.ArticleContents.Contains(Keyword))
                           select new
                           {
                               article = a,
                               userName = tt.UserName ?? ""
                           };

                total = temp.Count();

                list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking().ToList()
                .Select(a => new GetArticleSerachListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    LinkType = a.article.LinkType,
                    LinkPath = a.article.LinkPath ?? "",
                    CreateUserName = a.userName ?? "",
                    IsStrickTop = a.article.IsStickTop,
                    Status = ((AuditStatusEnum)a.article.Status).ToString()
                }).ToList();
                return list;
            }
            else if (SearchType == 3)//3标题和内容都搜
            {
                var temp = from a in db.Articles
                           join b in db.Users on a.CreateUser equals b.ID into t1
                           from tt in t1.DefaultIfEmpty()
                           join c in db.ArticleContents on a.ID equals c.ArticleID into t2
                           from tt2 in t2.DefaultIfEmpty()
                           where a.PubTime <= DateTime.Now && a.ExpiredTime >= DateTime.Now && a.Status == (int)AuditStatusEnum.审核通过
                           && (string.IsNullOrEmpty(Keyword) || a.Title.Contains(Keyword) || tt2.ArticleContents.Contains(Keyword))
                           select new
                           {
                               article = a,
                               userName = tt.UserName ?? ""
                           };

                total = temp.Count();

                list = temp.OrderByDescending(a => a.article.IsStickTop).ThenBy(a => a.article.OrderID).ThenByDescending(a => a.article.PubTime).Skip(offset).Take(limit).AsNoTracking().ToList()
                .Select(a => new GetArticleSerachListOutput()
                {
                    ID = a.article.ID,
                    Title = a.article.Title ?? "",
                    PubTime = a.article.PubTime,
                    LinkType = a.article.LinkType,
                    LinkPath = a.article.LinkPath ?? "",
                    CreateUserName = a.userName ?? "",
                    IsStrickTop = a.article.IsStickTop,
                    Status = ((AuditStatusEnum)a.article.Status).ToString()
                }).ToList();
                return list;
            }
            return list;
        }
    }
}

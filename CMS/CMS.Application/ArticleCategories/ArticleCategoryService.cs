using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Articles.Dto;
using CMS.Application.Extension;
using CMS.Application.Templates.Dto;
using CMS.Model;
using CMS.Model.Enum;
using CMS.Util;
using Dapper;

namespace CMS.Application.ArticleCategories
{
    public class ArticleCategoryService : BaseService<ArticleCategory>, IArticleCategoryService
    {
        public ArticleCategoryService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool ChangeOrderID(ChangeCategoryOrderIDInput input)
        {
            var oldCate = db.ArticleCategories.Find(input.OldID);
            var newCate = db.ArticleCategories.Find(input.NewID);

            if (oldCate != null)
            {
                oldCate.OrderID = input.NewOrderID;
                db.Entry(oldCate).State = EntityState.Modified;
            }

            if (newCate != null)
            {
                newCate.OrderID = input.OldOrderID;
                db.Entry(newCate).State = EntityState.Modified;
            }

            return db.SaveChanges() > 0;
        }

        public ArticleCategory Addinfo(CreateCategoryInput input)
        {
            var checkCategory = db.ArticleCategories.FirstOrDefault(a => a.Name == input.Name);
            if (checkCategory != null)
            {
                throw new UserFriendlyException("栏目名重复");
            }

            string guid = Guid.NewGuid().ToString();
            int[] orderIDs = db.ArticleCategories.Where(a => a.ParentID == input.ParentID).Select(a => a.OrderID).ToArray();
            int orderID = orderIDs.Any() ? orderIDs.Max() + 1 : 1;

            var category = input.MapTo<ArticleCategory>();
            category.CreateTime = DateTime.Now;
            category.CreateIP = IPHelper.GetIPAddress;
            category.Guid = guid;
            category.OrderID = orderID;

            db.ArticleCategories.Add(category);

            var attach = input.Attach.MapTo<ArticleAttach>();
            if (attach != null)
            {
                attach.ModuleType = (int)AttachTypesEnum.文章分类图片;
                attach.ArticleGuid = guid;
                attach.CreateTime = DateTime.Now;
                attach.CreateIP = IPHelper.GetIPAddress;
                attach.CreateUser = input.CreateUser;
                attach.AttachIndex = 1;
                db.ArticleAttaches.Add(attach);
            }
            return db.SaveChanges() > 0 ? category : null;
        }



        public bool Editinfo(UpdateCategoryInput input)
        {
            var checkCategory = db.ArticleCategories.FirstOrDefault(a => a.Name == input.Name && a.ID != input.ID);
            if (checkCategory != null)
            {
                throw new UserFriendlyException("栏目名重复");
            }

            var category = db.ArticleCategories.Find(input.ID);
            if (category == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }
            category = input.MapTo(category);
            category.ModifyTime = DateTime.Now;
            category.ModifyIP = IPHelper.GetIPAddress;

            if (!input.Attach.ID.HasValue || input.Attach.ID == 0)
            {
                var attach = db.ArticleAttaches.FirstOrDefault(a => a.ArticleGuid == category.Guid && a.ModuleType == (int)AttachTypesEnum.文章分类图片);
                if (attach != null)
                {
                    db.ArticleAttaches.Remove(attach);
                }

                if (input.Attach.ID.HasValue && input.Attach.ID == 0)
                {
                    db.ArticleAttaches.Add(new ArticleAttach()
                    {
                        HashValue = input.Attach.HashValue,
                        ArticleGuid = category.Guid,
                        AttachName = input.Attach.AttachName,
                        AttachNewName = input.Attach.AttachNewName,
                        AttachUrl = input.Attach.AttachUrl,
                        AttachFormat = input.Attach.AttachFormat,
                        AttachIndex = 1,
                        AttachBytes = input.Attach.AttachBytes,
                        AttachType = input.Attach.AttachType,
                        ModuleType = (int)AttachTypesEnum.文章分类图片,
                        CreateTime = DateTime.Now,
                        CreateUser = input.ModifyUser,
                        CreateIP = IPHelper.GetIPAddress
                    });
                }
            }
            db.Entry(category).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public IEnumerable<ArticleCategory> GetSonID(int parentId)
        {
            var query = from c in db.ArticleCategories
                        where c.ParentID == parentId
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
        }

        public IEnumerable<int> GetParentID(int parentId)
        {
            var query = from c in db.ArticleCategories
                        where c.ID == parentId
                        select c.ParentID;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetParentID(t)));
        }

        //public IEnumerable<int> GetSonCateID(int[] cateArr, int parentId)
        //{
        //    var query = from c in db.ArticleCategories
        //                where c.ParentID == parentId
        //                select c.ID;

        //    query = query.Where(a => cateArr.Contains(a));

        //    return query.ToList().Concat(query.ToList().SelectMany(t => GetSonCateID(cateArr, t)));
        //}

        public IEnumerable<int> GetSonCateID(int[] cateArr, int parentId)
        {
            var temp = ChildIntsNoSelf(0);
            var list = temp.Where(a => cateArr.Contains(a));
            return list;
        }

        public List<GetCategoryTreeListOutput> GetTreeList(int userId)
        {
            var user = db.Users.Find(userId);

            if (user == null)
            {
                throw new UserFriendlyException(LocalizationConst.NoExist);
            }

            var cateArr = (from b in db.RoleArticleCategoryActions
                           join c in db.UserRoles on b.RoleID equals c.RoleID
                           where c.UserID == userId
                           select b.ArticleCategoryID).Distinct().ToArray();

            var arr = GetSonCateID(cateArr, 0);

            var list = (from a in db.ArticleCategories
                        where user.Type == 1 || arr.Contains(a.ID)
                        orderby a.OrderID
                        select new GetCategoryTreeListOutput
                        {
                            ID = a.ID,
                            OrderID = a.OrderID,
                            Name = a.Name,
                            RefNo = a.RefNo,
                            AddArticlePermissions = a.AddArticlePermissions,
                            ParentID = a.ParentID
                        }).AsNoTracking().ToList();
            return list;
        }

        public List<GetCategoryListOutput> FrontlistByParent(int parentId)
        {
            var list = (from a in db.ArticleCategories
                        join b in db.Templates on new { tId = a.TemplateId ?? 0, tState = 0 } equals new { tId = b.ID, tState = b.UseAble } into t1
                        from tt in t1.DefaultIfEmpty()
                        join c in db.ArticleAttaches on a.Guid equals c.ArticleGuid into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where a.ParentID == parentId && a.State == 0
                        orderby a.OrderID
                        select new GetCategoryListOutput
                        {
                            ID = a.ID,
                            AddArticlePermissions = a.AddArticlePermissions,
                            LinkType = a.LinkType,
                            LinkPath = a.LinkType == 1 ? a.LinkPath : tt.Path ?? "",
                            ParentID = a.ParentID,
                            Name = a.Name ?? "",
                            RefNo = a.RefNo ?? "",
                            OrderID = a.OrderID,
                            ConvertImage = tt2.AttachUrl ?? ""
                        }).ToList();
            return list;
        }

        public List<GetCategoryListOutput> FrontlistByRefNo(string RefNo)
        {
            var list = (from a in db.ArticleCategories
                        join b in db.Templates on new { tId = a.TemplateId ?? 0, tState = 0 } equals new { tId = b.ID, tState = b.UseAble } into t1
                        from tt in t1.DefaultIfEmpty()
                        join c in db.ArticleAttaches on a.Guid equals c.ArticleGuid into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where a.RefNo == RefNo && a.State == 0
                        orderby a.OrderID
                        select new GetCategoryListOutput
                        {
                            ID = a.ID,
                            AddArticlePermissions = a.AddArticlePermissions,
                            LinkType = a.LinkType,
                            LinkPath = a.LinkType == 1 ? a.LinkPath : tt.Path ?? "",
                            ParentID = a.ParentID,
                            Name = a.Name ?? "",
                            RefNo = a.RefNo ?? "",
                            OrderID = a.OrderID,
                            ConvertImage = tt2.AttachUrl ?? ""
                        }).ToList();
            return list;
        }

        public List<GetCategoryListOutput> FrontlistById(int Id)
        {
            var list = (from a in db.ArticleCategories
                        join b in db.Templates on new { tId = a.TemplateId ?? 0, tState = 0 } equals new { tId = b.ID, tState = b.UseAble } into t1
                        from tt in t1.DefaultIfEmpty()
                        join c in db.ArticleAttaches on a.Guid equals c.ArticleGuid into t2
                        from tt2 in t2.DefaultIfEmpty()
                        where a.ID == Id && a.State == 0
                        orderby a.OrderID
                        select new GetCategoryListOutput
                        {
                            ID = a.ID,
                            AddArticlePermissions = a.AddArticlePermissions,
                            LinkType = a.LinkType,
                            LinkPath = a.LinkType == 1 ? a.LinkPath : tt.Path ?? "",
                            ParentID = a.ParentID,
                            Name = a.Name ?? "",
                            RefNo = a.RefNo ?? "",
                            OrderID = a.OrderID,
                            ConvertImage = tt2.AttachUrl ?? ""
                        }).ToList();
            return list;
        }

        public int[] ChildInts(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<int>(@"with my1 as(select * from ArticleCategory where ParentID = @id
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.id = ArticleCategory.ParentID
                )
                select id from my1 ", new { id = id }).ToList();
                ids.Add(id);
                return ids.ToArray();
            }
        }

        public int[] ChildIntsNoSelf(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<int>(@"with my1 as(select * from ArticleCategory where ParentID = @id
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.id = ArticleCategory.ParentID
                )
                select id from my1 ", new { id = id }).ToArray();
                return ids;
            }
        }

        public int[] ChildStateInts(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<int>(@"with my1 as(select * from ArticleCategory where ParentID = @id and state=0
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.id = ArticleCategory.ParentID and ArticleCategory.state=0
                )
                select id from my1 ", new { id = id }).ToList();
                ids.Add(id);
                return ids.ToArray();
            }
        }

        public int[] ParentInts(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<int>(@"with my1 as(select * from ArticleCategory where ID = @id
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.ParentID = ArticleCategory.id
                )
                select id,Name from my1 ", new { id = id }).ToList();
                return ids.ToArray();
            }
        }

        public List<GetCategoryCrumbOutput> GetCrumbs(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<GetCategoryCrumbOutput>(@"with my1 as(select * from ArticleCategory where ID = @id
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.ParentID = ArticleCategory.id
                )
                select id,Name,RefNo from my1 ", new { id = id }).Reverse().ToList();
                return ids;
            }
        }

        public int[] ParentIntsNoSelf(int id)
        {
            using (var sdb = new SqlConnection(conn))
            {
                var ids = sdb.Query<int>(@"with my1 as(select * from ArticleCategory where ID = @id
                 union all select ArticleCategory.* from my1, ArticleCategory where my1.ParentID = ArticleCategory.id
                )
                select id from my1 ", new { id = id }).ToList();
                return ids.ToArray();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Roles.Dto;
using CMS.Model;
using CMS.Util;
using EntityFramework.Extensions;

namespace CMS.Application.Roles
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Role Addinfo(CreateRoleInput input)
        {
            Role role = new Role() { RoleName = input.RoleName, Remark = input.Remark, CreateUser = input.CreateUser, CreateTime = DateTime.Now,OrderID = input.OrderID,CreateIP= IPHelper.GetIPAddress};
            db.Roles.Add(role);

            List<RoleNavAction> roleNavActions = new List<RoleNavAction>();
            if (!string.IsNullOrEmpty(input.Nav_SelActions))
            {
                string[] NavSelActionArr = input.Nav_SelActions.Split(',');
                foreach (var nav in NavSelActionArr)
                {
                    if (!string.IsNullOrWhiteSpace(nav))
                    {
                        var arr = nav.Split('_');
                        roleNavActions.Add(new RoleNavAction() { Action_Code = arr[1], CreateTime = DateTime.Now, Nav_Code = arr[0], Role = role });
                    }
                }
            }

            List<RoleArticleCategoryAction> roleArticleCategoryActions = new List<RoleArticleCategoryAction>();
            if (!string.IsNullOrEmpty(input.Cate_SelActions))
            {
                string[] CateSelActionArr = input.Cate_SelActions.Split(',');
                foreach (var cate in CateSelActionArr)
                {
                    if (!string.IsNullOrWhiteSpace(cate))
                    {
                        var arr = cate.Split('_');
                        roleArticleCategoryActions.Add(new RoleArticleCategoryAction()
                        {
                            ActionCode = arr[1],
                            ArticleCategoryID = Convert.ToInt32(arr[0]),
                            Role = role,
                            CreateTime = DateTime.Now
                        });
                    }
                }
            }
            db.RoleNavActions.AddRange(roleNavActions);
            db.RoleArticleCategoryActions.AddRange(roleArticleCategoryActions);

            return db.SaveChanges() > 0 ? role : null;
        }

        public bool EditInfo(UpdateRoleInput input)
        {
            Role role = db.Roles.Find(input.ID);
            role = input.MapTo(role);
            db.Entry<Role>(role).State = EntityState.Modified;

            var removeNavList = db.RoleNavActions.Where(a => a.RoleID == input.ID);
            db.RoleNavActions.RemoveRange(removeNavList);

            List<RoleNavAction> roleNavActions = new List<RoleNavAction>();

            if (!string.IsNullOrEmpty(input.Nav_SelActions))
            {
                string[] NavSelActionArr = input.Nav_SelActions.Split(',');
                foreach (var nav in NavSelActionArr)
                {
                    if (!string.IsNullOrWhiteSpace(nav))
                    {
                        var arr = nav.Split('_');
                        roleNavActions.Add(new RoleNavAction()
                        {
                            Action_Code = arr[1],
                            CreateTime = DateTime.Now,
                            Nav_Code = arr[0],
                            Role = role
                        });
                    }
                }
            }

            var removeCateList = db.RoleArticleCategoryActions.Where(a => a.RoleID == input.ID);
            db.RoleArticleCategoryActions.RemoveRange(removeCateList);

            List<RoleArticleCategoryAction> roleArticleCategoryActions = new List<RoleArticleCategoryAction>();

            if (!string.IsNullOrEmpty(input.Cate_SelActions))
            {
                string[] CateSelActionArr = input.Cate_SelActions.Split(',');
                foreach (var cate in CateSelActionArr)
                {
                    if (!string.IsNullOrWhiteSpace(cate))
                    {
                        var arr = cate.Split('_');
                        roleArticleCategoryActions.Add(new RoleArticleCategoryAction()
                        {
                            ActionCode = arr[1],
                            ArticleCategoryID = Convert.ToInt32(arr[0]),
                            Role = role,
                            CreateTime = DateTime.Now
                        });
                    }
                }
            }

            db.RoleNavActions.AddRange(roleNavActions);
            db.RoleArticleCategoryActions.AddRange(roleArticleCategoryActions);

            return db.SaveChanges() > 0;
        }

        public string GetSelCategoryActions(int id, int categoryID)
        {
            var list = db.RoleArticleCategoryActions.Where(a => a.RoleID == id && a.ArticleCategoryID == categoryID).Select(a => a.ActionCode);
            return string.Join(",", list);
        }

        public string GetSelectNavActions(int id,string navCode)
        {
            var list = db.RoleNavActions.Where(a => a.RoleID == id && a.Nav_Code == navCode).Select(a => a.Action_Code);
            return string.Join(",", list);
        }
    }
}

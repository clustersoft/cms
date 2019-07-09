using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Extension;
using CMS.Application.Navgations.Dto;
using CMS.Application.Users.Dto;
using CMS.Model;
using CMS.Util;

namespace CMS.Application.Users
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public LoginOutput Login(LoginInput input)
        {
            var user = db.Users.FirstOrDefault(u => u.LoginName == input.loginname && u.PassWord == input.password);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (user.Status == 0)
                {
                    throw new UserFriendlyException("账号未启用");
                }
                else
                {
                    //记录日志
                    //记录IP，登录时间
                    user.LastLoginIP = IPHelper.GetHostAddress();
                    user.LastLoginTime = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return user.MapTo<LoginOutput>();
            }
        }

        public bool ChangePassWord(ChangePwdInput inpt)
        {
            var user = db.Users.Find(inpt.ID);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            else
            {
                if (user.PassWord != inpt.oldPwd)
                {
                    throw new UserFriendlyException("原密码不正确");
                }
                else
                {
                    user.PassWord = inpt.newPwd;
                    db.Entry(user).State = EntityState.Modified;
                    return db.SaveChanges() > 0;
                }
            }
        }


        public User GetInclude(int id)
        {
            var user = db.Users.Include(a => a.UserRoles).FirstOrDefault(a => a.ID == id);
            return user;
        }

        public User AddInfo(User user, int[] roleIds)
        {
            var checkUser = db.Users.FirstOrDefault(a => a.LoginName == user.LoginName);
            if (checkUser != null)
            {
                throw new UserFriendlyException("登录名重复");
            }

            db.Users.Add(user);
            var Roles = db.Roles.Where(a => roleIds.Contains(a.ID));
            var userRoles = new List<UserRole>();
            foreach (var role in Roles)
                userRoles.Add(new UserRole() { User = user, Role = role });

            db.UserRoles.AddRange(userRoles);
            return db.SaveChanges() > 0 ? user : null;
        }

        public bool editInfo(UpdateUserInput input, int[] roleIds)
        {
            var user = db.Users.Find(input.ID);

            if (user == null)
                throw new UserFriendlyException(LocalizationConst.UserNoExist);

            user = input.MapTo(user);
            user.ModifyTime = DateTime.Now;
            user.ModifyIP = IPHelper.GetIPAddress;

            var list = db.UserRoles.Where(a => a.UserID == user.ID);
            db.UserRoles.RemoveRange(list);

            var Roles = db.Roles.Where(a => roleIds.Contains(a.ID));
            var userRoles = new List<UserRole>();
            foreach (var role in Roles)
                userRoles.Add(new UserRole() { User = user, Role = role });
            db.UserRoles.AddRange(userRoles);

            db.Entry(user).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public int[] GetPermCategoryIds(int id)
        {
           var list= from a in db.RoleArticleCategoryActions
                     join b in db.UserRoles on a.RoleID equals b.RoleID
                     join c in db.Users on b.UserID equals c.ID 
                     where c.ID==id
                     group a by new { a.ArticleCategoryID }
                     into g
                     select g.FirstOrDefault();
           return list.Select(a => a.ArticleCategoryID).ToArray();
        }

        public string[] GetPermNavCodes(int id)
        {
            var list = from a in db.RoleNavActions
                       join b in db.UserRoles on a.RoleID equals b.RoleID
                       join c in db.Users on b.UserID equals c.ID
                       where c.ID == id&&a.Action_Code.ToLower()=="show"
                       group a by new { a.Nav_Code}
                       into g
                       select g.FirstOrDefault();
            return list.Select(a => a.Nav_Code).ToArray();
        }

        public GetNavCodeOutput GetNavCode(int userID, string navCode)
        {
            var list = (from a in db.RoleNavActions
                join b in db.UserRoles on a.RoleID equals b.RoleID
                join c in db.Users on b.UserID equals c.ID
                where c.ID == userID && a.Nav_Code == navCode
                select a.Action_Code).AsNoTracking().ToList();
            var user = db.Users.Find(userID);
            var navcode=new GetNavCodeOutput() {NavCode = navCode,IsAdmin=user.Type,ActionCode = string.Join(",",list)};
            return navcode;
        }

        public GetCategoryActionOutput GetCategoryAction(int userID)
        {
            var temp = (from a in db.RoleArticleCategoryActions
                        join b in db.UserRoles on a.RoleID equals b.RoleID
                        join c in db.Users on b.UserID equals c.ID
                        where c.ID == userID
                        group a by a.ArticleCategoryID into g
                        select new 
                        {
                            ArticleCategoryID = g.Key,
                            ActionCode=g.Select(a => a.ActionCode)
                        }).AsNoTracking().ToList();

            var list = temp.Select(s => new GetCategoryActionData()
            {
                ActionCode = string.Join(",", s.ActionCode),
                ArticleCategoryID = s.ArticleCategoryID
            }).ToList();
            var user = db.Users.Find(userID);
            GetCategoryActionOutput output=new GetCategoryActionOutput();
            output.List = list;
            output.IsAdmin = user.Type;

            return output;
        }
    }
}

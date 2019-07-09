using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;
using CMS.Utils;

namespace CMS.Application.Auth
{
    public class AuthService
    {
        public async Task<User> FindUserAsync(string userName, string password)
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(a => a.LoginName == userName && a.PassWord == password&&a.Status==1);
                return user;
            }
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            using (var db = new CMSContext())
            {
                var existingToken = db.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject);

                if (existingToken != null)
                {
                    db.RefreshTokens.Remove(existingToken);
                }

                db.RefreshTokens.Add(token);

                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            using (var db = new CMSContext())
            {
                var refreshToken = await db.RefreshTokens.FindAsync(refreshTokenId);

                if (refreshToken != null)
                {
                    db.RefreshTokens.Remove(refreshToken);
                    return await db.SaveChangesAsync() > 0;
                }

                return false;
            }
        }

        //public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        //{
        //    using (var db = new CMSContext())
        //    {
        //        db.RefreshTokens.Remove(refreshToken);
        //        return await db.SaveChangesAsync() > 0;
        //    }
        //}

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            using (var db = new CMSContext())
            {
                var refreshToken = await db.RefreshTokens.FindAsync(refreshTokenId);

                return refreshToken;
            }
        }
    }
}

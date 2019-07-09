using System;
using System.Threading.Tasks;
using CMS.Application.Auth;
using CMS.Model;
using CMS.Util;
using Microsoft.Owin.Security.Infrastructure;

namespace CMS.WebApi
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        /// <summary>
        /// 生成RefreshToken值，生成后需要持久化在数据库中，客户端需要拿RefreshToken来请求刷新token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("N");

            AuthService _repo = new AuthService();
            var token = new RefreshToken()
            {
                //ID = refreshTokenId.GetHash(),
                ID = refreshTokenId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(CMSConst.AccessRefreshTokenExpireTimeSpanMinute)
            };
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await _repo.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }
        /// <summary>
        /// 拿客户的RefreshToken和数据库中RefreshToken做对比，验证成功后删除此refreshToken
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthService _repo = new AuthService();
            string hashedTokenId = context.Token;
            //string hashedTokenId = context.Token.GetHash();

            var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await _repo.RemoveRefreshToken(hashedTokenId);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CMS.Application.Auth;

namespace CMS.WebApi
{
    /// <summary>
    /// 只有这两个方法(ValidateClientAuthentication、GrantResourceOwnerCredentials)同时认证通过才会颁发token
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 用来对third party application 认证，
        /// 具体的做法是为third party application颁发appKey和appSecrect，
        /// 在此省略了颁发appKey和appSecrect的环节，认为所有的third party application都是合法的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //表示所有允许此third party application请求
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 此方法则是resource owner password credentials模式的重点，由于客户端发送了用户的用户名和密码，所以我们在这里验证用户名和密码是否正确
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //验证用户名密码
            AuthService _repo = new AuthService();
            //IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            var user = await _repo.FindUserAsync(context.UserName, context.Password);
            //var user = _repo.FindUser(context.UserName, context.Password);
            if (user == null)
            {
                //context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.SetError("invalid_grant", "用户名或密码不正确.");
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType); //ClaimsIdentity当作一个NameValueCollection看待
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", context.ClientId ?? string.Empty
                },
                {
                    "userName", context.UserName
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        /// <summary>
        /// 把Context中的属性加入到token中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using CMS.Application;
using CMS.Model;
using CMS.Util;
using CMS.WebApi.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(CMS.WebApi.Startup))]

namespace CMS.WebApi
{
    public class Startup
    {
        /*
           一、向服务器请求token （POST，x-www-form-urlencoded）
           resource owner password credentials模式需要body包含3个参数：
           grant_type：必须为password
           username：用户名
           password：用户密码

           二、使用token访问受保护的api
           在Header中加入：Authorization – bearer {{token}}，此token就是上一步得到的token。

           三、当token过期后，凭借上次得到的refresh_token重新获取token（POST，x-www-form-urlencoded），每次refresh_token只能用一次
           grant_type： 必须为refresh_token
           refresh_token：上次（包含在获取的token信息中）得到的refresh_token
       */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            //IOC
            ContainerBuilder builder = new ContainerBuilder();
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("I")).AsImplementedInterfaces();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //AutoMapper
            Bootstrapper.RegisterAutoMapper();

            //Log4Net
            Bootstrapper.RegisterLog();

            #if DEBUG
                //EF生产SQL监控
                HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            #endif

            //EF初始化
            using (var dbcontext = new CMSContext())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
           
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            app.UseCors(CorsOptions.AllowAll); //同源策略
            ConfigureOAuth(app);
            //这一行代码必须放在ConfiureOAuth(app)之后
            app.UseWebApi(config);
        }

        /// <summary>
        /// 开启了OAuth服务
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //允许客户端使用http协议请求；
                TokenEndpointPath = new PathString("/token"), //token请求的地址，即http://localhost:端口号/token；
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(CMSConst.AccessTokenExpireTimeSpanMinute),
                //token过期时间；
                Provider = new SimpleAuthorizationServerProvider(), //提供具体的认证策略；

                //refresh token provider
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}

using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TokenAuthentication.Providers;

namespace TokenAuthentication
{

    // Install-Package Microsoft.AspNet.WebApi.Owin
    // Install-Package Microsoft.Owin.Host.SystemWeb

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Install-Package Microsoft.AspNet.Identity.Owin


            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            
            WebApiConfig.Register(config);
            // Install-Package Microsoft.Owin.Cors
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        
        // Install-Package Microsoft.Owin.Security.OAuth

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                Provider = new ServiceAuthorizationServerProvider()  
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
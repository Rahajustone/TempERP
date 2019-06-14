using System.Web.Http;
using Owin;

namespace Samr.ERP.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            OAuthConfig.ConfigureOAuth(app);

            WebApiConfig.Register(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);

        }

    }
}
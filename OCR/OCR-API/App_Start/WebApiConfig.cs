using System.Web.Http;
using System.Web.Http.Cors;

namespace OCR_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.EnableCors(new EnableCorsAttribute("*", "", ""));

            //app.UseCors(CorsOptions.AllowAll);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Formatters.Add(new BrowserJsonFormatter());
        }
    }
}

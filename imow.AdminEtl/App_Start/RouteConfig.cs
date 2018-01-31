using System.Web.Mvc;
using System.Web.Routing;

namespace imow.AdminEtl
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Index", action = "Index"},
                namespaces:new string[] { "imow.AdminEtl.Controllers"}
            );
        }

    }
}

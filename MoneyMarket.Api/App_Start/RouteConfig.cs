using System.Web.Mvc;
using System.Web.Routing;

namespace MoneyMarket.Api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "NewEmailVerification",                                           // Route name
                "NewEmailVerification",                            // URL with parameters
                new { controller = "User", action = "NewEmailVerification" }  // Parameter defaults
            );

            routes.MapRoute(
                "PrivacyPolicy",                                           // Route name
                "PrivacyPolicy",                            // URL with parameters
                new { controller = "Home", action = "PrivacyPolicy" }  // Parameter defaults
            );

            routes.MapRoute(
                "GrantSuccess",                                           // Route name
                "GrantSuccess",                            // URL with parameters
                new { controller = "Home", action = "GrantSuccess" }  // Parameter defaults
            );
            routes.MapRoute(
                "Grant",                                           // Route name
                "Grant",                            // URL with parameters
                new { controller = "Home", action = "Grant" }  // Parameter defaults
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{parameter}",
                defaults: new { controller = "Home", action = "Index", parameter = UrlParameter.Optional }
            );
        }
    }
}

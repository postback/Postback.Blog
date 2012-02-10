using System.Web.Mvc;
using System.Web.Routing;

namespace Postback.Blog.App.Bootstrap
{
    public class MvcBoostrap : IStartUpTask
    {
        public void Configure()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Old Blog", // Route name
               "blog/{*url}", // URL with parameters
               new { controller = "Error", action = "Index" }, // Parameter defaults
               namespaces: new string[] { "Postback.Blog.Controllers" }
           );

            routes.MapRoute(
                "Tag", // Route name
                "tag/{tag}", // URL with parameters
                new { controller = "Post", action = "Tag", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new string[] { "Postback.Blog.Controllers" }
            );

            routes.MapRoute(
                "Post detail", // Route name
                "read/{slug}", // URL with parameters
                new { controller = "Post", action = "Post", slug = UrlParameter.Optional }, // Parameter defaults
                namespaces: new string[] { "Postback.Blog.Controllers" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Post", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new string[] { "Postback.Blog.Controllers" }
            );

            

        }
    }
}
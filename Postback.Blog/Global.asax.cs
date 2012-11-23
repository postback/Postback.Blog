using System.Web.Mvc;
using Postback.Blog.App.Bootstrap;
using Postback.Blog.App.DependencyResolution;
using StructureMap;

namespace Postback.Blog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DependencyResolver.SetResolver(new ServiceLocatorDependencyResolver());
        }

    }
}
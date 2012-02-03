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
            InitializeContainer();

            Bootstrapper.StartUp();
        }

        private static void InitializeContainer()
        {
            //Paradox is that we are implicitly using some dependencies here, while the purpose of this code is to do dependency injection
            DependencyRegistrar.EnsureDependenciesRegistered();
            var container = ObjectFactory.Container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
        }
    }
}
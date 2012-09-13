using System.Web;
using Postback.Blog.App.Security;
using Postback.Blog.App.Services;
using StructureMap.Configuration.DSL;

namespace Postback.Blog.App.DependencyResolution
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            For<HttpContext>().Use(() => HttpContext.Current);
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));

            For<IAuth>().Use<FormsAuthWrapper>();
            For<IMessagingService>().Use<SimpleMessagingService>();

            For<ISecurityContext>().Use<WebSecurityContext>();
            For<ISystemClock>().Use<SystemClock>();
        }
    }
}
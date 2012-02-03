using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using StructureMap.Configuration.DSL;

namespace Postback.Blog.App.DependencyResolution
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            For<IPersistenceSession>()
                .Use<MongoSession>();

            For<IAuth>()
                .Use<FormsAuthWrapper>();

            For<IMessagingService>()
                .Use<SimpleMessagingService>();
        }
    }
}
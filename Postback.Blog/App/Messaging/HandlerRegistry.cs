using Postback.Blog.App.Services;
using Postback.Blog.Models;
using StructureMap.Configuration.DSL;

namespace Postback.Blog.App.Messaging
{
    public class HandlerRegistry : Registry
    {
        public HandlerRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.ConnectImplementationsToTypesClosing(typeof(Handler<>));
            });
        }

    }
}
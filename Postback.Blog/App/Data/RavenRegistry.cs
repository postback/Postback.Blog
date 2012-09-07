using System.Configuration;
using Raven.Client;
using StructureMap.Configuration.DSL;

namespace Postback.Blog.App.Data
{
    public class RavenRegistry : Registry
    {
        public RavenRegistry()
        {
            For<IPersistenceSession>()
                .Use<RavenSession>();

            For<IDocumentStore>().Use(InitDocumentStore());
            For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(x =>
                {
                    var store = x.GetInstance<IDocumentStore>();
                    return store.OpenSession();
                });
        }

        public IDocumentStore InitDocumentStore()
        {
            var documentStore = new Raven.Client.Document.DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };

            documentStore.Initialize();
            return documentStore;
        }
    }
}
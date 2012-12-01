using System;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Config;
using Raven.Database.Server;

namespace Postback.Blog.Tests.Data
{
    public class BaseRavenTest : BaseTest, IDisposable
    {
        protected EmbeddableDocumentStore store;

        public EmbeddableDocumentStore NewStore()
        {
            store = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    DefaultStorageTypeName = "munin",
                    RunInMemory = true,
                },
                UseEmbeddedHttpServer = false,
            };

            ModifyStore(store);
            ModifyConfiguration(store.Configuration);

            store.RegisterListener(new WaitForNonStaleResults());
            store.Initialize();

            CreateDefaultIndexes(store);

            return store;
        }

        protected virtual void ModifyStore(EmbeddableDocumentStore documentStore)
        {

        }

        protected virtual void ModifyConfiguration(RavenConfiguration configuration)
        {
        }

        protected virtual void CreateDefaultIndexes(IDocumentStore documentStore)
        {
            new RavenDocumentsByEntityName().Execute(documentStore);
        }

        public virtual void Dispose()
        {
            store.Dispose();
        }
    }
}

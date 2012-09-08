using System;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Config;

namespace Postback.Blog.Tests.Data
{
    public class RavenBaseTest : BaseTest, IDisposable
    {
        public EmbeddableDocumentStore NewStore()
        {
            var store = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    DefaultStorageTypeName = "munin",
                    RunInMemory = true,
                },
                UseEmbeddedHttpServer = true,
            };

            ModifyStore(store);
            ModifyConfiguration(store.Configuration);

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
        }
    }
}

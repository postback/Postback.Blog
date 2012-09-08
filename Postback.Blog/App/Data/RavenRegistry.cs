using System;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Security;
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

        public static Func<IDocumentStore> InitDocumentStore = () =>
        {
            var documentStore = new Raven.Client.Document.DocumentStore
                                    {
                                        ConnectionStringName = "RavenDB"
                                    };

            documentStore.RegisterListener(new AuditableEntityListener(() => { return ServiceLocator.Current.GetInstance<ISecurityContext>().CurrentUser.Identity.Name; }));

            documentStore.Initialize();
            return documentStore;
        };
    }
}
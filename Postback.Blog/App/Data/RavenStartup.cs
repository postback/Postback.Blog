using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Bootstrap;
using Raven.Client;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Data
{
    public class RavenStartup : IStartUpTask
    {
        public void Configure()
        {
            IndexCreation.CreateIndexes(typeof(RavenStartup).Assembly, ServiceLocator.Current.GetInstance<IDocumentStore>());
        }
    }
}
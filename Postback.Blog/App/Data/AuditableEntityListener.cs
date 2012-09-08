using System;
using System.Web.WebPages;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.Models;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace Postback.Blog.App.Data
{
    public class AuditableEntityListener : IDocumentStoreListener
    {
        private Func<string> getCurrentUser;

        public AuditableEntityListener(Func<string> getCurrentUser)
        {
            this.getCurrentUser = getCurrentUser;
        }

        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
        {
            var auditableEntity = entityInstance as AuditedEntity;

            if (auditableEntity == null)
                return false;

            var now = ServiceLocator.Current.GetInstance<ISystemClock>().Now();
            var author = getCurrentUser();
            if (auditableEntity.CreatedBy.IsEmpty())
            {
                auditableEntity.CreatedBy = author;
                auditableEntity.Created = now;
            }

            return true;
        }

        public void AfterStore(string key, object entityInstance, RavenJObject metadata)
        {

        }
    }
}

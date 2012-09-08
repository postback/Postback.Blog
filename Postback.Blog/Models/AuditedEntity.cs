using System;

namespace Postback.Blog.Models
{
    public abstract class AuditedEntity : Entity, IAuditable
    {
        private string createdby;
        private DateTime created;

        public AuditedEntity()
        {
        }

        public string CreatedBy
        {
            get { return createdby; }
            set { createdby = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
    }
}
using System;

namespace Postback.Blog.Models
{
    public abstract class AuditedEntity : Entity, IAuditable
    {
        private User author;
        private DateTime created;

        public User Author
        {
            get { return author; }
            set { author = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
    }
}
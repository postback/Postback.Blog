using System;

namespace Postback.Blog.Models
{
    public class Comment : Entity
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Body { get; set; }
        public string IpAddress { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;


namespace Postback.Blog.Models
{
    public class Post : AuditedEntity
    {
        public Post():base()
        {
            Tags = new List<Tag>();
            Comments = new List<Comment>();
            Active = true;
        }

        public Post(string title):this()
        {
            Uri = title.ToUri();
            Title = title;
        }

        public string Title { get; set; }
        public string Uri { get; set; }
        public string Body { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Comment> Comments { get; set; }
        public Category Category { get; set; }
        public bool Active { get; set; }
        public DateTime? PublishFrom { get; set; }
    }
}
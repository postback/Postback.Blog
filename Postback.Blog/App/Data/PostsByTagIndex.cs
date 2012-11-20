using Postback.Blog.Models;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Data
{
    public class PostsByTagIndex : AbstractIndexCreationTask<Post, PostsByTagIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public string Tag { get; set; }
            public int PostId { get; set; }
        }
 
        public PostsByTagIndex()
        {
            Map = posts => from post in posts
                           from tag in post.Tags
                           select new
                           {
                               Tag = tag,
                               PostId = post.Id
                           };
 
            Reduce = results => from result in results
                                group result by result.Tag
                                    into g
                                    select new
                                    {
                                        Tag = g.Key
                                    };
        }
    }
}
using Postback.Blog.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Data.Indexes
{
    public class PostSearchIndex : AbstractIndexCreationTask<Post, PostSearchIndex.Result>
    {
        public class Result { public string Content; }

        public PostSearchIndex()
        {
            Map = post => from p in post
                          select new
                          {
                              Content =
                                new object[]
                                    {
                                        p.Title, p.Tags, p.Body
                                    },
                                    PostId = p.Id
                            };

            Index(x => x.Content, FieldIndexing.Analyzed);
        }
    }
}
using Postback.Blog.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Data.Indexes
{
    public class PostIndex : AbstractIndexCreationTask<Post, Post>
    {
        public PostIndex()
        {
            Map = posts => from doc in posts
                           select new { doc.Tags, doc.Body, doc.Title, doc.Id };

            Stores.Add(x => x.Title, FieldStorage.Yes);
            Stores.Add(x => x.Id, FieldStorage.Yes);

            Indexes.Add(x => x.Title, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Tags, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Body, FieldIndexing.Analyzed);
        }
    }
}
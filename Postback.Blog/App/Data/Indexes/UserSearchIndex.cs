using Postback.Blog.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Data.Indexes
{
    public class UserSearchIndex : AbstractIndexCreationTask<User, UserSearchIndex.Result>
    {
        public class Result { public string Content; }

        public UserSearchIndex()
        {
            Map = user => from u in user
                          select new
                          {
                              Content =
                                new object[]
                                    {
                                        u.Email, u.Name
                                    },
                              UserId = u.Id
                          };

            Index(x => x.Content, FieldIndexing.Analyzed);
        }
    }
}
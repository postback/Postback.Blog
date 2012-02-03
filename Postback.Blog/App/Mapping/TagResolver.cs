using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.App.Mapping
{
    public class TagResolver : ValueResolver<PostEditModel, IList<Tag>>
    {
        protected override IList<Tag> ResolveCore(PostEditModel source)
        {
            var tags = new List<Tag>();
            if(!string.IsNullOrEmpty(source.Tags))
            {
                return source.Tags.Split(new[] { ',', ';' }).Select(s => new Tag(s.Trim())).ToList();
            }

            return null;
        }
    }
}
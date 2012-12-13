using AutoMapper;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Mapping
{
    public class IsPublishedResolver : ValueResolver<Post, Boolean>
    {
        protected override bool ResolveCore(Post source)
        {
            return source.Active && (source.PublishFrom == null || source.PublishFrom <= ServiceLocator.Current.GetInstance<ISystemClock>().Now());
        }
    }
}
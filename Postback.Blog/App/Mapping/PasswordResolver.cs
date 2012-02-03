using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.App.Mapping
{
    public class PasswordResolver : ValueResolver<UserEditModel, string>
    {
        protected override string ResolveCore(UserEditModel source)
        {
            if(!string.IsNullOrEmpty(source.Password))
            {
                return source.Password;
            }

            return null;
        }
    }
}
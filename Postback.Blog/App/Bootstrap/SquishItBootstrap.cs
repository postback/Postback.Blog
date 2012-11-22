using AutoMapper;
using Postback.Blog.App.Mapping;
using SquishIt.Framework;

namespace Postback.Blog.App.Bootstrap
{
    public class SquishItBootstrap : IStartUpTask
    {
        public void Configure()
        {
            //Front-end
            Bundle.JavaScript()
            .Add("~/js/lib/modernizr-2.6.2.js")
            .Add("~/js/lib/jquery.validate.js")
            .Add("~/js/lib/jquery.validate.unobtrusive.js")
            .Add("~/js/app/core.js")
            .AsCached("frontendscripts", "~/assets/js/frontendscripts");

            Bundle.Css()
           .Add("~/css/styles.css")
           .AsCached("frontend", "~/assets/css/frontend");

            //back-end
            Bundle.JavaScript()
            .Add("~/js/lib/modernizr-2.6.2.js")
            .Add("~/js/lib/jquery.validate.js")
            .Add("~/js/lib/jquery.validate.unobtrusive.js")
            .Add("~/js/lib/bootstrap.min.js")
            .Add("~/js/app/admin.js")
            .AsCached("backendscripts", "~/assets/js/backendscripts");

            Bundle.Css()
           .Add("~/css/lib/bootstrap.min.css")
            .AsCached("backend", "~/assets/css/backend");
        }
    }
}
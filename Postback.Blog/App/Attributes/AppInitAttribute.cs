using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using Postback.Blog.App.Data;
using Postback.Blog.Models;
using StructureMap;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;

namespace Postback.Blog
{
    public class AppInitAttribute : ActionFilterAttribute
    {
        private IDocumentSession session;

        public AppInitAttribute()
        {
            this.session = ServiceLocator.Current.GetInstance<IDocumentSession>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var count = this.session.Query<User>().Count();

                if (count == 0)
                {
                    filterContext.HttpContext.Response.Redirect("/admin/setup", true);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
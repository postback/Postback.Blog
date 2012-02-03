using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Norm;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using StructureMap;

namespace Postback.Blog
{
    public class AppInitAttribute : ActionFilterAttribute
    {
        private IPersistenceSession session;

        public AppInitAttribute()
        {
            this.session = ObjectFactory.GetInstance<IPersistenceSession>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var count = this.session.All<User>().Count();

                if (count == 0)
                {
                    filterContext.HttpContext.Response.Redirect("/admin/setup", true);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
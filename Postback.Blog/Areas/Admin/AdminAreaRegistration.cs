using System.Web.Mvc;

namespace Postback.Blog.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller="Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

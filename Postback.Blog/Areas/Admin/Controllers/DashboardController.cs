using System.Web.Mvc;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [Authorize]
    [AppInit]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}

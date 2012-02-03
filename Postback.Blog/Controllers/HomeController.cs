using System;
using System.Linq;
using System.Web.Mvc;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;

namespace Postback.Blog.Controllers
{
    public class HomeController : Controller
    {
        private const int PAGE_SIZE = 2;
        private IPersistenceSession session;

        public HomeController(IPersistenceSession session)
        {
            this.session = session;
        }

        public ActionResult Index(int? page)
        {
            ViewBag.Message = "Postback Blog";
            ViewBag.Paging = new PagingView("home","index")
                                 {
                                     ItemCount = session.All<Post>().Count(),
                                     CurrentPage = page.HasValue ? page.Value : 0,
                                     ItemsOnOnePage = PAGE_SIZE
                                 };

            return View(session.All<Post>().OrderByDescending(p => p.Created).Skip(page.HasValue ? (page.Value-1)*PAGE_SIZE : 0).Take(PAGE_SIZE).ToList());
        }
    }
}

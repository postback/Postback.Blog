using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;

namespace Postback.Blog.Controllers
{
    public class PostController : Controller
    {
        private const int PAGE_SIZE = 10;
        private IPersistenceSession session;

        public PostController(IPersistenceSession session)
        {
            this.session = session;
        }

        [OutputCache(Duration = 1200, VaryByParam = "page")]
        public ActionResult Index(int? page)
        {
            ViewBag.Paging = new PagingView()
                                 {
                                     ItemCount = session.All<Post>().Count(),
                                     CurrentPage = page.HasValue ? page.Value : 0,
                                     ItemsOnOnePage = PAGE_SIZE
                                 };
            var clock = ServiceLocator.Current.GetInstance<ISystemClock>();
            return View("index", session.All<Post>().Where(p => p.Active && (p.PublishFrom == null || p.PublishFrom <= clock.Now())).OrderByDescending(p => p.Created).Skip(page.HasValue ? (page.Value - 1) * PAGE_SIZE : 0).Take(PAGE_SIZE).ToList());
        }

        [OutputCache(Duration = 1200, VaryByParam = "tag")]
        public ActionResult Tag(string tag, int? page)
        {
            ViewBag.Tag = tag;
            ViewBag.Paging = new PagingView()
            {
                ItemCount = session.All<Post>().Where(p => p.Tags.Any(t => t.Uri == tag)).Count(),
                CurrentPage = page.HasValue ? page.Value : 0,
                ItemsOnOnePage = PAGE_SIZE,
            };

            return View(session.All<Post>().Where(p => p.Tags.Any(t => t.Uri == tag)).OrderByDescending(p => p.Created).Skip(page.HasValue ? (page.Value - 1) * PAGE_SIZE : 0).Take(PAGE_SIZE).ToList());
        }

         [OutputCache(Duration = 1200, VaryByParam = "page")]
        public ActionResult Post(string slug)
        {
            var post = session.FindOne<Post>(p => p.Uri == slug);
            if (post != null && post.Active && (post.PublishFrom == null || post.PublishFrom <= DateTime.Now) || HttpContext.User.Identity.IsAuthenticated)
            {
                return View(post);
            }
            
            throw new HttpException(404,"Post not found");
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}

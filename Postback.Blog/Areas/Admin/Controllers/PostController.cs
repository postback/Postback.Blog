using System.Linq;
using System.Web.Mvc;
using AutoMapper;

using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;
using Raven.Client.Document;
using Raven.Client;
using Raven.Client.Linq;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Data.Indexes;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private IDocumentSession session;

        public PostController()
        {
            this.session = ServiceLocator.Current.GetInstance < IDocumentSession>();
        }

        public ActionResult Index(int? page)
        {
            var posts = this.session.Query<Post>()
                .OrderByDescending(p => p.Created)
                .Skip(page.HasValue ? ((page.Value - 1) * Settings.PageSize) : 0)
                .Take(Settings.PageSize).ToList();

            ViewBag.Paging = new PagingView()
            {
                ItemCount = session.Query<Post>().Count(),
                CurrentPage = page.HasValue ? page.Value : 0,
                ItemsOnOnePage = Settings.PageSize
            };

            var models = posts.Select(Mapper.Map<Post, PostViewModel>)
                .ToList();

            return View(models);
        }

        public JsonResult Search(string query) {

            var posts = session.Query<PostSearchIndex.Result, PostSearchIndex>().Search(x => x.Content, query).As<Post>().ToList();

            return Json(posts);
        }

        public ActionResult Edit(string id)
        {
            var post = session.Query<Post>().SingleOrDefault(u => u.Id == id);
            if (post != null)
            {
                return View(Mapper.Map<Post, PostEditModel>(post));
            }

            return View(new PostEditModel());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PostEditModel model)
        {
            if (ModelState.IsValid)
            {
                var post = Mapper.Map<PostEditModel, Post>(model);
                session.Store(post);
                session.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var post = session.Query<Post>().SingleOrDefault(u => u.Id == id);
            if (post != null)
            {
                session.Delete<Post>(post);
                session.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

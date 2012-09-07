using System.Linq;
using System.Web.Mvc;
using AutoMapper;

using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private IPersistenceSession session;

        public PostController(IPersistenceSession session)
        {
            this.session = session;
        }

        public ActionResult Index(int? page)
        {
            var posts = this.session.All<Post>()
                .OrderByDescending(p => p.Created)
                .Skip(page.HasValue ? ((page.Value - 1) * Settings.PageSize) : 0)
                .Take(Settings.PageSize).ToList();

            var models = posts.Select(Mapper.Map<Post, PostViewModel>)
                .ToList();

            return View(models);
        }

        public ActionResult Edit(string id)
        {
            var post = session.Single<Post>(u => u.Id == id);
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
                session.Save<Post>(post);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var post = session.Single<Post>(u => u.Id == id);
            if (post != null)
            {
                session.Delete<Post>(post);
            }

            return RedirectToAction("Index");
        }
    }
}

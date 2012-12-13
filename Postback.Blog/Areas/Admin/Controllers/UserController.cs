using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Raven.Client.Linq;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;
using Raven.Client;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Data.Indexes;
using System.Collections.Generic;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IDocumentSession session;

        public UserController()
        {
            this.session = ServiceLocator.Current.GetInstance < IDocumentSession>();
        }

        public ActionResult Index(int? page, string q)
        {
            var users = new List<User>();

            if (string.IsNullOrEmpty(q))
            {
                users = this.session.Query<User>()
                 .Skip(page.HasValue ? ((page.Value - 1) * Settings.PageSize) : 0)
                 .Take(Settings.PageSize)
                 .ToList();

                ViewBag.Paging = new PagingView()
                {
                    ItemCount = session.Query<User>().Count(),
                    CurrentPage = page.HasValue ? page.Value : 0,
                    ItemsOnOnePage = Settings.PageSize
                };
            }
            else {
                users = session.Query<UserSearchIndex.Result, UserSearchIndex>().Search(x => x.Content, q).As<User>().ToList();

                ViewBag.Query = q;
            }

           

            var models = users.Select(Mapper.Map<User, UserViewModel>).ToList();

            return View(models);
        }

        public JsonResult Search(string query)
        {
            var users = session.Query<UserSearchIndex.Result, UserSearchIndex>().Search(x => x.Content, query).As<User>().ToList();

            return Json(users);
        }

        public ActionResult Edit(string id)
        {
            var user = session.Query<User>().SingleOrDefault(u => u.Id == id);
            if(user != null)
            {
                return View(Mapper.Map<User, UserEditModel>(user));
            }

            return View(new UserEditModel());
        }

        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<UserEditModel, User>(model);
                session.Store(user);
                session.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var user = session.Query<User>().SingleOrDefault(u => u.Id == id);
            if(user != null && user.Email != HttpContext.User.Identity.Name)
            {
                session.Delete<User>(user);
                session.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

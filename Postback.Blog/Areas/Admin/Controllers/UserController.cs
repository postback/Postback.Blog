using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Norm;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IPersistenceSession session;

        public UserController(IPersistenceSession session)
        {
            this.session = session;
        }

        public ActionResult Index(int? page)
        {
            var users = this.session.All<User>()
                .Skip(page.HasValue ? ((page.Value - 1)*Settings.PageSize):0)
                .Take(Settings.PageSize)
                .Select(u => Mapper.Map<User,UserViewModel>(u))
                .ToList();

            return View(users);
        }

        public ActionResult Edit(ObjectId id)
        {
            var user = session.Single<User>(u => u.Id == id);
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
                session.Save<User>(user);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ObjectId id)
        {
            var user = session.Single<User>(u => u.Id == id);
            if(user != null && user.Email != HttpContext.User.Identity.Name)
            {
                session.Delete<User>(user);
            }

            return RedirectToAction("Index");
        }
    }
}

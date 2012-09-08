using System.Linq;
using System.Web.Mvc;
using AutoMapper;

using Postback.Blog.App.Data;
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
                .ToList();

            var models = users.Select(Mapper.Map<User, UserViewModel>).ToList();

            return View(models);
        }

        public ActionResult Edit(string id)
        {
            var user = session.FindOne<User>(u => u.Id == id);
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
                session.Save(user);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var user = session.FindOne<User>(u => u.Id == id);
            if(user != null && user.Email != HttpContext.User.Identity.Name)
            {
                session.Delete<User>(user);
            }

            return RedirectToAction("Index");
        }
    }
}

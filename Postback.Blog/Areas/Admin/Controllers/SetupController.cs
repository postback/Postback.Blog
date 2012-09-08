using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        private IPersistenceSession session;

        public SetupController(IPersistenceSession session)
        {
            this.session = session;
        }

        public ActionResult Index()
        {
            if (session.All<User>().Count() == 0)
            {
                return View(new InitialSetupModel());
            }

            return RedirectToAction("index", "authentication");
        }

        [HttpPost]
        public ActionResult Index(InitialSetupModel user)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<InitialSetupModel, User>(user);
                session.Add<User>(entity);

                return RedirectToAction("index", "authentication");
            }

            return View("Index", user);
        }
    }
}

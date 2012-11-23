using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;

namespace Postback.Blog.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        private IDocumentSession session;

        public SetupController(IDocumentSession session)
        {
            this.session = session;
        }

        public ActionResult Index()
        {
            if (session.Query<User>().Count() == 0)
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
                session.Store(entity);
                session.SaveChanges();

                return RedirectToAction("index", "authentication");
            }

            return View("Index", user);
        }
    }
}

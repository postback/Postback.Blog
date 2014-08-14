using System.Linq;
using System.Web.Mvc;

using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Raven.Client;

namespace Postback.Blog.Areas.Api.Controllers
{
    public class UserController : Controller
    {
        private IDocumentSession session;

        public UserController(IDocumentSession session)
        {
            this.session = session;
        }

        public JsonResult IsUnique(string email, string id)
        {
            var collection = session.Query<User>().Where(u => u.Email == email);
            if (collection.Count() == 0 || collection.First().Id == id)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("That e-mailadres is not unique", JsonRequestBehavior.AllowGet);
        }

    }
}

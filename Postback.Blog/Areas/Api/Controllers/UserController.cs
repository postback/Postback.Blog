using System.Linq;
using System.Web.Mvc;
using Norm;
using Postback.Blog.App.Data;
using Postback.Blog.Models;

namespace Postback.Blog.Areas.Api.Controllers
{
    public class UserController : Controller
    {
        private IPersistenceSession session;

        public UserController(IPersistenceSession session)
        {
            this.session = session;
        }

        public JsonResult IsUnique(string email, string id)
        {
            var collection = session.All<User>().AsQueryable().Where(u => u.Email == email);
            if (collection.Count() == 0 || collection.First().Id == new ObjectId(id))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("That e-mailadres is not unique", JsonRequestBehavior.AllowGet);
        }

    }
}

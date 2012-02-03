using System.Linq;
using System.Web.Mvc;
using Norm;
using Postback.Blog.App.Data;
using Postback.Blog.Models;

namespace Postback.Blog.Areas.Api.Controllers
{
    public class PostController : Controller
    {
        private IPersistenceSession session;

        public PostController(IPersistenceSession session)
        {
            this.session = session;
        }

        public JsonResult IsUnique(string title, string id)
        {
            var collection = session.All<Post>().AsQueryable().Where(u => u.Title == title);
            if (collection.Count() == 0 || collection.First().Id == new ObjectId(id))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("That title is not unique", JsonRequestBehavior.AllowGet);
        }

    }
}

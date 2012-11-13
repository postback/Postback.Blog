using SquishIt.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Postback.Blog.Controllers
{
    public class AssetsController : Controller
    {
        public ActionResult Js(string id)
        {
            // Set max-age to a year from now
            Response.Cache.SetMaxAge(TimeSpan.FromDays(365));
            return Content(Bundle.JavaScript().RenderCached(id), "text/javascript");
        }

        public ActionResult Css(string id)
        {
            // Set max-age to a year from now
            Response.Cache.SetMaxAge(TimeSpan.FromDays(365));
            return Content(Bundle.Css().RenderCached(id), "text/css");
        }
    }
}

using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Postback.Blog.App.Data;
using Postback.Blog.App.Mvc;
using Postback.Blog.Models;

namespace Postback.Blog.Controllers
{
    public class RssController : Controller
    {
         private const int PAGE_SIZE = 10;
        private IPersistenceSession session;

        public RssController(IPersistenceSession session)
        {
            this.session = session;
        }

        public ActionResult Index()
        {
            var feed = new SyndicationFeed() { Title = new TextSyndicationContent("Interrupted: postback.be RSS Feed"), Description = new TextSyndicationContent("Interrupted: postback.be RSS Feed") };
            feed.Items = session.All<Post>().Where(
                p => p.Active && (p.PublishFrom == null || p.PublishFrom <= DateTime.Now)).OrderByDescending(
                    p => p.Created).Take(PAGE_SIZE)
                .Select(p => new SyndicationItem(p.Title, p.Body, new Uri(Request.Url,"/read/" + p.Uri)){PublishDate = p.Created});
            return new RssActionResult() { Feed = feed };
        }

    }
}

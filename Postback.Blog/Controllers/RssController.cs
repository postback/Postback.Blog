using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
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
            var clock = ServiceLocator.Current.GetInstance<ISystemClock>();
            var feed = new SyndicationFeed() { Title = new TextSyndicationContent("Interrupted: postback.be RSS Feed"), Description = new TextSyndicationContent("Interrupted: postback.be RSS Feed") };
            var items =
                session.All<Post>().Where(p => p.Active && (p.PublishFrom == null || p.PublishFrom <= clock.Now())).
                    OrderByDescending(p => p.Created).Take(PAGE_SIZE).ToList();
            feed.Items = items.Select(p => new SyndicationItem(p.Title, p.Body, new Uri("http://" + Request.Url.Host + "/read/" + p.Uri)) { PublishDate = p.Created });
            return new RssActionResult() { Feed = feed };
        }

    }
}

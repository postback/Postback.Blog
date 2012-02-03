using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest : BaseTest
    {
        [Test]
        public void IndexShouldReturnView()
        {
            var controller = new HomeController(M<IPersistenceSession>());

            var result = controller.Index(null) as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<Post>)));
            Assert.That(result.ViewBag.Paging, Is.Not.Null);
            Assert.That(result.ViewBag.Paging, Is.InstanceOf(typeof(PagingView)));
        }
    }
}

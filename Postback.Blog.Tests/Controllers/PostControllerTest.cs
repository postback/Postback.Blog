using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class PostControllerTest : BaseTest
    {
        [Test]
        public void IndexShouldReturnView()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(M<ISystemClock>());

            var posts = new List<Post> { new Post() { Active = true }, new Post() { Active = false }, new Post() { Active = true, PublishFrom = new DateTime(9,8,7)} };
            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<Post>()).Return(posts.AsQueryable()).Repeat.Twice();
            var controller = new PostController(session);

            var result = controller.Index(null) as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<Post>)));
            var model = result.Model as IList<Post>;
            model.Count.ShouldEqual(1);
            Assert.That(result.ViewBag.Paging, Is.Not.Null);
            Assert.That(result.ViewBag.Paging, Is.InstanceOf(typeof(PagingView)));
            session.VerifyAllExpectations();
            locator.VerifyAllExpectations();
        }
    }
}

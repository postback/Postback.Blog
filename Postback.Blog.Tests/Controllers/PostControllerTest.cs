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
using Raven.Client;
using Postback.Blog.Tests.Data;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class PostControllerTest : BaseRavenControllerTests
    {
        private IDocumentStore Store { get; set; }

        [TestFixtureSetUp]
        public void SetUp()
        {
            Store = NewStore();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        [Test]
        public void IndexShouldReturnView()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(M<ISystemClock>());

            SetupData(s => s.Store(new Post() { Active = true }));
            SetupData(s => s.Store(new Post() { Active = false }));
            SetupData(s => s.Store(new Post() { Active = true, PublishFrom = new DateTime(9,8,7)} ));

            var session = Store.OpenSession();
            var controller = new PostController(session);

            var result = controller.Index(null) as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<Post>)));
            var model = result.Model as IList<Post>;
            model.Count.ShouldEqual(1);
            Assert.That(result.ViewBag.Paging, Is.Not.Null);
            Assert.That(result.ViewBag.Paging, Is.InstanceOf(typeof(PagingView)));
            locator.VerifyAllExpectations();
        }
    }
}

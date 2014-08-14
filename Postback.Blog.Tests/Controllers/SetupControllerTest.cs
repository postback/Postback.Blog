using System.Linq;
using System.Web.Mvc;

using MvcContrib.TestHelper;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Areas.Admin.Models;
using System.Collections.Generic;
using Rhino.Mocks;
using StructureMap;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Postback.Blog.Tests.Data;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class SetupControllerTest : BaseRavenControllerTests
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

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var controller = new SetupController();

            var result = controller.Index() as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(InitialSetupModel)));
        }

        [Test]
        public void IndexShouldRedirectWhenThereAreUsers()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var user = new User();
            session.Store(user);
            session.SaveChanges();

            var controller = new SetupController();

            var result = controller.Index();

            result.AssertActionRedirect().ToController("authentication").ToAction("index");

            session.Delete(user);
            session.SaveChanges();
        }

        [Test]
        public void IndexShouldRedirectWhenUserIsSaved()
        {
            var crypto = M<ICryptographer>();
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<ICryptographer>()).Return(crypto);

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var controller = new SetupController();
            var model = new InitialSetupModel { Email = "john@doe.com", Password = "test", PasswordConfirm = "test" };

            var result = controller.Index(model);

            result.AssertActionRedirect().ToController("authentication").ToAction("index");

            session.Delete(session.Query<User>().First());
            session.SaveChanges();
        }
    }
}

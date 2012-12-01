using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Rhino.Mocks;
using Postback.Blog.Tests.Data;
using Raven.Client;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest : BaseRavenControllerTests
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

            SetupData(s => s.Store( new User() { Active = true }));
            SetupData(s => s.Store( new User() { Active = false }));
            SetupData(s => s.Store( new User() { Active = true }));

            var controller = new UserController();
            ViewResult result = null;
            ExecuteAction<UserController>(c => result = c.Index(null) as ViewResult);

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<UserViewModel>)));
        }

        [Test]
        public void DeleteWillNotDeleteYourself()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var userid = "abc123";
            var email = "john@doe.com";
            var user = new User {Id = userid, Email=email};
            SetupData(s => s.Store(user));

            var contextuser = M<IPrincipal>();
            var context = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            var identity = M<IIdentity>();
            identity.Expect(i => i.Name).Return(email);
            contextuser.Expect(u => u.Identity).Return(identity);
            context.Expect(h => h.Response).Return(response);
            context.Expect(h => h.User).Return(contextuser);

            var controller = new UserController();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
        }

        [Test]
        public void DeleteWillDeleteOtherUser()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var userid = "abc123";
            var email = "john@doe.com";
            var user = new User { Id = userid, Email = "polle@pap.com" };

            var contextuser = M<IPrincipal>();
            var context = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            var identity = M<IIdentity>();
            identity.Expect(i => i.Name).Return(email);
            contextuser.Expect(u => u.Identity).Return(identity);
            context.Expect(h => h.Response).Return(response);
            context.Expect(h => h.User).Return(contextuser);

            var controller = new UserController();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
        }
    }
}

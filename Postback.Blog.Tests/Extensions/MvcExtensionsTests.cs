using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Tests.Data;
using Raven.Client.Embedded;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Extensions
{
    [TestFixture]
    public class MvcExtensionTests : BaseRavenTest
    {
        private EmbeddableDocumentStore Store { get; set; }

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
        public void ShouldReturnControllerName()
        {
            var name = typeof(UserController).GetControllerName();
            Assert.That(name, Is.EqualTo("User"));
        }

        [Test]
        public void ShouldReturnActionName()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var session = Store.OpenSession();
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(session);

            var c = new UserController();
            var name = CaptureName<UserController>(x => x.Edit(new UserEditModel()));
            Assert.That(name, Is.EqualTo("Edit"));
        }

        public string CaptureName<TController>(Expression<Func<TController, object>> actionExpression)
        {
            string actionName = actionExpression.GetActionName();
            return actionName;
        }
    }
}

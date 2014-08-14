using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.Data;
using Raven.Client;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class AppInitControllerAttributeTests : BaseRavenControllerTests
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
        public void AdminControllersHaveAppInitAttributeExceptForSetupAndAuthenticationController()
        {
            var controllers = from t in typeof(User).Assembly.GetTypes()
                              where t.IsAbstract == false
                              where typeof(Controller).IsAssignableFrom(t)
                              where t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && !t.Name.Contains("Setup") && !t.Name.Contains("Authentication")
                              where t.Namespace.Contains("Admin")
                              select t;

            foreach(var controller in controllers)
            {
                Assert.That(Attribute.GetCustomAttribute(controller, typeof(AuthorizeAttribute)), Is.Not.Null, controller + " has not Authorize attribute");
            }
        }

        [Test]
        public void AuthenticationControllerHasAppInitAttribute()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<IDocumentSession>()).Return(Store.OpenSession());
            Assert.That(Attribute.GetCustomAttribute(typeof(AuthenticationController), typeof(AppInitAttribute)), Is.Not.Null);

            locator.VerifyAllExpectations();
        }
    }
}

using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class AppInitControllerAttributeTests
    {
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
            Assert.That(Attribute.GetCustomAttribute(typeof(AuthenticationController), typeof(AppInitAttribute)),Is.Not.Null);
        }
    }
}

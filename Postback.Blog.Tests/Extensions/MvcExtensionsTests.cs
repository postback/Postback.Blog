using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Areas.Admin.Models;

namespace Postback.Blog.Tests.Extensions
{
    [TestFixture]
    public class MvcExtensionTests : BaseTest
    {
        [Test]
        public void ShouldReturnControllerName()
        {
            var name = typeof(UserController).GetControllerName();
            Assert.That(name, Is.EqualTo("User"));
        }

        [Test]
        public void ShouldReturnActionName()
        {
            var c = new UserController(M<IPersistenceSession>());
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

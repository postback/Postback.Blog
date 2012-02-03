using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Norm;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Controllers;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest:BaseTest
    {
        [Test]
        public void IndexShouldReturnView()
        {
            var controller = new UserController(M<IPersistenceSession>());

            var result = controller.Index(null) as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<UserViewModel>)));
        }

        [Test]
        public void DeleteWillNotDeleteYourself()
        {
            var userid = new ObjectId("abc123");
            var email = "john@doe.com";
            var user = new User {Id = userid, Email=email};

            var contextuser = new Mock<IPrincipal>();
            var context = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.Name).Returns(email);
            contextuser.Setup(u => u.Identity).Returns(identity.Object);
            context.Setup(h => h.Response).Returns(response.Object);
            context.Setup(h => h.User).Returns(contextuser.Object);

            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            var controller = new UserController(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
            session.Verify(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>()), Times.Once());
            session.Verify(s => s.Delete<User>(It.IsAny<User>()), Times.Never());
        }

        [Test]
        public void DeleteWillDeleteOtherUser()
        {
            var userid = new ObjectId("abc123");
            var email = "john@doe.com";
            var user = new User { Id = userid, Email = "polle@pap.com" };

            var contextuser = new Mock<IPrincipal>();
            var context = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.Name).Returns(email);
            contextuser.Setup(u => u.Identity).Returns(identity.Object);
            context.Setup(h => h.Response).Returns(response.Object);
            context.Setup(h => h.User).Returns(contextuser.Object);

            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            var controller = new UserController(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
            session.Verify(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>()), Times.Once());
            session.Verify(s => s.Delete<User>(It.IsAny<User>()), Times.Once());
        }
    }
}

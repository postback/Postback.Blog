using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Areas.Admin.Models;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests : BaseTest
    {
        [Test]
        public void IndexShouldReturnViewResultWithAuthenticationModel()
        {
            var controller = new AuthenticationController(M<ICryptographer>(),M<IPersistenceSession>(),M<IAuth>(),M<IMessagingService>());
            var result = controller.Index();
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));

            var viewresult = (ViewResult) result;
            Assert.That(viewresult.ViewData.Model, Is.Not.Null);
            Assert.That(viewresult.ViewData.Model, Is.InstanceOf(typeof(AuthenticationModel)));
        }

        [Test]
        public void IndexActionAuthenticatesRedirectsWhenAuthenticationModelIsValid()
        {
            var email = "john@doe.com";
            var salt = "salt";
            var pass = "pass";

            var user = new Mock<User>();
            user.Object.Email = email;
            user.Object.PasswordSalt = salt;
            user.Object.PasswordHashed = "hashedpassword";
            
            var crypto = new Mock<ICryptographer>();
            crypto.Setup(c => c.GetPasswordHash(pass, salt)).Returns("hashedpassword");

            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(user.Object);

            var auth = new Mock<IAuth>();

            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();

            request.Setup(c => c.QueryString).Returns(new NameValueCollection());
            context.Setup(c => c.Request).Returns(request.Object);

            var controller = new AuthenticationController(crypto.Object, session.Object,auth.Object,M<IMessagingService>());
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            
            var model = Mock.Of<AuthenticationModel>();
            model.Email = email;
            model.Password = pass;
            
            var result = controller.Index(model);

            result.AssertActionRedirect().ToController("dashboard").ToAction("index");

            session.Verify(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>()), Times.Once());
            crypto.Verify(c => c.GetPasswordHash(pass, salt), Times.Once());
            auth.Verify(a => a.DoAuth(email, true), Times.Once());

        }

        [Test]
        public void IndexActionAuthenticatesRedirectsToQueryStringParameterWhenAuthenticationModelIsValid()
        {
            var user = new Mock<User>();
            user.Object.Email = string.Empty;
            user.Object.PasswordSalt = string.Empty;
            user.Object.PasswordHashed = "hashedpassword";

            var crypto = new Mock<ICryptographer>();
            crypto.Setup(c => c.GetPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns("hashedpassword");

            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.Single<User>(It.IsAny<Expression<Func<User, bool>>>())).Returns(user.Object);

            var auth = new Mock<IAuth>();

            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();

            var qstrings = new NameValueCollection();
            qstrings.Add("ReturnUrl", "/somepagetoredirecto");
            request.Setup(c => c.QueryString).Returns(qstrings);
            context.Setup(c => c.Request).Returns(request.Object);

            var controller = new AuthenticationController(crypto.Object, session.Object, auth.Object, M<IMessagingService>());
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var model = Mock.Of<AuthenticationModel>();
            model.Email = "e";
            model.Password = "s";

            var result = controller.Index(model);

            Assert.That(result, Is.InstanceOfType(typeof (RedirectResult)));
            Assert.That(((RedirectResult) result).Url, Is.EqualTo("/somepagetoredirecto"));

        }

        [Test]
        public void IndexActionReturnsToViewWhenAuthenticationModelIsInValid()
        {
            var controller = new AuthenticationController(M<ICryptographer>(), M<IPersistenceSession>(), M<IAuth>(), M<IMessagingService>());
            var result = controller.Index(Mock.Of<AuthenticationModel>());

            var viewresult = result.AssertViewRendered();
            Assert.That(viewresult.ViewData.Model, Is.Not.Null);
            Assert.That(viewresult.ViewData.Model, Is.InstanceOf(typeof(AuthenticationModel)));
        }
    }
}

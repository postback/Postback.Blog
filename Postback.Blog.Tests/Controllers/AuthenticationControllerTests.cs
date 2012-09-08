using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Areas.Admin.Models;
using Rhino.Mocks;

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

            var user = M<User>();
            user.Email = email;
            user.PasswordSalt = salt;
            user.PasswordHashed = "hashedpassword";
            
            var crypto = M<ICryptographer>();
            crypto.Expect(c => c.GetPasswordHash(pass, salt)).Return("hashedpassword").Repeat.Once();

            var session = M<IPersistenceSession>();
            session.Expect(s => s.Single<User>(Arg<Expression<Func<User, bool>>>.Is.Anything)).Return(user).Repeat.Once();

            var auth = M<IAuth>();
            auth.Expect(a => a.DoAuth(email, true)).Repeat.Once();

            var request = M<HttpRequestBase>();
            var context = M<HttpContextBase>();

            request.Expect(c => c.QueryString).Return(new NameValueCollection());
            context.Expect(c => c.Request).Return(request);

            var controller = new AuthenticationController(crypto, session,auth,M<IMessagingService>());
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
            
            var model = M<AuthenticationModel>();
            model.Email = email;
            model.Password = pass;
            
            var result = controller.Index(model);

            result.AssertActionRedirect().ToController("dashboard").ToAction("index");

            session.VerifyAllExpectations();
            crypto.VerifyAllExpectations();
            auth.VerifyAllExpectations();

        }

        [Test]
        public void IndexActionAuthenticatesRedirectsToQueryStringParameterWhenAuthenticationModelIsValid()
        {
            var user = M<User>();
            user.Email = string.Empty;
            user.PasswordSalt = string.Empty;
            user.PasswordHashed = "hashedpassword";

            var crypto = M<ICryptographer>();
            crypto.Expect(c => c.GetPasswordHash(Arg<string>.Is.Anything, (Arg<string>.Is.Anything))).Return("hashedpassword");

            var session = M<IPersistenceSession>();
            session.Expect(s => s.Single<User>(Arg<Expression<Func<User, bool>>>.Is.Anything)).Return(user);

            var auth = M<IAuth>();

            var request = M<HttpRequestBase>();
            var context = M<HttpContextBase>();

            var qstrings = new NameValueCollection();
            qstrings.Add("ReturnUrl", "/somepagetoredirecto");
            request.Expect(c => c.QueryString).Return(qstrings);
            context.Expect(c => c.Request).Return(request);

            var controller = new AuthenticationController(crypto, session, auth, M<IMessagingService>());
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var model = M<AuthenticationModel>();
            model.Email = "e";
            model.Password = "s";

            var result = controller.Index(model);

            Assert.That(result, Is.InstanceOfType(typeof (RedirectResult)));
            Assert.That(((RedirectResult) result).Url, Is.EqualTo("/somepagetoredirecto"));

        }

        [Test]
        public void IndexActionReturnToViewWhenAuthenticationModelIsInValid()
        {
            var controller = new AuthenticationController(M<ICryptographer>(), M<IPersistenceSession>(), M<IAuth>(), M<IMessagingService>());
            var result = controller.Index(M<AuthenticationModel>());

            var viewresult = result.AssertViewRendered();
            Assert.That(viewresult.ViewData.Model, Is.Not.Null);
            Assert.That(viewresult.ViewData.Model, Is.InstanceOf(typeof(AuthenticationModel)));
        }
    }
}

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

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest:BaseTest
    {
        [Test]
        public void IndexShouldReturnView()
        {
            var users = new List<User> { new User() { Active = true }, new User() { Active = false }, new User() { Active = true} };
            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<User>()).Return(users.AsQueryable()).Repeat.Once();


            var controller = new UserController(session);

            var result = controller.Index(null) as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(IList<UserViewModel>)));
            session.VerifyAllExpectations();
        }

        [Test]
        public void DeleteWillNotDeleteYourself()
        {
            var userid = "abc123";
            var email = "john@doe.com";
            var user = new User {Id = userid, Email=email};

            var contextuser = M<IPrincipal>();
            var context = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            var identity = M<IIdentity>();
            identity.Expect(i => i.Name).Return(email);
            contextuser.Expect(u => u.Identity).Return(identity);
            context.Expect(h => h.Response).Return(response);
            context.Expect(h => h.User).Return(contextuser);

            var session = M<IPersistenceSession>();
            session.Expect(s => s.FindOne<User>(Arg<Expression<Func<User, bool>>>.Is.Anything)).Return(user).Repeat.Once();
            session.Expect(s => s.Delete<User>(Arg<User>.Is.Anything)).Repeat.Never();

            var controller = new UserController(session);
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
            session.VerifyAllExpectations();
        }

        [Test]
        public void DeleteWillDeleteOtherUser()
        {
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

            var session = M<IPersistenceSession>();
            session.Expect(s => s.FindOne<User>(Arg<Expression<Func<User, bool>>>.Is.Anything)).Return(user).Repeat.Once();
            session.Expect(s => s.Delete<User>(Arg<User>.Is.Anything)).Repeat.Once();

            var controller = new UserController(session);
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var result = controller.Delete(userid) as ViewResult;
            session.VerifyAllExpectations();
        }
    }
}

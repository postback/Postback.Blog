using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Rhino.Mocks;
using StructureMap;

namespace Postback.Blog.Tests.Attributes
{
    [TestFixture]
    public class AppInitTests : BaseTest
    {
        [Test]
        public void ShouldRedirectToSetupWhenNoUsersYet()
        {
            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<User>()).Return(new List<User>().AsQueryable()).Repeat.Once();
            ObjectFactory.Inject(typeof(IPersistenceSession), session);

            var user = M<IPrincipal>();
            var httpContext = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            response.Expect(r => r.Redirect("/admin/setup", true)).Repeat.Once();
            user.Expect(u => u.Identity).Return(M<IIdentity>());
            httpContext.Expect(h => h.Response).Return(response);
            httpContext.Expect(h => h.User).Return(user);
            var controller = M<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, M<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            session.VerifyAllExpectations();
            response.VerifyAllExpectations();
        }

        [Test]
        public void ShouldNotRedirectToSetupWhenThereAreUsers()
        {
            var users = new List<User>();
            users.Add(new User());

            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<User>()).Return(users.AsQueryable()).Repeat.Once();
            ObjectFactory.Inject(typeof(IPersistenceSession), session);

            var user = M<IPrincipal>();
            var httpContext = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            response.Expect(r => r.Redirect("/admin/setup", true)).Repeat.Never();
            user.Expect(u => u.Identity).Return(M<IIdentity>());
            httpContext.Expect(h => h.Response).Return(response);
            httpContext.Expect(h => h.User).Return(user);
            var controller = M<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, M<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            response.VerifyAllExpectations();
        }

        [Test]
        public void ShouldNotCheckRepoWhenUserIsAuthenticated()
        {
            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<User>()).Return(new List<User>().AsQueryable()).Repeat.Never();
            ObjectFactory.Inject(typeof(IPersistenceSession), session);

            var user = M<IPrincipal>();
            var identity = M<IIdentity>();
            var httpContext = M<HttpContextBase>();
            var response = M<HttpResponseBase>();
            identity.Expect(i => i.IsAuthenticated).Return(true).Repeat.AtLeastOnce();
            user.Expect(u => u.Identity).Return(identity);
            httpContext.Expect(h => h.Response).Return(response);
            httpContext.Expect(h => h.User).Return(user);
            var controller = M<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, M<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            session.VerifyAllExpectations();
            identity.VerifyAllExpectations();
        }
    }
}

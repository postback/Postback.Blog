using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using StructureMap;

namespace Postback.Blog.Tests.Attributes
{
    [TestFixture]
    public class AppInitTests
    {
        [Test]
        public void ShouldRedirectToSetupWhenNoUsersYet()
        {
            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.All<User>()).Returns(new List<User>().AsQueryable());
            ObjectFactory.Inject(typeof(IPersistenceSession), session.Object);

            var user = new Mock<IPrincipal>();
            var httpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            user.Setup(u => u.Identity).Returns(Mock.Of<IIdentity>());
            httpContext.Setup(h => h.Response).Returns(response.Object);
            httpContext.Setup(h => h.User).Returns(user.Object);
            var controller = Mock.Of<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, Mock.Of<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            session.Verify(s => s.All<User>(), Times.Once());
            response.Verify(r => r.Redirect("/admin/setup",true), Times.Once());
        }

        [Test]
        public void ShouldNotRedirectToSetupWhenThereAreUsers()
        {
            var users = new List<User>();
            users.Add(new User());

            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.All<User>()).Returns(users.AsQueryable());
            ObjectFactory.Inject(typeof(IPersistenceSession), session.Object);

            var user = new Mock<IPrincipal>();
            var httpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            user.Setup(u => u.Identity).Returns(Mock.Of<IIdentity>());
            httpContext.Setup(h => h.Response).Returns(response.Object);
            httpContext.Setup(h => h.User).Returns(user.Object);
            var controller = Mock.Of<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, Mock.Of<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            response.Verify(r => r.Redirect("/admin/setup", true), Times.Never());
        }

        [Test]
        public void ShouldNotCheckRepoWhenUserIsAuthenticated()
        {
            var session = new Mock<IPersistenceSession>();
            session.Setup(s => s.All<User>()).Returns(new List<User>().AsQueryable());
            ObjectFactory.Inject(typeof(IPersistenceSession), session.Object);

            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            var httpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            identity.Setup(i => i.IsAuthenticated).Returns(true);
            user.Setup(u => u.Identity).Returns(identity.Object);
            httpContext.Setup(h => h.Response).Returns(response.Object);
            httpContext.Setup(h => h.User).Returns(user.Object);
            var controller = Mock.Of<ControllerBase>();

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            var filterContext = new ActionExecutingContext(controllerContext, Mock.Of<ActionDescriptor>(), new Dictionary<string, object>());
            var attribute = new AppInitAttribute();

            attribute.OnActionExecuting(filterContext);

            session.Verify(s => s.All<User>(), Times.Never());
            identity.Verify(i => i.IsAuthenticated, Times.AtLeastOnce());
        }
    }
}

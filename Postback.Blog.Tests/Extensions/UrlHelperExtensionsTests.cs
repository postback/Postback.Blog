using NUnit.Framework;
using NBehave.Spec.NUnit;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Postback.Blog.Tests.Extensions
{
    [TestFixture]
    public class UrlHelperExtensionsTests : BaseTest
    {
        [Test]
        public void ShouldConvertToLower()
        {
            var context = M<RequestContext>();
            var routedata = new RouteData();
            routedata.Values.Add("controller", "users");
            routedata.Values.Add("action", "delete");
            context.Expect(c => c.RouteData).Return(routedata);
            new UrlHelper(context).Is("USERS", "DELETE").ShouldBeTrue();
        }

        [Test]
        public void ShouldCheck()
        {
            var context = M<RequestContext>();
            var routedata = new RouteData();
            routedata.Values.Add("controller", "users");
            routedata.Values.Add("action", "delete");
            context.Expect(c => c.RouteData).Return(routedata);
            new UrlHelper(context).Is("users", "delete").ShouldBeTrue();
        }

        [Test]
        public void ControllerMismatch()
        {
            var context = M<RequestContext>();
            var routedata = new RouteData();
            routedata.Values.Add("controller", "users");
            routedata.Values.Add("action", "delete");
            context.Expect(c => c.RouteData).Return(routedata);
            new UrlHelper(context).Is("posts", "delete").ShouldBeFalse();
        }

        [Test]
        public void ActionMismatch()
        {
            var context = M<RequestContext>();
            var routedata = new RouteData();
            routedata.Values.Add("controller", "users");
            routedata.Values.Add("action", "delete");
            context.Expect(c => c.RouteData).Return(routedata);
            new UrlHelper(context).Is("users", "index").ShouldBeFalse();
        }
    }
}

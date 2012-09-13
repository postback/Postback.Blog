using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Security;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Security
{
    [TestFixture]
    public class WebSecurityContextTests : BaseTest
    {
        [Test]
        public void CanConstruct()
        {
            var httpContext = S<HttpContextBase>();

            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<HttpContextBase>()).Return(httpContext);

            var context = new WebSecurityContext();
            locator.VerifyAllExpectations();
        }

        [Test]
        public void ReturnsIsAuthenticated()
        {
            var httpContext = S<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(S<HttpRequestBase>());
            httpContext.Request.Expect(x => x.IsAuthenticated).Return(false);

            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetInstance<HttpContextBase>()).Return(httpContext);

            var context = new WebSecurityContext();
            locator.VerifyAllExpectations();
            context.IsAuthenticated().ShouldBeFalse();
        }

        [Test]
        public void ReturnsCurrentIdentityAndCurrentUser()
        {
            var locator = S<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var ticket = new FormsAuthenticationTicket(1, "dummy", SystemTime.Now(), SystemTime.Now().AddHours(1), false, null);

            var principal = M<IPrincipal>();

            var httpContext = M<HttpContextBase>();
            httpContext.Stub(x => x.User).Return(principal);

            locator.Expect(l => l.GetInstance<HttpContextBase>()).Return(httpContext).Repeat.Once(); ;

            var context = new WebSecurityContext();

            //Assert
            var currentUser = context.CurrentUser;
            currentUser.ShouldNotBeNull();

            context.CurrentUser.ShouldEqual(principal);
        }
    }
}

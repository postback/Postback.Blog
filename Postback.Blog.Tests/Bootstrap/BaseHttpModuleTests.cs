using System;
using System.Web;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Bootstrap;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Bootstrap
{
    [TestFixture]
    public class BaseHttpModuleTests : BaseTest
    {
        private bool BeginRequestVerified = false;
        private bool EndRequestVerified = false;
        private bool ErrorRequestVerified = false;
        private bool PostAuthRequestVerified = false;

        [Test]
        public void InitModule()
        {
            //Arrange
            var mocks = new MockRepository();
            var module = new DummyModule();
            var app = mocks.StrictMock<HttpApplication>();
            With.Mocks(mocks).Expecting(() => app.BeginRequest += new DelegateContainer().BeginDummy).Verify(() => MethodThatVerifiesBeginRequest(app));
            With.Mocks(mocks).Expecting(() => app.EndRequest += new DelegateContainer().EndDummy).Verify(() => MethodThatVerifiesEndRequest(app));
            With.Mocks(mocks).Expecting(() => app.Error += new DelegateContainer().ErrorDummy).Verify(() => MethodThatVerifiesError(app));
            With.Mocks(mocks).Expecting(() => app.PostAuthenticateRequest += new DelegateContainer().PostAuthDummy).Verify(() => MethodThatVerifiesPostAuth(app));

            //Act
            module.Init(app);

            //Assert
            module.ShouldNotBeNull();
            BeginRequestVerified.ShouldBeTrue();
            EndRequestVerified.ShouldBeTrue();
            ErrorRequestVerified.ShouldBeTrue();
            PostAuthRequestVerified.ShouldBeTrue();
        }

        public void MethodThatVerifiesBeginRequest(HttpApplication app)
        {
            BeginRequestVerified = true;
        }

        public void MethodThatVerifiesEndRequest(HttpApplication app)
        {
            EndRequestVerified = true;
        }

        public void MethodThatVerifiesError(HttpApplication app)
        {
            ErrorRequestVerified = true;
        }

        public void MethodThatVerifiesPostAuth(HttpApplication app)
        {
            PostAuthRequestVerified = true;
        }

        public class DummyModule : BaseHttpModule
        {
        }

        public class DelegateContainer
        {
            public void BeginDummy(object sender, EventArgs e)
            {
            }

            public void EndDummy(object sender, EventArgs e)
            {
            }

            public void ErrorDummy(object sender, EventArgs e)
            {
            }

            public void PostAuthDummy(object sender, EventArgs e)
            {
            }
        }
    }
}

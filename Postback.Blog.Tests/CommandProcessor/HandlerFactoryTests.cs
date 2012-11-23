using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Postback.Blog.App.Messaging;
using Postback.Blog.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using NBehave.Spec.NUnit;

namespace Postback.Blog.Tests.CommandProcessor
{
    [TestFixture]
    public class HandlerFactoryTests : BaseTest
    {
        [Test]
        public void FactoryCallsLocator() {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            locator.Expect(l => l.GetAllInstances(typeof(Handler<String>))).Return(new List<Type>() { typeof(StringHandler) });


            var factory = new HandlerFactory();
            var result = factory.GetHandlers(typeof(String));

            result.ShouldNotBeNull();
            locator.VerifyAllExpectations();
        }

        public class StringHandler : Handler<String> {
            protected override ReturnValue Handle(string commandMessage)
            {
                throw new NotImplementedException();
            }
        }
    }
}

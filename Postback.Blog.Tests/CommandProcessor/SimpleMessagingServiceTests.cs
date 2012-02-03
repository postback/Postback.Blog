using System.Linq;
using NUnit.Framework;
using Postback.Blog.App.Messaging;
using Postback.Blog.App.Services;
using Postback.Blog.Models;
using StructureMap;

namespace Postback.Blog.Tests.CommandProcessor
{
    [TestFixture]
    public class SimpleMessagingServiceTests : BaseTest
    {
        [Test]
        public void TheSimpleMessagingServiceShouldProcessThisMessage()
        {
            ObjectFactory.Configure(cfg => cfg.Scan(x =>
                                                        {
                                                            x.TheCallingAssembly();
                                                            x.AssemblyContainingType<User>();
                                                            x.ConnectImplementationsToTypesClosing(typeof(Handler<>));

                                                        })
                );

            var factory = new HandlerFactory();
            var service = new SimpleMessagingService(factory);
            var result = service.Send(new TestMessage {Message = "foo"});


            Assert.That(result.Messages,Has.Count.EqualTo(0));
            Assert.That(result.ReturnItems[typeof(TestMessage)],Is.Not.Null);
            Assert.That(((TestMessage)result.ReturnItems[typeof(TestMessage)]).Message, Is.EqualTo("foo bar"));
        }

        public class TestMessage : IMessage
        {
            public string Message { get; set; }
        }


        public class TestMessageCommandHandler : Handler<TestMessage>
        {
            public bool wasExecuted;

            protected override ReturnValue Handle(TestMessage message)
            {
                wasExecuted = true;
                message.Message += " bar";
                return new ReturnValue { Type = typeof(TestMessage), Value = message };
            }
        }
    }
}
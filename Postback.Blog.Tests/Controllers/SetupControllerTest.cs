using System.Linq;
using System.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Areas.Admin.Models;
using System.Collections.Generic;
using StructureMap;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class SetupControllerTest : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            InjectMock<ICryptographer>();
        }

        [Test]
        public void IndexShouldReturnView()
        {
            var controller = new SetupController(M<IPersistenceSession>());

            var result = controller.Index() as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(InitialSetupModel)));
        }

        [Test]
        public void IndexShouldRedirectWhenThereAreUsers()
        {
            var session = new Mock<IPersistenceSession>();
            var users = new List<User>();
            users.Add(new User());
            session.Setup(s => s.All<User>()).Returns(users.AsQueryable());

            var controller = new SetupController(session.Object);

            var result = controller.Index();

            result.AssertActionRedirect().ToController("authentication").ToAction("index");
            session.Verify(s => s.All<User>(), Times.Once());
        }

        [Test]
        public void IndexShouldRedirectWhenUserIsSaved()
        {
            ObjectFactory.Inject<ICryptographer>(Mock.Of<ICryptographer>());

            var session = new Mock<IPersistenceSession>();
            var users = new List<User>();
            users.Add(new User());

            var controller = new SetupController(session.Object);
            var model = new InitialSetupModel { Email = "john@doe.com", Password = "test", PasswordConfirm = "test" };

            var result = controller.Index(model);

            result.AssertActionRedirect().ToController("authentication").ToAction("index");
            session.Verify(s => s.Add<User>(It.IsAny<User>()), Times.Once());
        }
    }
}

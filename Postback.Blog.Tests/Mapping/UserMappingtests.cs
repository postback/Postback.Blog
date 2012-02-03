using AutoMapper;
using Moq;
using NUnit.Framework;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using StructureMap;

namespace Postback.Blog.Tests.Mapping
{
    [TestFixture]
    public class UserMappingTests : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            var crypto = new Mock<ICryptographer>();
            crypto.Setup(c => c.CreateSalt()).Returns("salt");
            crypto.Setup(c => c.GetPasswordHash("somepassword","salt")).Returns("hashedpassword");
            ObjectFactory.Inject(typeof(ICryptographer), crypto.Object);
        }

        [Test]
        public void UserIsMappedFromInitialUserModelUsingConstructorWhenPasswordIsSet()
        {
            var model = new InitialSetupModel { Name = "Jon", Email = "jon@doe.com", Password = "somepassword" };
            var user = Mapper.Map<InitialSetupModel, User>(model);

            Assert.That(user.Name, Is.EqualTo(model.Name));
            Assert.That(user.Email, Is.EqualTo(model.Email));
            Assert.That(user.Active, Is.True);
            Assert.That(user.PasswordHashed, Is.Not.Null.And.Not.Empty);
            Assert.That(user.PasswordHashed, Is.EqualTo("hashedpassword"));
            Assert.That(user.PasswordSalt, Is.Not.Null.And.Not.Empty);
            Assert.That(user.PasswordSalt, Is.EqualTo("salt"));
        }

        [Test]
        public void UserIsMappedFromModelUsingConstructorWhenPasswordIsSet()
        {
            var model = new UserEditModel {Name = "Jon", Email = "jon@doe.com", Password = "somepassword"};
            var user = Mapper.Map<UserEditModel, User>(model);

            Assert.That(user.Name, Is.EqualTo(model.Name));
            Assert.That(user.Email, Is.EqualTo(model.Email));
            Assert.That(user.Active, Is.True);
            Assert.That(user.PasswordHashed, Is.Not.Null.And.Not.Empty);
            Assert.That(user.PasswordHashed, Is.EqualTo("hashedpassword"));
            Assert.That(user.PasswordSalt, Is.Not.Null.And.Not.Empty);
            Assert.That(user.PasswordSalt, Is.EqualTo("salt"));
        }

        [Test]
        public void UserIsMappedFromModelUsingConstructorButWithoutUpdatingPasswordWhenPasswordIsNotSet()
        {
            var model = new UserEditModel { Name = "Jon", Email = "jon@doe.com" };
            var user = Mapper.Map<UserEditModel, User>(model);

            Assert.That(user.Name, Is.EqualTo(model.Name));
            Assert.That(user.Email, Is.EqualTo(model.Email));
            Assert.That(user.Active, Is.True);
            Assert.That(user.PasswordHashed, Is.Null);
            Assert.That(user.PasswordSalt, Is.Null);
        }
    }
}

﻿using System.Linq;
using System.Web.Mvc;

using MvcContrib.TestHelper;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Areas.Admin.Models;
using System.Collections.Generic;
using Rhino.Mocks;
using StructureMap;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class SetupControllerTest : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            M<ICryptographer>();
        }

        [Test]
        public void IndexShouldReturnView()
        {
            var session = M<IPersistenceSession>();
            session.Expect(s => s.All<User>()).Return(new List<User>().AsQueryable()).Repeat.Once();
            var controller = new SetupController(session);

            var result = controller.Index() as ViewResult;

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf(typeof(InitialSetupModel)));

            session.VerifyAllExpectations();
        }

        [Test]
        public void IndexShouldRedirectWhenThereAreUsers()
        {
            var session = M<IPersistenceSession>();
            var users = new List<User>();
            users.Add(new User());
            session.Expect(s => s.All<User>()).Return(users.AsQueryable()).Repeat.Once();

            var controller = new SetupController(session);

            var result = controller.Index();

            result.AssertActionRedirect().ToController("authentication").ToAction("index");
            session.VerifyAllExpectations();
        }

        [Test]
        public void IndexShouldRedirectWhenUserIsSaved()
        {
            ObjectFactory.Inject<ICryptographer>(M<ICryptographer>());

            var session = M<IPersistenceSession>();
            session.Expect(s => s.Add<User>(Arg<User>.Is.Anything)).Repeat.Once();
            var users = new List<User>();
            users.Add(new User());

            var controller = new SetupController(session);
            var model = new InitialSetupModel { Email = "john@doe.com", Password = "test", PasswordConfirm = "test" };

            var result = controller.Index(model);

            result.AssertActionRedirect().ToController("authentication").ToAction("index");
            session.VerifyAllExpectations();
        }
    }
}

using Microsoft.Practices.ServiceLocation;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Mvc;
using Postback.Blog.Models;
using Postback.Blog.Tests.Data;
using Raven.Client;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Mvc
{
    [TestFixture]
    public class ConventionModelBinderProviderTests : BaseRavenTest
    {
        private IDocumentStore Store { get; set; }

        [TestFixtureSetUp]
        public void SetUp()
        {
            Store = NewStore();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        [Test]
        public void DoesNotLoadForNonContentOrNonEntity()
        {
            //Arrange
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var provider = new ConventionModelBinderProvider();

            //Act
            var binder = provider.GetBinder(typeof(FooBarClass));

            //Assert
            binder.ShouldBeNull();
        }

        [Test]
        public void LoadsForEntities()
        {
            //Arrange
            var locator = M<IServiceLocator>();
            locator.Expect(l => l.GetInstance(typeof(EntityModelBinder<BarEntity>))).Return(new EntityModelBinder<BarEntity>(Store.OpenSession())).Repeat.Once();
            ServiceLocator.SetLocatorProvider(() => locator);
            var provider = new ConventionModelBinderProvider();

            //Act
            var binder = provider.GetBinder(typeof(BarEntity));

            //Assert
            binder.ShouldBeInstanceOfType(typeof(EntityModelBinder<BarEntity>));
        }
    }

    public class BarEntity : Entity
    {
        public string BarProp { get; set; }
    }

    public class FooBarClass
    { }
}

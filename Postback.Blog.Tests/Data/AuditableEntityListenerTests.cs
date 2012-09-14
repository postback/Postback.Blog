using System;
using Microsoft.Practices.ServiceLocation;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Raven.Client.Embedded;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Data
{
    [TestFixture]
    public class AuditableEntityListenerTests : RavenBaseTest
    {
        private EmbeddableDocumentStore Store { get; set; }
        private DateTime created = new DateTime(2011, 2, 13, 8, 55, 13);
        private DateTime updated = new DateTime(2012, 3, 14, 9, 56, 14);
        private string user = "vincent";

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

        private string GetCurrentUser()
        {
            return user;
        }

        protected override void ModifyStore(EmbeddableDocumentStore documentStore)
        {
            documentStore.RegisterListener(new AuditableEntityListener(GetCurrentUser));
        }

        [Test]
        public void DoesNotAuditNonAuditable()
        {
            //Arrange
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(created).Repeat.Never();

            var entity = new DummyNonAuditedEntity();

            //Act
            using (var session = Store.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }

            //Assert
            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }

        [Test]
        public void WhenNoIdAlsoPopulatesCreatedInfo()
        {
            //Arrange
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(created).Repeat.Once();

            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock).Repeat.Once();

            var entity = new DummyAuditedEntity();

            //Act
            using (var session = Store.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }

            //Assert
            entity.Created.ShouldEqual(created);

            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }

        [Test]
        public void AuditedFieldsAreUpdates()
        {
            //Arrange
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);

            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(created);

            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            var entity = new DummyAuditedEntity();

            using (var session = Store.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }

            clock.BackToRecord(BackToRecordOptions.All);
            clock.Replay();
            clock.Expect(c => c.Now()).Return(updated);
            user = "homer";

            //Act
            using (var session = Store.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }

            //Assert
            entity.Created.ShouldEqual(created);

            entity.CreatedBy.ShouldEqual("vincent");

            //Reset
            user = "vincent";
        }
    }


    public class DummyAuditedEntity : AuditedEntity
    { }

    public class DummyNonAuditedEntity
    { }
}

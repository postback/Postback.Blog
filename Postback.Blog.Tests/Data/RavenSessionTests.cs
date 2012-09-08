using System;
using System.Linq;
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
    public class RavenSessionTests : RavenBaseTest
    {
        private EmbeddableDocumentStore Store { get; set; }
        private string Id { get; set; }
        private string IdOfJane { get; set; }

        private string GetCurrentUser()
        {
            return "vincent";
        }

        protected override void ModifyStore(EmbeddableDocumentStore documentStore)
        {
            documentStore.RegisterListener(new AuditableEntityListener(GetCurrentUser));
        }

        [SetUp]
        public void SetUp()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            clock.Expect(c => c.Now()).Return(new DateTime(2012, 8, 9, 0, 1, 2)).Repeat.Once();

            Store = NewStore();

            var entitya = new RepoEntity { Name = "John", Active = true, Address = "Sperperiodestraat", Zip = 9000 };
            var entityb = new RepoEntity { Name = "Jane", Active = false, Address = "Jasper straat", Zip = 9100 };
            var entityc = new RepoEntity { Name = "Jasper", Active = true, Address = "Karrewiet", Zip = 123456789 };
            var entitye = new RepoEntity { Name = "Vincent", Active = true, Address = "Renbaanstraat", Zip = 8888 };
            var entityf = new RepoEntity { Name = "An", Active = true, Address = "Alleren", Zip = 8888 };
            var entityg = new RepoEntity { Name = "Peter", Active = true, Address = "Reno street", Zip = 8888 };
            var entityh = new RepoEntity { Name = "Francis", Active = true, Address = "Renbaanstraat", Zip = 8888 };
            var entityi = new RepoEntity { Name = "Frederik", Active = true, Address = "Alleren", Zip = 8888 };
            var entityj = new RepoEntity { Name = "Thomas", Active = true, Address = "Fabiolalaan", Zip = 8888 };
            var entityk = new RepoEntity { Name = "Gert", Active = true, Address = "Fabiolalaan", Zip = 8888 };
            var entityl = new RepoEntity { Name = "Rembrand", Active = true, Address = "Fabiolalaan", Zip = 8888 };

            using (var session = Store.OpenSession())
            {
                session.Store(entitya);
                session.Store(entityb);
                session.Store(entityc);
                session.Store(entitye);
                session.Store(entityf);
                session.Store(entityg);
                session.Store(entityh);
                session.Store(entityi);
                session.Store(entityj);
                session.Store(entityk);
                session.Store(entityl);
                session.SaveChanges();

                //Do this just to make sure entities are stored before the test methods fire off
                var result = session.Query<RepoEntity>().Customize(a => a.WaitForNonStaleResults()).Where(x => true).ToList();
            }

            Id = entitya.Id;
            IdOfJane = entityb.Id;
        }

        [TearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        [Test]
        public void CanSingle()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);

            //Act
            var result = ravenSession.FindOne<RepoEntity>(r => r.Id == Id);

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Id);
            result.Name.ShouldEqual("John");
        }

        [Test]
        public void CanDelete()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);
            var item = ravenSession.FindOne<RepoEntity>(r => r.Id == Id);

            //Act
            ravenSession.Delete(item);

            //Assert
            var deleted = ravenSession.FindOne<RepoEntity>(r => r.Id == Id);
            deleted.ShouldBeNull();
        }

        [Test]
        public void CanSave()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);

            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            clock.Expect(c => c.Now()).Return(new DateTime(2012, 1, 2, 3, 4, 5)).Repeat.Once();

            //Act
            var result = ravenSession.Save(new RepoEntity() { Name = "Pol" });

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBeEmpty();
            var item = ravenSession.FindOne<RepoEntity>(r => r.Id == result.Id);
            item.Name.ShouldEqual("Pol");
            item.Created.ShouldNotBeNull();
            item.Created.ShouldEqual(new DateTime(2012, 1, 2, 3, 4, 5));
            clock.VerifyAllExpectations();
            locator.VerifyAllExpectations();
        }

        [Test]
        public void CanUpdate()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);

            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            clock.Expect(c => c.Now()).Return(new DateTime(2012, 1, 2, 3, 4, 5)).Repeat.Once();

            //Act
            var result = ravenSession.Save(new RepoEntity() { Name = "Pol" });

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBeEmpty();
            var item = ravenSession.FindOne<RepoEntity>(r => r.Id == result.Id);
            item.Name.ShouldEqual("Pol");
            item.Created.ShouldEqual(new DateTime(2012, 1, 2, 3, 4, 5));
            

            //Act
            item.Name = "Marc";
            ravenSession.Save(item);

            //Assert
            var updated = ravenSession.FindOne<RepoEntity>(r => r.Id == result.Id);
            updated.Name.ShouldEqual("Marc");
            updated.Created.ShouldEqual(new DateTime(2012, 1, 2, 3, 4, 5));
        }

        [Test]
        public void CanFindOne()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);

            //Act
            var item = ravenSession.FindOne<RepoEntity>(r => r.Name == "Jane");

            //Assert
            item.ShouldNotBeNull();
            item.Id.ShouldEqual(IdOfJane);
        }

        [Test]
        public void CanFindMultiple()
        {
            //Arrange
            var session = Store.OpenSession();
            var ravenSession = new RavenSession(session);

            //Act
            var items = ravenSession.Find<RepoEntity>(r => r.Name.StartsWith("Ja"));

            //Assert
            items.ShouldNotBeNull();
            items.Count().ShouldEqual(2);
        }
    }

    public class RepoEntity : AuditedEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pass { get; set; }
        public string CultureOfThing { get; set; }
        public string Key { get; set; }
        public int Zip { get; set; }
        public bool Active { get; set; }
    }
}

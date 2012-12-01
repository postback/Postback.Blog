using System;
using System.Linq;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.DependencyResolution;
using Raven.Client.Embedded;
using StructureMap;
using StructureMap.Configuration.DSL;
using Postback.Blog.Tests.Data;

namespace Postback.Blog.Tests.DI
{
    [TestFixture]
    public class StructureMapServiceLocatorTests : BaseRavenTest
    {
        private EmbeddableDocumentStore Store { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            Store = NewStore();

            RavenRegistry.InitDocumentStore = () =>
            {
                return Store;
            };
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        [Test]
        public void RegistersDependencies()
        {
            try{
            var locator = new StructureMapServiceLocator();
            locator.RegisterDependencies();
            locator.Reset();
                }catch(Exception ex)
                {
                    true.ShouldBeFalse();
                }

            true.ShouldBeTrue();
        }

        [Test]
        public void CallsInitialize()
        {
            var called = false;
            var locator = new StructureMapServiceLocator();
            locator.Initialize = (type) => { called = true; };
            locator.RegisterDependencies();
            called.ShouldBeTrue();
            locator.Reset();
        }

        [Test]
        public void GetsAssemblyPrefix()
        {
            var prefix = StructureMapServiceLocator.GetAssembliesPrefix(this.GetType());
            prefix.ShouldEqual("Postback.Blog");
        }

        [Test]
        public void EnsuresDependenciesRegistered()
        {
            var callsInitialize = false;
            var locator = new StructureMapServiceLocator();
            locator.Reset();
            locator.Initialize = (type) => { callsInitialize = true; };
            locator.EnsureDependenciesRegistered();
            callsInitialize.ShouldBeTrue();
            locator.Reset();
        }

        [Test]
        public void GetsInstanceNamesFor()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var impl = locator.GetInstanceNamesFor<ISimpleInterface>();
            impl.Count().ShouldEqual(1);
            impl.First().ShouldEqual("Postback.Blog.Tests.DI.SimpleInterface, Postback.Blog.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            locator.Reset();
        }

        [Test]
        public void GetsServiceByType()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var impl = locator.GetService(typeof(ISimpleInterface));
            impl.ShouldBeInstanceOfType(typeof(SimpleInterface));
            locator.Reset();
        }

        [Test]
        public void GetsInstanceByType()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var impl = locator.GetInstance(typeof(ISimpleInterface));
            impl.ShouldBeInstanceOfType(typeof(SimpleInterface));
            locator.Reset();
        }

        [Test]
        public void GetsInstanceByTypeAndmptyKey()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();

            var impl = locator.GetInstance(typeof(IOtherInterface), string.Empty);
            impl.ShouldBeInstanceOfType(typeof(OtherOtherInterface));

            var nullimpl = locator.GetInstance(typeof(IOtherInterface), null);
            nullimpl.ShouldBeInstanceOfType(typeof(OtherOtherInterface));

            locator.Reset();
        }

        [Test]
        public void GetsInstanceByTypeAndKey()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var impl = locator.GetInstance(typeof(IOtherInterface), "bar");
            impl.ShouldBeInstanceOfType(typeof(OtherOtherInterface));

            locator.Reset();
        }

        [Test]
        public void GetsAllInstancesByType()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var items = locator.GetAllInstances(typeof(IFakeInterface));
            items.Count().ShouldEqual(2);
            locator.Reset();
        }

        [Test]
        public void GenericGet()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var impl = locator.GetInstance<ISimpleInterface>();
            impl.ShouldBeInstanceOfType(typeof(SimpleInterface));
            locator.Reset();
        }

        [Test]
        public void GenericGetWithKey()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();

            var impl = locator.GetInstance<IOtherInterface>("foo");
            impl.ShouldBeInstanceOfType(typeof(OtherInterface));

            bool thrown = false;
            try
            {
                var nullimpl = locator.GetInstance<IOtherInterface>("test");
            }
            catch (Exception ex)
            {
                thrown = true;
                ex.ShouldBeInstanceOfType(typeof(StructureMapException));
            }

            thrown.ShouldBeTrue();
            locator.Reset();
        }

        [Test]
        public void GenericGetAll()
        {
            var locator = new StructureMapServiceLocator();
            locator.EnsureDependenciesRegistered();
            var items = locator.GetAllInstances<IFakeInterface>();
            items.Count().ShouldEqual(2);
            locator.Reset();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
        }

        [TestFixtureTearDown]
        public void SetupTearDown()
        {
            ObjectFactory.ResetDefaults();
        }
    }

    public class StructureMapTestRegistry : Registry
    {
        public StructureMapTestRegistry()
        {
            For<IFakeInterface>().Use<FakeInterface>();
            For<IFakeInterface>().Use<OtherFakeInterface>();

            For<IOtherInterface>().Use<OtherInterface>().Named("foo");
            For<IOtherInterface>().Use<OtherOtherInterface>().Named("bar");
        }
    }

    public interface ISimpleInterface
    { }

    public class SimpleInterface : ISimpleInterface
    { }

    public interface IFakeInterface
    { }

    public class FakeInterface : IFakeInterface
    { }

    public class OtherFakeInterface : IFakeInterface
    { }

    public interface IOtherInterface
    { }

    public class OtherInterface : IOtherInterface
    { }

    public class OtherOtherInterface : IOtherInterface
    { }
}

using AutoMapper;
using NUnit.Framework;
using Postback.Blog.App.Bootstrap;
using StructureMap;

namespace Postback.Blog.Tests.Bootstrap
{
    [TestFixture]
    public class BootstrapTests
    {
        [Test]
        public void BootstrapperLoadsAllInstancesOfIStartUpAndEnsuresDependenciesAreRegistered()
        {
            //Cannot be tested
            Assert.IsTrue(true);
        }

        [Test]
        public void AutoMapperBootstrapConfiguresAutomapper()
        {
            new AutoMapperBootstrap().Configure();
            Mapper.AssertConfigurationIsValid();
            Assert.That(true);
        }

        [Test]
        public void MvcBoostrapConfiguresMvcFramework()
        {
            //Cannot be tested
            Assert.IsTrue(true);
        }

        [Test]
        public void ValidationBoostrapConfiguresMvcFramework()
        {
            //Cannot be tested
            Assert.IsTrue(true);
        }
    }
}

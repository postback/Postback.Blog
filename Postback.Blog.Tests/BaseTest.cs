using System.Web.Mvc;
using AutoMapper;
using NUnit.Framework;
using Postback.Blog.App.DependencyResolution;
using Postback.Blog.App.Mapping;
using Rhino.Mocks;
using StructureMap;

namespace Postback.Blog.Tests
{
    [TestFixture]
    public class BaseTest
    {
        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());

            SetStructureMapDependencyResolver();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }

        /// <summary>
        /// Create a mock
        /// </summary>
        /// <typeparam name="T">Type to be mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>TMessage</returns>
        protected static T M<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateMock<T>(argumentsForConstructor);
        }

        /// <summary>
        /// Create a dynamic mock
        /// </summary>
        /// <typeparam name="T">Type to be mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>TMessage</returns>
        protected static T DM<T>(params object[] argumentsForConstructor) where T : class
        {
            var mocks = new MockRepository();
            return mocks.DynamicMock<T>(argumentsForConstructor);
        }

        /// <summary>
        /// Create a stub
        /// </summary>
        /// <typeparam name="T">Type to be stubbed</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>TMessage</returns>
        protected static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }

        public void SetStructureMapDependencyResolver()
        {
            DependencyResolver.SetResolver(new ServiceLocatorDependencyResolver());
        }
    }
}

using System.Web.Mvc;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Postback.Blog.App.DependencyResolution;
using Postback.Blog.App.Mapping;
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

        public static T M<T>() where T : class
        {
            return Mock.Of<T>();
        }

        public void InjectMock<T>() where T : class
        {
            ObjectFactory.Inject(typeof(T), M<T>());
        }

        public void SetStructureMapDependencyResolver()
        {
            var container = ObjectFactory.Container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
        }
    }
}

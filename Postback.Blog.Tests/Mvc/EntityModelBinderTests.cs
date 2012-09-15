using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.App.Data;
using Postback.Blog.App.Mvc;
using Postback.Blog.Models;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Mvc
{
    [TestFixture]
    public class EntityModelBinderTests : BaseTest
    {
        private IPersistenceSession persistenceSession;

        EntityModelBinder<Parent> entityModelBinder;
        ControllerContext controllerContext;
        HttpRequestBase request;

        Parent parent;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            parent = new Parent
            {
                Id = "vincent-1",
                Name = "Vincent from repository",
                Child = new Child
                {
                    Id = "jasper-6",
                    Name = "Jasper from repository"
                }
            };

            persistenceSession = M<IPersistenceSession>();
            persistenceSession.Expect(r => r.Get<Parent>(parent.Id)).Return(parent);
            persistenceSession.Expect(r => r.Get<Child>(parent.Child.Id)).Return(parent.Child);

            request = S<HttpRequestBase>();
            controllerContext = new ControllerContext
            {
                HttpContext = S<HttpContextBase>()
            };
            controllerContext.HttpContext.Stub(x => x.Request).Return(request);

            entityModelBinder = new EntityModelBinder<Parent>(persistenceSession);
            entityModelBinder.SetModelBinderDictionary(new ModelBinderDictionary { DefaultBinder = entityModelBinder });
        }

        [Test]
        public void GetChildFromSession()
        {
            var values = new NameValueCollection
            {
                { "Id", "vincent-1" },
                { "Name"    , "Parent from form" },
                { "Child.Id", "jasper-6" },
                { "Child.Name", "Child from form" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GenerateModelMetaData(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("nl-BE"))
            };

            var model = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            model.ShouldNotBeNull();
            model.Id.ShouldEqual("vincent-1");
            model.Name.ShouldEqual("Parent from form");
            model.ShouldBeTheSameAs(parent);

            model.Child.Id.ShouldEqual("jasper-6");
            model.Child.ShouldBeTheSameAs(model.Child);

            // Child.Name should be updated, although child entity is pulled from the repository
            model.Child.Name.ShouldEqual("Child from form");

            bindingContext.ModelState.ToAssert();
        }

        [Test]
        public void UpdateChildFromSessionIfIdIsNotGiven()
        {
            var values = new NameValueCollection
            {
                { "Id", "vincent-1" },
                { "Name", "Vincent" },
                { "Child.Name", "Peter" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GenerateModelMetaData(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("nl-BE"))
            };

            var model = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            model.ShouldNotBeNull();
            model.Id.ShouldEqual("vincent-1");
            model.Name.ShouldEqual("Vincent");
            model.ShouldBeTheSameAs(parent);

            model.Child.Id.ShouldEqual("jasper-6");
            model.Child.Name.ShouldEqual("Peter");
            model.Child.ShouldBeTheSameAs(parent.Child);

            bindingContext.ModelState.ToAssert();
        }

        [Test]
        public void GetChildIfChildIdIsMissing()
        {
            var values = new NameValueCollection
            {
                { "Id", "vincent-1" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GenerateModelMetaData(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("nl-BE"))
            };

            var bound = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            bound.ShouldNotBeNull();
            bound.Id.ShouldEqual("vincent-1");
            bound.Name.ShouldEqual("Vincent from repository");
            bound.ShouldBeTheSameAs(parent);

            bound.Child.Id.ShouldEqual("jasper-6");
            bound.Child.ShouldBeTheSameAs(parent.Child);

            bindingContext.ModelState.ToAssert();
        }

        [Test]
        public void DontGetParentFromSessionIfNoIdIsGiven()
        {
            var values = new NameValueCollection
            {
                { "Name", "An" },
                { "Child.Name", "Mathijs" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GenerateModelMetaData(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var bound = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            bound.ShouldNotBeNull();
            bound.Id.ShouldBeNull();
            bound.Name.ShouldEqual("An");
            bound.ShouldNotBeTheSameAs(parent);

            bound.Child.Id.ShouldBeNull();
            bound.Child.Name.ShouldEqual("Mathijs");
            bound.Child.ShouldNotBeTheSameAs(parent.Child);

            bindingContext.ModelState.ToAssert();
        }

        private static ModelMetadata GenerateModelMetaData(Func<object> modelAccessor)
        {
            return new ModelMetadata(
                new DataAnnotationsModelMetadataProvider(),
                null,
                modelAccessor,
                typeof(Parent),
                null);
        }

        
    }

    public class Parent : Entity
    {
        public string Name { get; set; }
        public Child Child { get; set; }
    }

    public class Child : Entity
    {
        public string Name { get; set; }
    }
}

public static class ModelStateDictionaryExtensions
{
    public static void ToAssert(this ModelStateDictionary modelState)
    {
        foreach (var key in modelState.Keys)
        {
            foreach (var error in modelState[key].Errors)
            {
                Console.WriteLine(error.ErrorMessage);
                Assert.Fail("Model binding errors occured");
            }
        }
    }
}
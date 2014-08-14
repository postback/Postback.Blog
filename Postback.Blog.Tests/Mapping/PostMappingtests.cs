using System;
using AutoMapper;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Microsoft.Practices.ServiceLocation;
using Rhino.Mocks;

namespace Postback.Blog.Tests.Mapping
{
    [TestFixture]
    public class PostMappingTests : BaseTest
    {
        [Test]
        public void PostIsMappedFromModelUsingConstructor()
        {
            var model = new PostEditModel {Title = "The title", Tags = "music,movie", Body = "The body", Created=DateTime.Now,CreatedBy = "the author"};
            var post = Mapper.Map<PostEditModel, Post>(model);

            Assert.That(post.Title, Is.EqualTo(model.Title));
            Assert.That(post.Body, Is.EqualTo(model.Body));
            Assert.That(post.Comments, Is.Empty);
            Assert.That(post.Created, Is.EqualTo(model.Created));
            Assert.That(post.CreatedBy, Is.EqualTo(model.CreatedBy));
            Assert.That(post.Uri, Is.EqualTo(model.Title.ToUri()));
            Assert.That(post.Tags, Has.Count.EqualTo(2));
        }

        [Test]
        public void PostIsMappedFromModelUsingConstructorButWithoutUpdatingPasswordWhenPasswordIsNotSet()
        {
            var model = new PostEditModel { Title = "Post title", Tags = "music , movie , multiple words ; other separator" };
            var post = Mapper.Map<PostEditModel, Post>(model);

            Assert.That(post.Tags, Has.Count.EqualTo(4));
            Assert.That(post.Tags[0].Name, Is.EqualTo("music"));
            Assert.That(post.Tags[2].Name, Is.EqualTo("multiple words"));
        }

        [Test]
        public void PostIsMappedFromModelWithPublishingDateSameAsCreatedWhenNotSet()
        {
            var model = new PostEditModel { Title = "Post title", Tags = "music , movie , multiple words ; other separator", Created = DateTime.Now };
            var post = Mapper.Map<PostEditModel, Post>(model);

            Assert.That(post.Created, Is.EqualTo(model.Created));
            Assert.That(post.PublishFrom, Is.EqualTo(model.Created));
        }

        [Test]
        public void PostIsMappedFromModelWithPublishingDateWhenSet()
        {
            var model = new PostEditModel { Title = "Post title", Tags = "music , movie , multiple words ; other separator", Created = new DateTime(2009, 8, 7, 6, 5, 4, DateTimeKind.Local), PublishFrom = new DateTime(2009, 1, 2, 3, 4, 5, DateTimeKind.Local) };
            var post = Mapper.Map<PostEditModel, Post>(model);

            Assert.That(post.Created, Is.EqualTo(model.Created));
            Assert.That(post.PublishFrom, Is.EqualTo(model.PublishFrom));
            Assert.That(post.PublishFrom, Is.Not.EqualTo(model.Created));
        }

        [Test]
        public void MapsToViewModelWithSmartDates()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(DateTime.Now).Repeat.Once();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock).Repeat.Once();

            var post = new Post { Id = "some-id", Uri = "some-uri", Title = "Post title", Active = true, Created = new DateTime(2009, 8, 7, 6, 5, 4, DateTimeKind.Local), PublishFrom = new DateTime(2009, 1, 2, 3, 4, 5, DateTimeKind.Local) };
            var model = Mapper.Map<Post, PostViewModel >(post);

            model.Active.ShouldBeTrue();
            model.Title.ShouldEqual("Post title");
            model.Id.ShouldEqual(post.Id);
            model.Uri.ShouldEqual(post.Uri);
            model.Created.ShouldEqual("07 aug 2009 06:05");
            model.PublishFrom.ShouldEqual("02 jan 2009 03:04");
        }

        [Test]
        public void InActiveIsNotPublished()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(DateTime.Now).Repeat.Never();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock).Repeat.Never();

            var post = new Post() { Active = false };
            var model = Mapper.Map<Post,PostViewModel>(post);

            model.IsPublished.ShouldBeFalse();
            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }

        [Test]
        public void ActiveAndFutureIsNotPublished()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(DateTime.Now);
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            var post = new Post() { Active = true,PublishFrom=DateTime.Now.AddDays(3) };
            var model = Mapper.Map<Post, PostViewModel>(post);

            model.IsPublished.ShouldBeFalse();
            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }

        [Test]
        public void ActiveAndPastIsPublished()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(DateTime.Now);
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock);

            var post = new Post() { Active = true, PublishFrom = DateTime.Now.AddDays(-3) };
            var model = Mapper.Map<Post, PostViewModel>(post);

            model.IsPublished.ShouldBeTrue();
            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }

        [Test]
        public void ActiveAndNoPublishDate()
        {
            var locator = M<IServiceLocator>();
            ServiceLocator.SetLocatorProvider(() => locator);
            var clock = M<ISystemClock>();
            clock.Expect(c => c.Now()).Return(DateTime.Now).Repeat.Never();
            locator.Expect(l => l.GetInstance<ISystemClock>()).Return(clock).Repeat.Never();

            var post = new Post() { Active = true, PublishFrom = null };
            var model = Mapper.Map<Post, PostViewModel>(post);

            model.IsPublished.ShouldBeTrue();
            locator.VerifyAllExpectations();
            clock.VerifyAllExpectations();
        }
    }
}

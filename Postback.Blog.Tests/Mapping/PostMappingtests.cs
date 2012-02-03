using AutoMapper;
using NUnit.Framework;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Mapping
{
    [TestFixture]
    public class PostMappingTests : BaseTest
    {
        [Test]
        public void PostIsMappedFromModelUsingConstructor()
        {
            var model = new PostEditModel {Title = "The title", Tags = "music,movie", Body = "The body"};
            var post = Mapper.Map<PostEditModel, Post>(model);

            Assert.That(post.Title, Is.EqualTo(model.Title));
            Assert.That(post.Body, Is.EqualTo(model.Body));
            Assert.That(post.Comments, Is.Empty);
            Assert.That(post.Created, Is.Not.Null);
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
    }
}

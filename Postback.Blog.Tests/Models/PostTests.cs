using NUnit.Framework;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Models
{
    [TestFixture]
    public class PostTests
    {
        [Test]
        public void DefaultConstructorMakesListsNotEmpty()
        {
            var post = new Post();
            Assert.That(post.Tags, Is.Not.Null);
            Assert.That(post.Comments, Is.Not.Null);
        }

        [Test]
        public void ConstructorWithNameSetsUri()
        {
            var post = new Post("The long title");
            Assert.That(post.Uri, Is.EqualTo(post.Title.ToUri()));
            Assert.That(post.Tags, Is.Not.Null);
            Assert.That(post.Comments, Is.Not.Null);
        }
    }
}

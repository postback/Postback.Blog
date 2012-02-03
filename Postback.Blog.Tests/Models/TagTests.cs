using NUnit.Framework;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Models
{
    [TestFixture]
    public class TagTests
    {
        [Test]
        public void ConstructorWithNameSetsUri()
        {
            var tag = new Tag("A name of a band");
            Assert.That(tag.Uri, Is.EqualTo(tag.Name.ToUri()));
        }
    }
}

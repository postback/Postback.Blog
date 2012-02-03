using NUnit.Framework;
using Postback.Blog.App.Services;

namespace Postback.Blog.Tests.Services
{
    [TestFixture]
    public class CryptographerTests
    {
        [Test]
        public void ShouldHashPassword()
        {
            ICryptographer cryptographer = new Cryptographer();

            var hash = cryptographer.GetPasswordHash("pass", "salt");
            Assert.That(hash, Is.EqualTo("cGS5SkKWZQ/PWvQvJaQfXnAAD7FAuqVmI8302iorwl8NtRaPV7Hr2WsQxAc3wacyhZByZfYZrIWygc0vxfQgfQ=="));
        }

        [Test]
        public void ShouldCreateSalt()
        {
            ICryptographer cryptographer = new Cryptographer();

            var salt = cryptographer.CreateSalt();
            Assert.That(salt.Length, Is.EqualTo(88));
        }
    }
}

using System;
using NUnit.Framework;

namespace Postback.Blog.Tests.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ShouldFormatToTime()
        {
            var date = new DateTime(2011, 1, 2, 23, 4, 5);
            Assert.That(date.FormatToTime(), Is.EqualTo("23:04"));
        }

        [Test]
        public void ShouldFormatToDate()
        {
            var date = new DateTime(2011, 1, 2, 23, 4, 5);
            Assert.That(date.FormatToDate(), Is.EqualTo("02/01/2011"));
        }
    }
}

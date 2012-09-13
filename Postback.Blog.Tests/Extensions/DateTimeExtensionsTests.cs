using System;
using NBehave.Spec.NUnit;
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

        [Test]
        public void Yesterday()
        {
            SystemTime.Now = () => new DateTime(2012, 3, 8, 4, 5, 6);
            var date = new DateTime(2012, 3, 7, 6, 7, 8);

            date.FormatToSmartTimeSpan().ShouldEqual("yesterday, " + date.ToString("HH:mm"));
        }

        [Test]
        public void Tomorrow()
        {
            SystemTime.Now = () => new DateTime(2012,3,8,4,5,6);
            var date = new DateTime(2012, 3, 9, 6, 7, 8);

            date.FormatToSmartTimeSpan().ShouldEqual("tomorrow, " + date.ToString("HH:mm"));
        }

        [Test]
        public void Today()
        {
            SystemTime.Now = () => new DateTime(2012, 3, 8, 4, 5, 6);
            var date = new DateTime(2012, 3, 8, 6, 7, 8);

            date.FormatToSmartTimeSpan().ShouldEqual("today, " + date.ToString("HH:mm"));
        }

        [Test]
        public void LongAgo()
        {
            SystemTime.Now = () => new DateTime(2012, 3, 8, 4, 5, 6);
            var date = new DateTime(2012, 3, 4, 6, 7, 8);

            date.FormatToSmartTimeSpan().ShouldEqual(date.ToString("dd/MM/yyyy HH:mm"));
        }

        [Test]
        public void AfterTomorrow()
        {
            SystemTime.Now = () => new DateTime(2012, 3, 8, 4, 5, 6);
            var date = new DateTime(2012, 3, 10, 6, 9, 36);

            date.FormatToSmartTimeSpan().ShouldEqual("2 days");
        }
    }
}

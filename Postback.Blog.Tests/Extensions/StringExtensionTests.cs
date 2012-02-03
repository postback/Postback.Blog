using NUnit.Framework;

namespace Postback.Blog.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionTests : BaseTest
    {
        [Test]
        public void ShouldConvertToUriByReplacingNonLettersAndLoweringCase()
        {
            Assert.That("Test name".ToUri(), Is.EqualTo("test-name"));
        }

        [Test]
        public void ShouldConvertToUriWithUnderscores()
        {
            Assert.That("test_thing".ToUri(), Is.EqualTo("test-thing"));
        }

        [Test]
        public void ShouldTrimTrailingDashesAndReplaceMultipleDashes()
        {
            Assert.That("---the----value---".ToUri(), Is.EqualTo("the-value"));
        }

        [Test]
        public void ShouldConvertAccentedLettersToLetterWithoutAccent()
        {
            Assert.That("Véncent çars".ToUri(), Is.EqualTo("vencent-cars"));
        }

        [Test]
        public void ShouldTrimWhiteSpaceWhenConvertingToUri()
        {
            Assert.That("    www    www".ToUri(), Is.EqualTo("www-www"));
        }

        [Test]
        public void ShouldReplaceWhiteSpaceWhenConvertingToUri()
        {
            Assert.That(@"start   
            
            
            end".ToUri(), Is.EqualTo("start-end"));
        }

        /*
        [Test]
        public void ShouldBeLocalizable()
        {
            Assert.That("test_name".Localize(), Is.EqualTo("Name of test"));
        }

        [Test]
        public void ShouldBeUnLocalizable()
        {
            Assert.That("not_present".Localize(), Is.EqualTo("not_present"));
        }

        [Test]
        public void ShouldBeLocalizableForMvcHtmlString()
        {
            Assert.That(MvcHtmlString.Create("test_name").Localize(), Is.EqualTo("Name of test"));
        }

        [Test]
        public void ShouldBeUnLocalizableForMvcHtmlString()
        {
            Assert.That(MvcHtmlString.Create("not_present").Localize(), Is.EqualTo("not_present"));
        }
         * */
    }
}

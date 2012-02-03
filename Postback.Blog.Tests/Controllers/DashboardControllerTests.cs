using System.Web.Mvc;
using NUnit.Framework;
using Postback.Blog.Areas.Admin.Controllers;

namespace Postback.Blog.Tests.Controllers
{
    [TestFixture]
    public class DashboardControllerTests
    {
        [Test]
        public void IndexShouldReturnNothing()
        {
            var controller = new DashboardController();
            var result = controller.Index();

            Assert.IsInstanceOf(typeof(ViewResult),result);
            var viewresult = (ViewResult)result;
            Assert.That(viewresult.ViewName,Is.Empty);
            Assert.That(viewresult.ViewData,Has.Count.EqualTo(0));
        }
    }
}

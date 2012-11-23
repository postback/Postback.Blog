using System.Linq;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using Postback.Blog.Controllers;
using Postback.Blog.Models;
using Postback.Blog.Models.ViewModels;

namespace Postback.Blog.Tests.ViewModels
{
    [TestFixture]
    public class PagingViewModelTests
    {
        [Test]
        public void UsesUriFunc()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 2, ItemsOnOnePage = 1 };
            paging.Uri = () => "some/fake/thing";
            paging.GetItems().First().Uri.ShouldEqual("some/fake/thing");
        }

        [Test]
        public void NoPageItemsWhenItemsSmallerThanPageSize()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 9, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(0));
        }

        [Test]
        public void NoPageItemsWhenItemsEqualToPageSize()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 10, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(0));
        }

        [Test]
        public void PageItemsWhenItemsLargerThanPageSize()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 11, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(2));
            Assert.That(paging.GetItems().First().Label, Is.EqualTo("1"));
            Assert.That(paging.GetItems().Last().Label, Is.EqualTo("2"));
        }

        [Test]
        public void PageItemsAlwaysContainFirstAndLast()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 500, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems().First().Label, Is.EqualTo("1"));
            Assert.That(paging.GetItems().Last().Label, Is.EqualTo("50"));
        }

        [Test]
        public void PageItemsCountIsAllPagesWhen10OrLessPagesToShow()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 90, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(9));
            Assert.That(paging.GetItems().First().Label, Is.EqualTo("1"));
            Assert.That(paging.GetItems().Last().Label, Is.EqualTo("9"));
        }

        [Test]
        public void PageItemForCurrentPageIsSelected()
        {
            var paging = new PagingView() { CurrentPage = 5, ItemCount = 90, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(9));
            Assert.That(paging.GetItems().First().Label, Is.EqualTo("1"));
            Assert.That(paging.GetItems().Where(p => p.Label == "5").Single().Selected, Is.True);
            Assert.That(paging.GetItems().Last().Label, Is.EqualTo("9"));
        }

        [Test]
        public void PageItemsCountIsNeverLargerThan12()
        {
            var paging = new PagingView() { CurrentPage = 1, ItemCount = 5000, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(11));
        }

        [Test]
        public void WhenCurrentPageIsLessOrEqualToHalfwayPagesCountToShowDontSlide()
        {
            var paging = new PagingView() { CurrentPage = 5, ItemCount = 5000, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(11));
            Assert.That(paging.GetItems().ElementAt(1).Label, Is.EqualTo("2"));
            Assert.That(paging.GetItems().ElementAt(paging.GetItems().Count-2).Label, Is.EqualTo("10"));
        }

        [Test]
        public void WhenCurrentPageIsMoreThanHalfwayPagesCountToShowDontSlide()
        {
            var paging = new PagingView() { CurrentPage = 7, ItemCount = 5000, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(11));
            Assert.That(paging.GetItems().ElementAt(1).Label, Is.EqualTo("3"));
            Assert.That(paging.GetItems().ElementAt(paging.GetItems().Count - 2).Label, Is.EqualTo("11"));
        }

        [Test]
        public void SlidingShowMiddleSeriesOfPageItems()
        {
            var paging = new PagingView() { CurrentPage = 10, ItemCount = 210, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(11));
            Assert.That(paging.GetItems().ElementAt(1).Label, Is.EqualTo("6"));
            Assert.That(paging.GetItems().ElementAt(paging.GetItems().Count - 2).Label, Is.EqualTo("14"));
        }

        [Test]
        public void WhenCurrentPageIsAtEndDontSlide()
        {
            var paging = new PagingView() { CurrentPage = 495, ItemCount = 5000, ItemsOnOnePage = 10 };
            Assert.That(paging.GetItems(), Has.Count.EqualTo(11));
            Assert.That(paging.GetItems().ElementAt(1).Label, Is.EqualTo("491"));
            Assert.That(paging.GetItems().ElementAt(paging.GetItems().Count - 2).Label, Is.EqualTo("499"));
        }
    }
}

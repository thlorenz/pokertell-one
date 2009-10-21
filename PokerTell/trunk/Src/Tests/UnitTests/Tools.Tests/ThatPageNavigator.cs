namespace Tools.Tests
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    public class ThatPageNavigator
    {
        StubBuilder _stub;

        const int FirstPage = 0;
        const int SecondPage = 1;
        const int ThirdPage = 2;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
        }
        
        [Test]
        public void Construct_EmptyList_SetsNumberOfPagesToZero()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();
            const int expectedNumberOfPages = 0;
            
            var navigator = new PageNavigator<int>(itemsPerPage, allItems);
           
            Assert.That(navigator.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void Construct_ItemsOnPageIsOneAndListHasOneItem_SetsNumberOfPagesToOne()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>() };
            const int expectedNumberOfPages = 1;

            var navigator = new PageNavigator<int>(itemsPerPage, allItems);

            Assert.That(navigator.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void Construct_ItemsOnPageIsOneAndListHasThreeItems_SetsNumberOfPagesToThree()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>(), _stub.Some<int>() };
            const int expectedNumberOfPages = 3;

            var navigator = new PageNavigator<int>(itemsPerPage, allItems);

            Assert.That(navigator.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void Construct_ItemsOnPageIsTwoAndListHasThreeItems_SetsNumberOfPagesToTwo()
        {
            const uint itemsPerPage = 2;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>(), _stub.Some<int>() };
            const int expectedNumberOfPages = 2;

            var navigator = new PageNavigator<int>(itemsPerPage, allItems);

            Assert.That(navigator.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void NavigateToPageZero_ListIsEmptyShouldShowIsAlwaysTrue_SetsShownListToEmpty()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();

            var navigator =
                new PageNavigator<int>(itemsPerPage, allItems)
                    .NavigateToPage(0, p => true);

            Assert.That(navigator.ShownItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void NavigateToPageZero_OnePageShouldShowIsAlwaysTrue_SetsShownListToFirstPage()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { FirstPage };

            var navigator =
                new PageNavigator<int>(itemsPerPage, allItems)
                    .NavigateToPage(0, p => true);

            Assert.That(navigator.ShownItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void NavigateToPageZero_OnePageShouldShowIsAlwaysFalse_SetsShownListToEmpty()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>() };

            var navigator =
                new PageNavigator<int>(itemsPerPage, allItems)
                    .NavigateToPage(0, p => false);

            Assert.That(navigator.ShownItems.Count, Is.EqualTo(0));
        }
    }
}
namespace Tools.Tests
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PokerTell.UnitTests;

    using Tools.Interfaces;

    public class ThatItemsPagesManager
    {
        #region Constants and Fields

        const int FirstItem = 0;

        const int SecondItem = 1;

        const int ThirdItem = 2;

        IItemsPagesManager<int> _manager;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _manager = new ItemsPagesManager<int>();
        }

        [Test]
        public void CanNavigateBackward_EmptyList_ReturnsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateBackward, Is.False);
        }

        [Test]
        public void CanNavigateBackward_OnePageCurrentPageIsFirstPage_ReturnsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateBackward, Is.False);
        }

        [Test]
        public void CanNavigateBackward_TwoPagesCurrentPageIsFirstPage_ReturnsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateBackward, Is.False);
        }

        [Test]
        public void CanNavigateBackward_TwoPagesCurrentPageIsSecondPage_ReturnsTrue()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(2);

            Assert.That(_manager.CanNavigateBackward, Is.True);
        }

        [Test]
        public void CanNavigateForward_OnePageCurrentPageIsLastPage_ReturnsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateForward, Is.False);
        }

        [Test]
        public void CanNavigateForward_TwoPagesCurrentPageIsFirst_ReturnsTrue()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateForward, Is.True);
        }

        [Test]
        public void CanNavigateForward_TwoPagesCurrentPageIsSecond_ReturnsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>() };

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(2);

            Assert.That(_manager.CanNavigateForward, Is.False);
        }

        [Test]
        public void FilterItems_AllItemsMatchFilter_ShownItemsContainsAllItems()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .FilterItems(f => true);

            Assert.That(_manager.AllShownItems, Is.EqualTo(allItems));
        }

        [Test]
        public void FilterItems_CurrentPageIsSecondAllItemsMatchFilter_CurrentPageIsSetToFirstPage()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .NavigateToPage(2)
                .FilterItems(f => true);

            Assert.That(_manager.CurrentPage, Is.EqualTo(1));
        }

        [Test]
        public void FilterItems_CurrentPageIsSecondAllItemsMatchFilter_NavigatesToFirstPage()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };
            var expectedItems = new List<int> { FirstItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .NavigateToPage(2)
                .FilterItems(f => true);

            Assert.That(_manager.ItemsOnCurrentPage, Is.EqualTo(expectedItems));
        }

        [Test]
        public void FilterItems_CurrentPageIsSecondNoItemMatchesFilter_CurrentPageIsSetToFirstPage()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .NavigateToPage(2)
                .FilterItems(f => false);

            Assert.That(_manager.CurrentPage, Is.EqualTo(1));
        }

        [Test]
        public void FilterItems_NoItemMatchesFilter_SetsNumberOfPagesToOne()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .NavigateToPage(2)
                .FilterItems(f => false);

            Assert.That(_manager.CurrentPage, Is.EqualTo(1));
        }

        [Test]
        public void FilterItems_NoItemMatchesFilter_ShownItemsIsEmpty()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .FilterItems(f => false);

            Assert.That(_manager.AllShownItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void FilterItems_OneOfThreeItemMatchesFilter_ShownItemsContainsAllItemsThatMatchedFilter()
        {
            IList<int> allItems = new List<int> { FirstItem, SecondItem, ThirdItem };

            _manager
                .InitializeWith((uint)_stub.Valid(For.NumberOfItems, 1), allItems)
                .FilterItems(f => f.Equals(SecondItem));

            IList<int> expectedItems = new List<int> { SecondItem };

            Assert.That(_manager.AllShownItems, Is.EqualTo(expectedItems));
        }

        [Test]
        public void InitializeWith_EmptyList_CanNavigateForwardIsFalse()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.CanNavigateForward, Is.False);
        }

        [Test]
        public void InitializeWith_EmptyList_SetsNumberOfPagesToZero()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();
            const int expectedNumberOfPages = 0;

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void InitializeWith_ItemsOnPageIsOneAndListHasOneItem_SetsNumberOfPagesToOne()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>() };
            const int expectedNumberOfPages = 1;

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void InitializeWith_ItemsOnPageIsOneAndListHasThreeItems_SetsNumberOfPagesToThree()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>(), _stub.Some<int>() };
            const int expectedNumberOfPages = 3;

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void InitializeWith_ItemsOnPageIsTwoAndListHasThreeItems_SetsNumberOfPagesToTwo()
        {
            const uint itemsPerPage = 2;
            IList<int> allItems = new List<int> { _stub.Some<int>(), _stub.Some<int>(), _stub.Some<int>() };
            const int expectedNumberOfPages = 2;

            _manager
                .InitializeWith(itemsPerPage, allItems);

            Assert.That(_manager.NumberOfPages, Is.EqualTo(expectedNumberOfPages));
        }

        [Test]
        public void NavigateToFirstPage_ContainsOnePageAndShouldShowIsAlwaysTrue_NavigatesToToFirstPage()
        {
            const uint itemsPerPage = 1;

            IList<int> itemsOnFirstPage = new List<int> { FirstItem };
            IList<int> allItems = new List<int> { FirstItem };

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(1);

            Assert.That(_manager.ItemsOnCurrentPage, Is.EqualTo(itemsOnFirstPage));
        }

        [Test]
        public void NavigateToFirstPage_ListIsEmpty_SetsShownListToEmpty()
        {
            const uint itemsPerPage = 1;
            IList<int> allItems = new List<int>();

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(1);

            Assert.That(_manager.ItemsOnCurrentPage.Count, Is.EqualTo(0));
        }

        [Test]
        public void NavigateToSecondPage_ContainsOnePage_NavigatesToFirstPage()
        {
            const uint itemsPerPage = 1;
            IList<int> itemsOnFirstPage = new List<int> { FirstItem };
            IList<int> allItems = new List<int> { FirstItem };

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(2);

            Assert.That(_manager.ItemsOnCurrentPage, Is.EqualTo(itemsOnFirstPage));
        }

        [Test]
        public void NavigateToSecondPage_ContainsTwoPages_NavigatesToSecondPage()
        {
            const uint itemsPerPage = 1;

            IList<int> itemsOnSecondPage = new List<int> { SecondItem };
            IList<int> allItems = new List<int> { FirstItem, SecondItem };

            _manager
                .InitializeWith(itemsPerPage, allItems)
                .NavigateToPage(2);

            Assert.That(_manager.ItemsOnCurrentPage, Is.EqualTo(itemsOnSecondPage));
        }

        #endregion
    }
}
namespace Tools
{
    using System;
    using System.Collections.Generic;

    public class PageNavigator<T>
    {
        readonly uint _itemsPerPage;

        readonly IList<T> _allItems;

        readonly IList<T> _shownItems;

        public int NumberOfPages { get; private set; }

        public PageNavigator(uint itemsPerPage, IList<T> allItems)
        {
            _allItems = allItems;
            _itemsPerPage = itemsPerPage;
            _shownItems = new List<T>();

            DetermineNumberOfPages();
        }

        void DetermineNumberOfPages()
        {
            NumberOfPages = (int)(_allItems.Count / _itemsPerPage);
            NumberOfPages = _allItems.Count % _itemsPerPage == 0 ? NumberOfPages : NumberOfPages + 1;
        }

        public IList<T> ShownItems
        {
            get { return _shownItems; }
        }

        public PageNavigator<T> NavigateToPage(uint pageNumber, Predicate<T> shouldShowItem)
        {
            ShownItems.Clear();

            uint index = pageNumber * _itemsPerPage;
            int itemsOnNewPage = 0;

            while (index < _allItems.Count && itemsOnNewPage < _itemsPerPage)
            {
                if (shouldShowItem.Invoke(_allItems[(int)index]))
                {
                    ShownItems.Add(_allItems[(int)index]);
                    itemsOnNewPage++;
                }

                index++;
            }

            return this;
        }

    }
}
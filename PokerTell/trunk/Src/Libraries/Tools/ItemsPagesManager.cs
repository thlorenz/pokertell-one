namespace Tools
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Interfaces;

    public class ItemsPagesManager<T> : IItemsPagesManager<T>
    {
        #region Constants and Fields

        uint _itemsPerPage;

        IList<T> _allShownItems;

        #endregion

        #region Constructors and Destructors

        public ItemsPagesManager()
        {
            ItemsOnCurrentPage = new ObservableCollection<T>();
        }

        public IItemsPagesManager<T> InitializeWith(uint itemsPerPage, IList<T> allItems)
        {
            AllItems = allItems;
            _allShownItems = AllItems;
            _itemsPerPage = itemsPerPage;
            Initialize();

            return this;
        }

        #endregion

        #region Properties

        public IList<T> AllShownItems
        {
            get { return _allShownItems; }
        }

        public bool CanNavigateBackward
        {
            get { return CurrentPage > 1; }
        }

        public bool CanNavigateForward
        {
            get { return CurrentPage < NumberOfPages; }
        }

        public uint CurrentPage { get; private set; }

        public ObservableCollection<T> ItemsOnCurrentPage { get; private set; }

        public uint NumberOfPages { get; private set; }

        public IList<T> AllItems { get; private set; }

        #endregion

        #region Public Methods

        public IItemsPagesManager<T> FilterItems(Predicate<T> isMatch)
        {
            _allShownItems = (from item in AllItems where isMatch(item) select item).ToList();
            Initialize();
            return this;
        }

        public IItemsPagesManager<T> NavigateForward()
        {
            NavigateToPage(CurrentPage + 1);
            return this;
        }

        public IItemsPagesManager<T> NavigateBackward()
        {
            NavigateToPage(CurrentPage - 1);
            return this;
        }

        public IItemsPagesManager<T> NavigateToPage(uint pageNumber)
        {
            // Make sure we don't navigate beyond the boundary of the pages
            pageNumber = pageNumber > NumberOfPages ? NumberOfPages : pageNumber;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            CurrentPage = pageNumber;

            // Adjust PageNumber (Page 0 doesn't exist in a document but it exists in zero based List)  
            pageNumber--;

            uint index = pageNumber * _itemsPerPage;
            int itemsOnNewPage = 0;

            ItemsOnCurrentPage.Clear();
            while (index < AllShownItems.Count && itemsOnNewPage < _itemsPerPage)
            {
                ItemsOnCurrentPage.Add(AllShownItems[(int)index]);
                itemsOnNewPage++;

                index++;
            }

            return this;
        }

        #endregion

        #region Methods

        void Initialize()
        {
            DetermineNumberOfPages();
            NavigateToPage(1);
        }

        void DetermineNumberOfPages()
        {
            NumberOfPages = (uint)(AllShownItems.Count / _itemsPerPage);
            NumberOfPages = AllShownItems.Count % _itemsPerPage == 0 ? NumberOfPages : NumberOfPages + 1;
        }

        #endregion
    }
}
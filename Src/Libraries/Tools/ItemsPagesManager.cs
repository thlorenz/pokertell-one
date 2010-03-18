namespace Tools
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;

    using Tools.Interfaces;

    [Serializable]
    public class ItemsPagesManager<T> : IItemsPagesManager<T>
    {
        IList<T> _allItems;

        IList<T> _allShownItems;

        uint _currentPage;

        [NonSerialized]
        ObservableCollection<T> _itemsOnCurrentPage;

        uint _itemsPerPage;

        [NonSerialized]
        uint _numberOfPages;

        public ItemsPagesManager()
        {
            ItemsOnCurrentPage = new ObservableCollection<T>();
        }

        [field: NonSerialized]
        public event Action Deserialized;

        public IList<T> AllItems
        {
            get { return _allItems; }
            private set { _allItems = value; }
        }

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

        public uint CurrentPage
        {
            get { return _currentPage; }
            private set { _currentPage = value; }
        }

        public ObservableCollection<T> ItemsOnCurrentPage
        {
            get { return _itemsOnCurrentPage; }
            private set { _itemsOnCurrentPage = value; }
        }

        public uint ItemsPerPage
        {
            get { return _itemsPerPage; }
        }

        public uint NumberOfPages
        {
            get { return _numberOfPages; }
            private set { _numberOfPages = value; }
        }

        public IItemsPagesManager<T> FilterItems(Predicate<T> isMatch)
        {
            _allShownItems = (from item in AllItems where isMatch(item) select item).ToList();
            Initialize();
            return this;
        }

        public IItemsPagesManager<T> InitializeWith(uint itemsPerPage, IList<T> allItems)
        {
            AllItems = allItems;
            _allShownItems = AllItems;
            _itemsPerPage = itemsPerPage;
            Initialize();

            return this;
        }

        public IItemsPagesManager<T> NavigateBackward()
        {
            NavigateToPage(CurrentPage - 1);
            return this;
        }

        public IItemsPagesManager<T> NavigateForward()
        {
            NavigateToPage(CurrentPage + 1);
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

            uint index = pageNumber * ItemsPerPage;
            int itemsOnNewPage = 0;

            ItemsOnCurrentPage.Clear();
            while (index < AllShownItems.Count && itemsOnNewPage < ItemsPerPage)
            {
                ItemsOnCurrentPage.Add(AllShownItems[(int)index]);
                itemsOnNewPage++;

                index++;
            }

            return this;
        }

        void DetermineNumberOfPages()
        {
            NumberOfPages = (uint)(AllShownItems.Count / ItemsPerPage);
            NumberOfPages = AllShownItems.Count % ItemsPerPage == 0 ? NumberOfPages : NumberOfPages + 1;
        }

        void Initialize()
        {
            DetermineNumberOfPages();
            NavigateToPage(1);
        }

        void InvokeDeserialized()
        {
            Action deserialized = Deserialized;
            if (deserialized != null)
            {
                deserialized();
            }
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            ItemsOnCurrentPage = new ObservableCollection<T>();
            DetermineNumberOfPages();
            NavigateToPage(_currentPage);

            InvokeDeserialized();
        }
    }
}
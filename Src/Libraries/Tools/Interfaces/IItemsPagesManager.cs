namespace Tools.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IItemsPagesManager<T>
    {
        IList<T> AllShownItems { get; }

        bool CanNavigateBackward { get; }

        bool CanNavigateForward { get; }

        uint CurrentPage { get; }

        ObservableCollection<T> ItemsOnCurrentPage { get; }

        uint NumberOfPages { get; }

        IList<T> AllItems { get; }

        uint ItemsPerPage { get; }

        IItemsPagesManager<T> FilterItems(Predicate<T> isMatch1);

        IItemsPagesManager<T> NavigateToPage(uint pageNumber);

        IItemsPagesManager<T> InitializeWith(uint itemsPerPage, IList<T> allItems);

        IItemsPagesManager<T> NavigateForward();

        IItemsPagesManager<T> NavigateBackward();

        event Action Deserialized;
    }
}
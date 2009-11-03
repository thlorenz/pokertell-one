namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IHandHistoriesViewModel
    {
        IHandHistoriesViewModel ApplyFilter(IPokerHandCondition condition);

        ObservableCollection<IHandHistoryViewModel> HandHistoriesOnPage { get; }

        bool ShowSelectOption { get; set; }

        IEnumerable<IHandHistoryViewModel> SelectedHandHistories { get; }

        ObservableCollection<int> PageNumbers { get; }

        IHandHistoriesFilter HandHistoriesFilter { get; }

        bool SelectAllHandHistoriesOnPage { get; set; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);

        IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage);

        event Action PageTurn;

        bool SelectAllShownHandHistories { get; set; }

    }
}
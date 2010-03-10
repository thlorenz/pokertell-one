namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public interface IHandHistoriesViewModel
    {
        IHandHistoriesViewModel ApplyFilter(IPokerHandCondition condition);

        ObservableCollection<IHandHistoryViewModel> HandHistoriesOnPage { get; }

        bool ShowSelectOption { get; set; }

        IEnumerable<IHandHistoryViewModel> SelectedHandHistories { get; }

        ObservableCollection<int> PageNumbers { get; }

        IHandHistoriesFilter HandHistoriesFilter { get; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);

        IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage);

        event Action PageTurn;

        ICommand SelectAllHandHistoriesOnPageCommand { get; }

        ICommand UnselectAllHandHistoriesOnPageCommand { get; }

        ICommand SelectAllShownHandHistoriesCommand { get; }

        ICommand UnselectAllShownHandHistoriesCommand { get; }

        bool ShowHandNotes { set; }
    }
}
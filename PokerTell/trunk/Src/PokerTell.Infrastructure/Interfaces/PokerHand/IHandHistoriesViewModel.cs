namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Tools.GenericUtilities;

    public interface IHandHistoriesViewModel
    {
        IHandHistoriesViewModel ApplyFilter(IPokerHandCondition condition);

        ObservableCollection<IHandHistoryViewModel> HandHistoriesOnPage { get; }

        bool ShowSelectOption { get; set; }

        bool ShowSelectedOnly { get; set; }

        bool ShowPreflopFolds { get; set; }

        string HeroName { get; set; }

        IEnumerable<IHandHistoryViewModel> SelectedHandHistories { get; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);

        void SelectPlayer(bool clearSelection);

        IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage);

        event Action PageTurn;
    }
}
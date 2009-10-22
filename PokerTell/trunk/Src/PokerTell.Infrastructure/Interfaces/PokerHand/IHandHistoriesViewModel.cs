namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Tools.GenericUtilities;

    public interface IHandHistoriesViewModel
    {
        IHandHistoriesViewModel ApplyFilter(IPokerHandCondition condition);

        ObservableCollection<IHandHistoryViewModel> HandHistoryViewModelsOnPage { get; }

        bool ShowSelectOption { set; }

        bool ShowSelectedOnly { set; }

        bool ShowPreflopFolds { get; set; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);

        void SelectPlayer(string name);

        IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage);
    }
}
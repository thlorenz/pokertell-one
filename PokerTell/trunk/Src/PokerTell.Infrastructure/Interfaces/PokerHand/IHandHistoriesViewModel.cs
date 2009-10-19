namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Tools.GenericUtilities;

    public interface IHandHistoriesViewModel
    {
        CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction { get; }

        IEnumerable<IHandHistoryViewModel> HandHistoryViewModels { get; }

        bool ShowSelectOption { set; }

        bool ShowSelectedOnly { set; }

        bool ShowPreflopFolds { get; set; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);

        void SelectPlayer(string name);
    }
}
namespace PokerTell.PokerHand.ViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;

    public interface IHandHistoriesViewModel
    {
        CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction { get; }

        IEnumerable<IHandHistoryViewModel> HandHistoryViewModels { get; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);
    }
}
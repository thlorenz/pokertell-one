namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;

    using Tools.GenericUtilities;

    public interface IHandHistoriesViewModel
    {
        CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction { get; }

        IEnumerable<IHandHistoryViewModel> HandHistoryViewModels { get; }

        IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands);
    }
}
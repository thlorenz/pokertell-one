namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IOverlaySettingsAidViewModel : IFluentInterface
    {
        IOverlayBoardViewModel Board { get; }

        IList<IOverlayHoleCardsViewModel> HoleCards { get; }

        IOverlaySettingsAidViewModel InitializeWith(ITableOverlaySettingsViewModel overlaySettings);
    }
}
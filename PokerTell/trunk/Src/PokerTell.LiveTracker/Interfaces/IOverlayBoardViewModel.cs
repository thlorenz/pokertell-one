namespace PokerTell.LiveTracker.Interfaces
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    public interface IOverlayBoardViewModel : IBoardViewModel, IDisposable
    {
        double Left { get; set; }

        double Top { get; set; }

        IOverlayBoardViewModel HideBoardAfter(int seconds);

        IOverlayBoardViewModel InitializeWith(ITableOverlaySettingsViewModel overlaySettings);
    }
}
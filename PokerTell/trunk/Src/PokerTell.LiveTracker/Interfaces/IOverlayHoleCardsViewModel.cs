namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;

    using Infrastructure.Interfaces.PokerHand;

    using ViewModels.Overlay;

    public interface IOverlayHoleCardsViewModel : IHoleCardsViewModel, IDisposable
    {
        double Left { get; set; }

        double Top { get; set; }

        void SetLocationTo(Point location);

        IOverlayHoleCardsViewModel InitializeWith(IList<Point> holeCardPositions, int seatNumber);

        IOverlayHoleCardsViewModel HideHoleCardsAfter(int seconds);
    }
}
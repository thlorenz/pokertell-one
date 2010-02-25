namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows;

    public interface IPlayerStatusViewModel
    {
        bool IsPresent { get; set; }

        IHarringtonMViewModel HarringtonM { get; }

        IOverlayHoleCardsViewModel HoleCards { get; }

        IPlayerStatusViewModel ShowHoleCardsFor(int duration, string holecards);

        IPlayerStatusViewModel InitializeWith(IList<Point> holeCardsPositions, IList<Point> harringtonMPositions, int seatNumber);
    }
}
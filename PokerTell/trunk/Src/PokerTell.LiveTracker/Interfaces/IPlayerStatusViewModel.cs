namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows;

    using Tools.WPF.Interfaces;

    public interface IPlayerStatusViewModel
    {
        bool IsPresent { get; set; }

        IHarringtonMViewModel HarringtonM { get; }

        IOverlayHoleCardsViewModel HoleCards { get; }

        IPlayerStatusViewModel ShowHoleCardsFor(int duration, string holecards);

        IPlayerStatusViewModel InitializeWith(IPositionViewModel holeCardsPosition, IPositionViewModel harringtonMPosition);
    }
}
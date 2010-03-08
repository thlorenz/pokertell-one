namespace PokerTell.LiveTracker.Interfaces
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    public interface IOverlayBoardViewModel : IDisposable
    {
        IBoardViewModel BoardViewModel { get; }

        IPositionViewModel Position { get; }

        bool AllowDragging { get; set; }

        IOverlayBoardViewModel HideBoardAfter(int seconds);

        void UpdateWith(string boardString);

        IOverlayBoardViewModel InitializeWith(IPositionViewModel position);
    }
}
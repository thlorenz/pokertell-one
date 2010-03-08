namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Tools.WPF.Interfaces;

    public interface IOverlayHoleCardsViewModel : IDisposable
    {
        IOverlayHoleCardsViewModel HideHoleCardsAfter(int seconds);

        void UpdateWith(string holeCardsString);

        IOverlayHoleCardsViewModel InitializeWith(IPositionViewModel position);

        IPositionViewModel Position { get; }

        bool AllowDragging { get; set; }
    }
}
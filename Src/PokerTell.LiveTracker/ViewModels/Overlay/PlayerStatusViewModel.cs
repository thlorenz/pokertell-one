namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Interfaces;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class PlayerStatusViewModel : NotifyPropertyChanged, IPlayerStatusViewModel
    {
        public PlayerStatusViewModel(IHarringtonMViewModel harringtonM, IOverlayHoleCardsViewModel holeCards)
        {
            HarringtonM = harringtonM;
            HoleCards = holeCards;
        }

        bool _ispresent;

        public bool IsPresent
        {
            get { return _ispresent; }
            set
            {
                _ispresent = value;
                RaisePropertyChanged(() => IsPresent);
            }
        }

        public IOverlayHoleCardsViewModel HoleCards { get; protected set; }

        public IHarringtonMViewModel HarringtonM { get;  protected set; }

        public IPlayerStatusViewModel ShowHoleCardsFor(int duration, string holecards)
        {
            HoleCards.UpdateWith(holecards);
            HoleCards.HideHoleCardsAfter(duration);
            return this;
        }

        public IPlayerStatusViewModel InitializeWith(IPositionViewModel holeCardsPosition, IPositionViewModel harringtonMPosition)
        {
            HoleCards.InitializeWith(holeCardsPosition);
            HarringtonM.InitializeWith(harringtonMPosition);
            return this;
        }
    }
}
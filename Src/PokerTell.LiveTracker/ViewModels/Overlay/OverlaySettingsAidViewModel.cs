namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public class OverlaySettingsAidViewModel : IOverlaySettingsAidViewModel
    {
        readonly IConstructor<IOverlayHoleCardsViewModel> _holeCardsViewModelMake;

        public OverlaySettingsAidViewModel(IConstructor<IOverlayHoleCardsViewModel> holeCardsViewModelMake, IOverlayBoardViewModel boardViewModel)
        {
            _holeCardsViewModelMake = holeCardsViewModelMake;
            Board = boardViewModel;
        }

        public IOverlayBoardViewModel Board { get; protected set; }

        public IList<IOverlayHoleCardsViewModel> HoleCards { get; protected set; }

        public IOverlaySettingsAidViewModel InitializeWith(ITableOverlaySettingsViewModel overlaySettings)
        {
            Board
                .InitializeWith(overlaySettings.BoardPosition)
                .UpdateWith("Ts Js Qs Ks As");
            Board.AllowDragging = true;

            SetupHoleCards(overlaySettings);

            return this;
        }

        void SetupHoleCards(ITableOverlaySettingsViewModel overlaySettings)
        {
            HoleCards = new List<IOverlayHoleCardsViewModel>();
            
            foreach (var seatNumber in 0.To(overlaySettings.TotalSeats - 1))
            {
                var rank = seatNumber.Match()
                    .With(n => n + 1 == 1, _ => "A")
                    .With(n => n + 1 == 10, _ => "T")
                    .Else(n => (n + 1).ToString())
                    .Do();

                var holecards = _holeCardsViewModelMake.New
                    .InitializeWith(overlaySettings.HoleCardsPositions[seatNumber]);
                holecards
                    .UpdateWith(string.Format("{0}h {0}c", rank));
                holecards.AllowDragging = true;

                HoleCards.Add(holecards);
            }
        }
    }
}
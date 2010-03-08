namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class OverlayHoleCardsViewModel : NotifyPropertyChanged, IOverlayHoleCardsViewModel
    {
        public OverlayHoleCardsViewModel(IHoleCardsViewModel holeCardsViewModel, IDispatcherTimer dispatcherTimer)
        {
            _dispatcherTimer = dispatcherTimer;
            _holeCardsViewModel = holeCardsViewModel;

            _dispatcherTimer.Tick += (s, e) => {
                _dispatcherTimer.Stop();
                HoleCardsViewModel.Visible = false;
            };
        }

        readonly IHoleCardsViewModel _holeCardsViewModel;

        readonly IDispatcherTimer _dispatcherTimer;

        public IPositionViewModel Position { get; protected set; }

        public bool AllowDragging { get; set; }

        public IOverlayHoleCardsViewModel InitializeWith(IPositionViewModel position)
        {
            Position = position;

            return this;
        }

        public IOverlayHoleCardsViewModel HideHoleCardsAfter(int seconds)
        {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(seconds);
            _dispatcherTimer.Start();
            return this;
        }

        /// <summary>
        /// Necessary to prevent memory leaks
        /// http://geekswithblogs.net/dotnetrodent/archive/2009/11/05/136015.aspx
        /// </summary>
        public void Dispose()
        {
            _dispatcherTimer.Stop();
        }

        public IHoleCardsViewModel HoleCardsViewModel
        {
            get { return _holeCardsViewModel; }
        }

        public void UpdateWith(string holeCardsString)
        {
            HoleCardsViewModel.UpdateWith(holeCardsString);
        }
    }
}
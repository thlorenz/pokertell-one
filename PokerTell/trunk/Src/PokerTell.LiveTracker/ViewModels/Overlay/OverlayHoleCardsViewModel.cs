namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public class OverlayHoleCardsViewModel : NotifyPropertyChanged, IOverlayHoleCardsViewModel
    {

        public OverlayHoleCardsViewModel(IHoleCardsViewModel holeCardsViewModel, IDispatcherTimer dispatcherTimer)
        {
            _dispatcherTimer = dispatcherTimer;
            _holeCardsViewModel = holeCardsViewModel;

            _dispatcherTimer.Tick += (s, e) => {
                _dispatcherTimer.Stop();
                _holeCardsViewModel.Visible = false;
            };
        }

        readonly IHoleCardsViewModel _holeCardsViewModel;

        readonly IDispatcherTimer _dispatcherTimer;

        int _seatNumber;

        IList<Point> _holeCardPositions;

        public double Left
        {
            get { return _holeCardPositions[_seatNumber].X; }

            set
            {
                _holeCardPositions[_seatNumber] = new Point(value, Top);
                RaisePropertyChanged(() => Left);
            }
        }

        public double Top
        {
            get { return _holeCardPositions[_seatNumber].Y; }

            set
            {
                _holeCardPositions[_seatNumber] = new Point(Left, value);
                RaisePropertyChanged(() => Top);
            }
        }

        public IOverlayHoleCardsViewModel InitializeWith(IList<Point> holeCardPositions, int seatNumber)
        {
            _holeCardPositions = holeCardPositions;
            _seatNumber = seatNumber;
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

        public void SetLocationTo(Point location)
        {
            Left = location.X;
            Top = location.Y;
        }

        public string Rank1
        {
            get { return _holeCardsViewModel.Rank1; }
            set { _holeCardsViewModel.Rank1 = value; }
        }

        public string Rank2
        {
            get { return _holeCardsViewModel.Rank2; }
            set { _holeCardsViewModel.Rank2 = value; }
        }

        public Image Suit1
        {
            get { return _holeCardsViewModel.Suit1; }
            set { _holeCardsViewModel.Suit1 = value; }
        }

        public Image Suit2
        {
            get { return _holeCardsViewModel.Suit2; }
            set { _holeCardsViewModel.Suit2 = value; }
        }

        public bool Visible
        {
            get { return _holeCardsViewModel.Visible; }
            set { _holeCardsViewModel.Visible = value; }
        }

        public void UpdateWith(string holeCardsString)
        {
            _holeCardsViewModel.UpdateWith(holeCardsString);
        }
    }
}
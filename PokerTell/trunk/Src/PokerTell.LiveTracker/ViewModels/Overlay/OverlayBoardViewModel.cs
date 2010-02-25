namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    /// <summary>
    /// Adds some extra functionality, namely Position, a timer and Settings interaction to the BoardViewModel
    /// </summary>
    public class OverlayBoardViewModel : NotifyPropertyChanged, IOverlayBoardViewModel
    {
        public OverlayBoardViewModel(IBoardViewModel boardViewModel, IDispatcherTimer dispatcherTimer)
        {
            _dispatcherTimer = dispatcherTimer;
            _boardViewModel = boardViewModel;

            _dispatcherTimer.Tick += (s, e) => {
                _dispatcherTimer.Stop();
                _boardViewModel.Visible = false;
            };
        }

        readonly IBoardViewModel _boardViewModel;

        readonly IDispatcherTimer _dispatcherTimer;

        ITableOverlaySettingsViewModel _overlaySettings;

        public double Left
        {
            get { return _overlaySettings.BoardPosition.X; }

            set
            {
                _overlaySettings.BoardPosition = new Point(value, Top);
                RaisePropertyChanged(() => Left);
            }
        }

        public double Top
        {
            get { return _overlaySettings.BoardPosition.Y; }

            set
            {
                _overlaySettings.BoardPosition = new Point(Left, value);
                RaisePropertyChanged(() => Top);
            }
        }

        public IOverlayBoardViewModel HideBoardAfter(int seconds)
        {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(seconds);
            _dispatcherTimer.Start();
            return this;
        }

        public IOverlayBoardViewModel InitializeWith(ITableOverlaySettingsViewModel overlaySettings)
        {
            _overlaySettings = overlaySettings;
            return this;
        }

        public string Rank1
        {
            get { return _boardViewModel.Rank1; }
            set { _boardViewModel.Rank1 = value; }
        }

        public string Rank2
        {
            get { return _boardViewModel.Rank2; }
            set { _boardViewModel.Rank2 = value; }
        }

        public string Rank3
        {
            get { return _boardViewModel.Rank3; }
            set { _boardViewModel.Rank3 = value; }
        }

        public string Rank4
        {
            get { return _boardViewModel.Rank4; }
            set { _boardViewModel.Rank4 = value; }
        }

        public string Rank5
        {
            get { return _boardViewModel.Rank5; }
            set { _boardViewModel.Rank5 = value; }
        }

        public Image Suit1
        {
            get { return _boardViewModel.Suit1; }
            set { _boardViewModel.Suit1 = value; }
        }

        public Image Suit2
        {
            get { return _boardViewModel.Suit2; }
            set { _boardViewModel.Suit2 = value; }
        }

        public Image Suit3
        {
            get { return _boardViewModel.Suit3; }
            set { _boardViewModel.Suit3 = value; }
        }

        public Image Suit4
        {
            get { return _boardViewModel.Suit4; }
            set { _boardViewModel.Suit4 = value; }
        }

        public Image Suit5
        {
            get { return _boardViewModel.Suit5; }
            set { _boardViewModel.Suit5 = value; }
        }

        public bool Visible
        {
            get { return _boardViewModel.Visible; }
            set { _boardViewModel.Visible = value; }
        }

        public void UpdateWith(string boardString)
        {
            _boardViewModel.UpdateWith(boardString);
        }

        /// <summary>
        /// Necessary to prevent memory leaks
        /// http://geekswithblogs.net/dotnetrodent/archive/2009/11/05/136015.aspx
        /// </summary>
        public void Dispose()
        {
            _dispatcherTimer.Stop();
        }
    }
}
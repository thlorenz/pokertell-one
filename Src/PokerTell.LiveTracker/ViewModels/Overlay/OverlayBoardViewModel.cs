namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    /// <summary>
    /// Adds some extra functionality,  a timer  to the BoardViewModel
    /// </summary>
    public class OverlayBoardViewModel : NotifyPropertyChanged, IOverlayBoardViewModel
    {
        public OverlayBoardViewModel(IBoardViewModel boardViewModel, IDispatcherTimer dispatcherTimer)
        {
            _dispatcherTimer = dispatcherTimer;
            _boardViewModel = boardViewModel;

            _dispatcherTimer.Tick += (s, e) => {
                _dispatcherTimer.Stop();
                BoardViewModel.Visible = false;
            };
        }

        readonly IBoardViewModel _boardViewModel;

        readonly IDispatcherTimer _dispatcherTimer;

        public IPositionViewModel Position { get; protected set; }

        public IOverlayBoardViewModel InitializeWith(IPositionViewModel position)
        {
            Position = position;

            return this;
        }

        public IOverlayBoardViewModel HideBoardAfter(int seconds)
        {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(seconds);
            _dispatcherTimer.Start();
            return this;
        }

        public IBoardViewModel BoardViewModel
        {
            get { return _boardViewModel; }
        }

        public void UpdateWith(string boardString)
        {
            BoardViewModel.UpdateWith(boardString);
        }

        public bool AllowDragging { get; set; }

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
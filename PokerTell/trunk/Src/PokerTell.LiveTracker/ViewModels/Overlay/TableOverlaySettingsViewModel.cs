namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Interfaces;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class TableOverlaySettingsViewModel : NotifyPropertyChanged, ITableOverlaySettingsViewModel
    {
        bool _showPreFlop;

        public bool ShowPreFlop
        {
            get { return _showPreFlop; }
            set
            {
                _showPreFlop = value;
                RaisePropertyChanged(() => ShowPreFlop);
            }
        }

        bool _showFlop;

        public bool ShowFlop
        {
            get { return _showFlop; }
            set
            {
                _showFlop = value;
                RaisePropertyChanged(() => ShowFlop);
            }
        }

        bool _showTurn;

        public bool ShowTurn
        {
            get { return _showTurn; }
            set
            {
                _showTurn = value;
                RaisePropertyChanged(() => ShowTurn);
            }
        }

        bool _showRiver;

        public bool ShowRiver
        {
            get { return _showRiver; }
            set
            {
                _showRiver = value;
                RaisePropertyChanged(() => ShowRiver);
            }
        }

        bool _showHarringtonM;

        public bool ShowHarringtonM
        {
            get { return _showHarringtonM; }
            set
            {
                _showHarringtonM = value;
                RaisePropertyChanged(() => ShowHarringtonM);
            }
        }

        public IList<IPositionViewModel> PlayerStatisticsPanelPositions { get;  protected set; }

        public IList<IPositionViewModel> HarringtonMPositions { get; protected set; }

        public IList<IPositionViewModel> HoleCardsPositions { get;  protected set; }

        public IPositionViewModel BoardPosition { get; set; }

        public event Action PreferredSeatChanged = delegate { };
        
        double _width;

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged(() => Width);
            }
        }

        double _height;

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged(() => Height);
            }
        }

        IColorViewModel _background;

        public IColorViewModel Background
        {
            get { return _background; }
            set
            {
                _background = value;
                RaisePropertyChanged(() => Background);
            }
        }

        IColorViewModel _inPositionForeground;

        public IColorViewModel InPositionForeground
        {
            get { return _inPositionForeground; }
            set
            {
                _inPositionForeground = value;
                RaisePropertyChanged(() => InPositionForeground);
            }
        }

        IColorViewModel _outOfPositionForeground;

        public IColorViewModel OutOfPositionForeground
        {
            get { return _outOfPositionForeground; }
            set
            {
                _outOfPositionForeground = value;
                RaisePropertyChanged(() => OutOfPositionForeground);
            }
        }

        int _preferredSeat;

        public int PreferredSeat
        {
            get { return _preferredSeat; }
            set
            {
                _preferredSeat = value;
                RaisePropertyChanged(() => PreferredSeat);
                PreferredSeatChanged();
            }
        }

        int _totalSeats;

        public int TotalSeats
        {
            get { return _totalSeats; }
            set
            {
                _totalSeats = value;
                RaisePropertyChanged(() => TotalSeats);
            }
        }

        bool _positioningMuckedCards;

        public bool PositioningMuckedCards
        {
            get { return _positioningMuckedCards; }
            set
            {
                _positioningMuckedCards = value;
                RaisePropertyChanged(() => PositioningMuckedCards);
            }
        }
    }
}
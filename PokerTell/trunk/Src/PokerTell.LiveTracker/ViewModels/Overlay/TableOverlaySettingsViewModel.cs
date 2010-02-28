namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class TableOverlaySettingsViewModel : NotifyPropertyChanged, ITableOverlaySettingsViewModel
    {
        bool _settingsModified;

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

        double _statisticsPanelWidth;

        public double StatisticsPanelWidth
        {
            get { return _statisticsPanelWidth; }
            set
            {
                _statisticsPanelWidth = value;
                RaisePropertyChanged(() => StatisticsPanelWidth);
            }
        }

        double _statisticsPanelHeight;

        public double StatisticsPanelHeight
        {
            get { return _statisticsPanelHeight; }
            set
            {
                _statisticsPanelHeight = value;
                RaisePropertyChanged(() => StatisticsPanelHeight);
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

        public int TotalSeats { get; protected set; }

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

        public IList<IPositionViewModel> PlayerStatisticsPanelPositions { get; protected set; }

        public IList<IPositionViewModel> HarringtonMPositions { get; protected set; }

        public IList<IPositionViewModel> HoleCardsPositions { get; protected set; }

        public IPositionViewModel BoardPosition { get; set; }

        public event Action PreferredSeatChanged = delegate { };

        public event Action SaveChanges = delegate { };

        public event Action UndoChanges = delegate { };

        public ITableOverlaySettingsViewModel RaisePropertyChangedForAllProperties()
        {
            Background.RaisePropertyChangedForAllProperties();
            OutOfPositionForeground.RaisePropertyChangedForAllProperties();
            InPositionForeground.RaisePropertyChangedForAllProperties();
            return this;
        }

        ICommand _saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                return _saveChangesCommand ?? (_saveChangesCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _settingsModified = false;
                            SaveChanges();
                        }, 
                        CanExecuteDelegate = arg => _settingsModified
                    });
            }
        }

        ICommand _undoChangesCommand;

        public ICommand UndoChangesCommand
        {
            get
            {
                return _undoChangesCommand ?? (_undoChangesCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _settingsModified = false;
                            UndoChanges();
                        }, 
                        CanExecuteDelegate = arg => _settingsModified
                    });
            }
        }

        public ITableOverlaySettingsViewModel InitializeWith(
            int totalSeats, 
            bool showPreFlop, 
            bool showFlop, 
            bool showTurn, 
            bool showRiver, 
            bool showHarringtonM, 
            double width, 
            double height, 
            string background, 
            string outOfPositionForeground, 
            string inPositionForeground, 
            int preferredSeat, 
            IList<IPositionViewModel> playerStatisticPositions, 
            IList<IPositionViewModel> harringtonMPositions, 
            IList<IPositionViewModel> holeCardsPositions, 
            IPositionViewModel boardPosition)
        {
            TotalSeats = totalSeats;
            ShowPreFlop = showPreFlop;
            ShowFlop = showFlop;
            ShowTurn = showTurn;
            ShowRiver = showRiver;

            ShowHarringtonM = showHarringtonM;

            StatisticsPanelWidth = width;
            StatisticsPanelHeight = height;

            Background = new ColorViewModel(background);

            OutOfPositionForeground = new ColorViewModel(outOfPositionForeground);
            InPositionForeground = new ColorViewModel(inPositionForeground);

            PreferredSeat = preferredSeat;
            PlayerStatisticsPanelPositions = new List<IPositionViewModel>(playerStatisticPositions);

            HarringtonMPositions = harringtonMPositions;
            HoleCardsPositions = holeCardsPositions;
            BoardPosition = boardPosition;

            RegisterSettingsModificationEvents();

            _settingsModified = false;
            return this;
        }

        void RegisterSettingsModificationEvents()
        {
            Background.PropertyChanged += SettingsModified;
            OutOfPositionForeground.PropertyChanged += SettingsModified;
            InPositionForeground.PropertyChanged += SettingsModified;

            PlayerStatisticsPanelPositions.ForEach(p => p.PropertyChanged += SettingsModified);
            HarringtonMPositions.ForEach(p => p.PropertyChanged += SettingsModified);
            HoleCardsPositions.ForEach(p => p.PropertyChanged += SettingsModified);

            BoardPosition.PropertyChanged += SettingsModified;

            PropertyChanged += SettingsModified;
        }

        void SettingsModified(object sender, PropertyChangedEventArgs e)
        {
            _settingsModified = true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(
                string.Format(
                    "TotalSeats: {12}\n Background: {0}\n InPositionForeground: {1}\n OutOfPositionForeground: {2}\n PositioningMuckedCards: {3}\n PreferredSeat: {4}\n ShowFlop: {5}\n ShowHarringtonM: {6}\n ShowPreFlop: {7}\n ShowRiver: {8}\n ShowTurn: {9}\n StatisticsPanelHeight: {10}\n StatisticsPanelWidth: {11}\n BoardPosition: {13}", 
                    _background, 
                    _inPositionForeground, 
                    _outOfPositionForeground, 
                    _positioningMuckedCards, 
                    _preferredSeat, 
                    _showFlop, 
                    _showHarringtonM, 
                    _showPreFlop, 
                    _showRiver, 
                    _showTurn, 
                    _statisticsPanelHeight, 
                    _statisticsPanelWidth, 
                    TotalSeats, 
                    BoardPosition));
            sb.AppendLine("\nPlayerStatisticsPanelPositions: ");
            PlayerStatisticsPanelPositions.ForEach(s => sb.AppendFormat("[{0}]", s));

            sb.AppendLine("\nHarringtonMPositions : ");
            HarringtonMPositions.ForEach(s => sb.AppendFormat("[{0}]", s));

            sb.AppendLine("\nHoleCardsPositions : ");
            HoleCardsPositions.ForEach(s => sb.AppendFormat("[{0}]", s));
            return sb.ToString();
        }
    }
}
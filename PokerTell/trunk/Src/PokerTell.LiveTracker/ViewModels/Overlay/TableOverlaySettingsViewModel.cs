namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Windows.Media;

    using Interfaces;

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

        double _opacity;

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                RaisePropertyChanged(() => Opacity);
            }
        }

        string _background;

        public string Background
        {
            get { return _background; }
            set
            {
                _background = value;
                RaisePropertyChanged(() => Background);
            }
        }

        string _inPositionForeground;

        public string InPositionForeground
        {
            get { return _inPositionForeground; }
            set
            {
                _inPositionForeground = value;
                RaisePropertyChanged(() => InPositionForeground);
            }
        }

        string _outOfPositionForeground;

        public string OutOfPositionForeground
        {
            get { return _outOfPositionForeground; }
            set
            {
                _outOfPositionForeground = value;
                RaisePropertyChanged(() => OutOfPositionForeground);
            }
        }
    }
}
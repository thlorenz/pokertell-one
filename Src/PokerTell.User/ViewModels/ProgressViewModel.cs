namespace PokerTell.User.ViewModels
{
    using Interfaces;

    using Tools.WPF.ViewModels;

    public class ProgressViewModel : NotifyPropertyChanged, IProgressViewModel
    {
        double _percentCompleted;

        bool _visible;

        public ProgressViewModel()
        {
            PercentCompleted = 100.0;
        }

        public double PercentCompleted
        {
            get { return _percentCompleted; }
            set
            {
                _percentCompleted = value;
                RaisePropertyChanged(() => PercentCompleted);
                Visible = value < 100;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }
    }
}
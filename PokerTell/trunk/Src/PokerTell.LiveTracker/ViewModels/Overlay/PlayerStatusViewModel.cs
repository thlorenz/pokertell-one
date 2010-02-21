namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using Interfaces;

    using Tools.WPF.ViewModels;

    public class PlayerStatusViewModel : NotifyPropertyChanged, IPlayerStatusViewModel
    {
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

        public IHarringtonMViewModel HarringtonM { get; set; }
    }
}
namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.ViewModels;

    public class PlayerOverlayViewModel : NotifyPropertyChanged, IPlayerOverlayViewModel
    {
        public PlayerOverlayViewModel(IPlayerStatusViewModel playerStatus)
        {
            PlayerStatus = playerStatus;
        }

        public IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber)
        {
            Settings = settings;
            
            return this;
        }

        public IPlayerOverlayViewModel UpdateWith(IPlayerStatisticsViewModel playerStatistics, IConvertedPokerPlayer pokerPlayer)
        {
            throw new NotImplementedException();
        }

        public IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration)
        {
            throw new NotImplementedException();
        }

        // UpdateWith PlayerStatistics and PlayerStatus?
        public ITableOverlaySettingsViewModel Settings { get; set; }

        public IPlayerStatisticsViewModel PlayerStatistics { get; set; }

        public IPlayerStatusViewModel PlayerStatus { get; set; }


        double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                RaisePropertyChanged(() => Left);
            }
        }

        double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                RaisePropertyChanged(() => Top);
            }
        }
    }
}
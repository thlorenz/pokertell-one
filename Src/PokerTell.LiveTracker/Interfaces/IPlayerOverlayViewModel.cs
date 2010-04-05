namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.Interfaces;

    using ViewModels;

    public interface IPlayerOverlayViewModel
    {
        ITableOverlaySettingsViewModel Settings { get; }

        IPlayerStatisticsViewModel PlayerStatistics { get; }

        IPlayerStatusViewModel PlayerStatus { get; set; }

        IPositionViewModel Position { get; }

        string PlayerName { get; }

        bool IsPresentAndHasStatistics { get; }

        ICommand FilterAdjustmentRequestedCommand { get; }

        IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber);

        IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration);

        IPlayerOverlayViewModel UpdateStatusWith(IConvertedPokerPlayer pokerPlayer);

        void UpdateStatisticsWith(IPlayerStatisticsViewModel playerStatistics);

        event Action<IPlayerStatisticsViewModel> FilterAdjustmentRequested;
    }
}
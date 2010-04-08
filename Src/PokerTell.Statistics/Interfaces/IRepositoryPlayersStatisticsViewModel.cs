namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    public interface IRepositoryPlayersStatisticsViewModel
    {
        IFilterPopupViewModel FilterPopup { get; }

        IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; }

        IPlayerStatisticsViewModel SelectedPlayer { get; }

        ICommand FilterAdjustmentRequestedCommand { get; }

        IList<IPlayerIdentity> PlayerIdentities { get;}

        IPlayerIdentity SelectedPlayerIdentity { get; set; }
    }
}
namespace PokerTell.LiveTracker.IntegrationTests.DesignViewModels
{
    using Infrastructure.Interfaces.Statistics;
    using Infrastructure.Services;

    using Microsoft.Practices.Composite.Events;

    using Statistics.ViewModels;
    using Statistics.ViewModels.StatisticsSetDetails;

    using ViewModels;

    public class TableStatisticsDesignModel : TableStatisticsViewModel
    {
        #region Constructors and Destructors

        public TableStatisticsDesignModel(IEventAggregator eventAggregator)
            : base(eventAggregator,
                   new Constructor<IPlayerStatisticsViewModel>(() => null),
                   new DetailedStatisticsAnalyzerViewModel(
                       new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPreFlopStatisticsViewModel()),
                       new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPostFlopActionStatisticsViewModel()),
                       new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPostFlopReactionStatisticsViewModel())))
        {
            Players.Add(new PlayerStatisticsDesignModel("renniweg", 3000, 2000, 1000, 2500, 1500, 500));
            Players.Add(new PlayerStatisticsDesignModel("Greystoke-11", 3001, 2001, 1001, 2501, 1501, 501));
            Players.Add(new PlayerStatisticsDesignModel("satina13", 3002, 2002, 1002, 2502, 1502, 502));
            Players.Add(new PlayerStatisticsDesignModel("salemorguy", 3003, 2003, 1003, 2503, 1503, 503));
            Players.Add(new PlayerStatisticsDesignModel("Khan", 3004, 2004, 1004, 2504, 1504, 504));
            Players.Add(new PlayerStatisticsDesignModel("Raymer", 3005, 2005, 1005, 2505, 1505, 505));
            Players.Add(new PlayerStatisticsDesignModel("Doyle", 3006, 2006, 1006, 2506, 1506, 506));

            SelectedPlayer = Players[0];
        }

        #endregion
    }
}
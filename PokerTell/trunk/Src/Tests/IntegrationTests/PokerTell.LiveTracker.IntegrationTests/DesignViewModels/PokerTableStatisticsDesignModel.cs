namespace PokerTell.LiveTracker.IntegrationTests.DesignViewModels
{
    using Microsoft.Practices.Composite.Events;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class PokerTableStatisticsDesignModel : PokerTableStatisticsViewModel
    {
        #region Constructors and Destructors

        static readonly StubBuilder _stub = new StubBuilder();

        public PokerTableStatisticsDesignModel(IEventAggregator eventAggregator, IHandBrowserViewModel handBrowserViewModel)
            : base(eventAggregator, 
                   new Constructor<IPlayerStatisticsViewModel>(() => null), 
                   new DetailedStatisticsAnalyzerViewModel(
                       new Constructor<IDetailedStatisticsViewModel>(
                           () => new DetailedPreFlopStatisticsViewModel(
                                     handBrowserViewModel, 
                                     _stub.Out<IPreFlopRaiseReactionStatisticsViewModel> (), 
                                     _stub.Out<IDetailedPreFlopStatisticsDescriber>())), 
                       new Constructor<IDetailedStatisticsViewModel>(
                           () => new DetailedPostFlopHeroActsStatisticsViewModel(
                                     handBrowserViewModel, 
                                     _stub.Out<IPostFlopHeroActsRaiseReactionStatisticsViewModel>(), 
                                     _stub.Out<IDetailedPostFlopHeroActsStatisticsDescriber>())), 
                       new Constructor<IDetailedStatisticsViewModel>(
                           () => new DetailedPostFlopHeroReactsStatisticsViewModel(
                                     handBrowserViewModel, 
                                     _stub.Out<IPostFlopHeroReactsRaiseReactionStatisticsViewModel>(), 
                                     _stub.Out<IDetailedPostFlopHeroReactsStatisticsDescriber>()))))
        {
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
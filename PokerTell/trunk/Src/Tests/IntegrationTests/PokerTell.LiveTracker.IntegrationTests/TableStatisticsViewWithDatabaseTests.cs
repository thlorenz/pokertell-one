namespace PokerTell.LiveTracker.IntegrationTests
{
    using System;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Infrastructure.Services;
    using PokerTell.IntegrationTests;
    using PokerTell.LiveTracker.IntegrationTests.DesignWindows;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Dao;
    using PokerTell.PokerHand.ViewModels;
    using PokerTell.Repository;
    using PokerTell.Repository.Interfaces;
    using PokerTell.Repository.NHibernate;
    using PokerTell.Statistics;
    using PokerTell.Statistics.Analyzation;
    using PokerTell.Statistics.Filters;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels;
    using PokerTell.Statistics.ViewModels.Analyzation;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class TableStatisticsViewWithDatabaseTests : DatabaseConnectedPerformanceTests
    {
        const string PokerStars = "PokerStars";

        IUnityContainer _container;

        StubBuilder _stub;

        [Test]
        [RequiresSTA]
        public void UpdateWith_NoFilterSet_ProducesPlayerStatisticsFromDatabase()
        {
            Func<string, IPlayerStatistics> get =
                playerName => _container
                                  .Resolve<IPlayerStatistics>()
                                  .InitializePlayer(playerName, PokerStars)
                                  .UpdateStatistics();

            const string salemorguy = "salemorguy";
            const string greystoke = "Greystoke-11";
            const string renniweg = "renniweg";

            var eventAggregator = new EventAggregator();
            new PlayerStatisticsService(eventAggregator);

            var detailedPreFlopStatisticsViewModelMake =
                new Constructor<IDetailedStatisticsViewModel>(() => _container.Resolve<DetailedPreFlopStatisticsViewModel>());
            var detailedPostFlopHeroActsStatisticsViewModelMake =
                new Constructor<IDetailedStatisticsViewModel>(
                    () => _container.Resolve<DetailedPostFlopHeroActsStatisticsViewModel>());

            var detailedPostFlopHeroReactsStatisticsViewModelMake =
                new Constructor<IDetailedStatisticsViewModel>(
                    () => _container.Resolve<DetailedPostFlopHeroReactsStatisticsViewModel>());

            var detailedStatisticsAnalyzerViewModel =
                new DetailedStatisticsAnalyzerViewModel(detailedPreFlopStatisticsViewModelMake, 
                                                        detailedPostFlopHeroActsStatisticsViewModelMake, 
                                                        detailedPostFlopHeroReactsStatisticsViewModelMake);

            var tableStatisticsViewModel = new PokerTableStatisticsViewModel(
                eventAggregator, 
                new Constructor<IPlayerStatisticsViewModel>(() => new PlayerStatisticsViewModel()), 
                detailedStatisticsAnalyzerViewModel);
            var designWindow = new TableStatisticsDesignWindow(eventAggregator, 
                                                               _container.Resolve<IHandBrowserViewModel>())
                {
                    Topmost = true, DataContext = tableStatisticsViewModel 
                };

            tableStatisticsViewModel.UpdateWith(new[]
                {
                   // get(renniweg), 
                    get(greystoke), 
                    get(salemorguy)
                });

            designWindow.ShowDialog();
        }

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            _container = new UnityContainer()
                .RegisterInstance<IEventAggregator>(new EventAggregator())

                // Converted Constructors
                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterConstructor<IConvertedPokerPlayer, ConvertedPokerPlayer>()
                .RegisterConstructor<IConvertedPokerHand, ConvertedPokerHand>()

                // Daos 
                .RegisterType<IPlayerIdentityDao, PlayerIdentityDao>()
                .RegisterType<IConvertedPokerPlayerDao, ConvertedPokerPlayerDao>()
                .RegisterType<IConvertedPokerHandDao, ConvertedPokerHandDao>()
                .RegisterInstance(_stub.Out<IRepositoryParser>())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IRepository, Repository>()
                .RegisterType<IPlayerStatistics, PlayerStatistics>()

                // Database
                .RegisterInstance(_sessionFactoryManagerStub.Object)

                // Statistics
                .RegisterType<IReactionAnalyzationPreparer, ReactionAnalyzationPreparer>()
                .RegisterTypeAndConstructor<IRaiseReactionAnalyzer, RaiseReactionAnalyzer>()
                .RegisterType<IRaiseReactionsAnalyzer, RaiseReactionsAnalyzer>()
                .RegisterType<IRaiseReactionStatistics, RaiseReactionStatistics>()
                .RegisterType<IRaiseReactionStatisticsBuilder, RaiseReactionStatisticsBuilder>()
                .RegisterType<IHandBrowser, HandBrowser>()
                .RegisterType<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IHandBrowserViewModel, HandBrowserViewModel>()
                .RegisterType<IPreFlopRaiseReactionDescriber, PreFlopRaiseReactionDescriber>()
                .RegisterType<IPostFlopHeroActsRaiseReactionDescriber, PostFlopHeroActsRaiseReactionDescriber>()
                .RegisterType<IPostFlopHeroReactsRaiseReactionDescriber, PostFlopHeroReactsRaiseReactionDescriber>()
                .RegisterType<IPreFlopRaiseReactionStatisticsViewModel, PreFlopRaiseReactionStatisticsViewModel>()
                .RegisterType<IPostFlopHeroActsRaiseReactionStatisticsViewModel, PostFlopHeroActsRaiseReactionStatisticsViewModel>()
                .RegisterType<IPostFlopHeroReactsRaiseReactionStatisticsViewModel, PostFlopHeroReactsRaiseReactionStatisticsViewModel>();
        }

        [Test]
        public void Dependencies_Are_Complete()
        {
            _container.Resolve<IHandBrowserViewModel>();
        }
    }
}
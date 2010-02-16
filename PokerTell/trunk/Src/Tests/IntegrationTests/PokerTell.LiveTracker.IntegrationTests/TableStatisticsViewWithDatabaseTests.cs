namespace PokerTell.LiveTracker.IntegrationTests
{
    using System;
    using System.Windows;

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

    using Statistics.Detailed;

    public class TableStatisticsViewWithDatabaseTests : DatabaseConnectedPerformanceTests
    {
        const string PokerStars = "PokerStars";

        IUnityContainer _container;

        StubBuilder _stub;

        public TableStatisticsViewWithDatabaseTests()
        {
            _Init();
        }

        public Window ConnectToDataBase_LoadSomePLayers_ShowTheirStatistics_InTheLiveStats()
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
                  //  get(renniweg), 
                    get(greystoke), 
                    get(salemorguy)
                });

            designWindow.ShowDialog();
            return designWindow;
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

                /*
                 * Statistics
                */

                // RaiseReactionAnalyzation
                .RegisterType<IReactionAnalyzationPreparer, ReactionAnalyzationPreparer>()
                .RegisterTypeAndConstructor<IRaiseReactionAnalyzer, RaiseReactionAnalyzer>()
                .RegisterType<IRaiseReactionsAnalyzer, RaiseReactionsAnalyzer>()
                .RegisterType<IRaiseReactionStatistics, RaiseReactionStatistics>()
                .RegisterType<IRaiseReactionStatisticsBuilder, RaiseReactionStatisticsBuilder>()
                
                // HandBrowser
                .RegisterType<IHandBrowser, HandBrowser>()
                .RegisterType<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IHandBrowserViewModel, HandBrowserViewModel>()
                
                // Detailed Statistics Describers
                .RegisterType<IDetailedPreFlopStatisticsDescriber, DetailedPreFlopStatisticsDescriber>()
                .RegisterType<IDetailedPostFlopHeroActsStatisticsDescriber, DetailedPostFlopHeroActsStatisticsDescriber>()
                .RegisterType<IDetailedPostFlopHeroReactsStatisticsDescriber, DetailedPostFlopHeroReactsStatisticsDescriber>()
                
                // Raise Reaction Statistics Describers
                .RegisterType<IPreFlopRaiseReactionDescriber, PreFlopRaiseReactionDescriber>()
                .RegisterType<IPostFlopHeroActsRaiseReactionDescriber, PostFlopHeroActsRaiseReactionDescriber>()
                .RegisterType<IPostFlopHeroReactsRaiseReactionDescriber, PostFlopHeroReactsRaiseReactionDescriber>()
                
                // Raise Reaction Statistics ViewModels
                .RegisterType<IPreFlopRaiseReactionStatisticsViewModel, PreFlopRaiseReactionStatisticsViewModel>()
                .RegisterType<IPostFlopHeroActsRaiseReactionStatisticsViewModel, PostFlopHeroActsRaiseReactionStatisticsViewModel>()
                .RegisterType<IPostFlopHeroReactsRaiseReactionStatisticsViewModel, PostFlopHeroReactsRaiseReactionStatisticsViewModel>()
                
                // Valued HoleCards
                .RegisterConstructor<IValuedHoleCards, ValuedHoleCards>()
                .RegisterType<IValuedHoleCardsAverage, ValuedHoleCardsAverage>()

                // Preflop HandStrength Statistics
                .RegisterType<IPreFlopHandStrengthStatistics, PreFlopHandStrengthStatistics>()
                
                // Preflop HandStrengths Statistics Describers
                .RegisterType<IPreFlopUnraisedPotCallingHandStrengthDescriber, PreFlopUnraisedPotCallingHandStrengthDescriber>()
                .RegisterType<IPreFlopRaisedPotCallingHandStrengthDescriber, PreFlopRaisedPotCallingHandStrengthDescriber>()
                .RegisterType<IPreFlopRaisingHandStrengthDescriber, PreFlopRaisingHandStrengthDescriber>()
                
                // Preflop HandStrengths Statistics ViewModels
                .RegisterType<IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel, PreFlopUnraisedPotCallingHandStrengthStatisticsViewModel>()
                .RegisterType<IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel, PreFlopRaisedPotCallingHandStrengthStatisticsViewModel>()
                .RegisterType<IPreFlopRaisingHandStrengthStatisticsViewModel, PreFlopRaisingHandStrengthStatisticsViewModel>();
        }

        [Test]
        public void Dependencies_Are_Complete()
        {
            _container.Resolve<IHandBrowserViewModel>();
        }
    }
}
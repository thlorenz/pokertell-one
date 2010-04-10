namespace PokerTell.Statistics
{
    using System;
    using System.Reflection;

    using Analyzation;

    using Detailed;

    using Filters;

    using Infrastructure;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Tools.Interfaces;

    using ViewModels;
    using ViewModels.Analyzation;
    using ViewModels.StatisticsSetDetails;

    using Views;

    public class StatisticsModule : IModule
    {

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public StatisticsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            // Too slow in obtaining all the player identities
            // GlobalCommands.StartServicesCommand.RegisterCommand(new DelegateCommand<object>(StartServices));

            Log.Info("got initialized.");
        }

        void StartServices(object ignore)
        {
            var repositoryPlayersStatisticsView = _container.Resolve<RepositoryPlayersStatisticsView>();
            var region = _regionManager.Regions[ApplicationProperties.ShellMainRegion];
            region.Add(repositoryPlayersStatisticsView);
            region.Activate(repositoryPlayersStatisticsView);
        }

        void RegisterViewsAndServices()
        {
            _container

                // StatisicsSets
                .RegisterType<IPreFlopStatisticsSetsViewModel, PreFlopStatisticsSetsViewModel>()
                .RegisterType<IPostFlopStatisticsSetsViewModel, PostFlopStatisticsSetsViewModel>()

                // Statistics and ViewModel
                .RegisterTypeAndConstructor<IBackgroundWorker, BackgroundWorkerAdapter>()
                .RegisterTypeAndConstructor<IPlayerStatistics, PlayerStatistics>(() => _container.Resolve<IPlayerStatistics>())
                .RegisterTypeAndConstructor<IPlayerStatisticsViewModel, PlayerStatisticsViewModel>(() => _container.Resolve<IPlayerStatisticsViewModel>())
                .RegisterType<IPlayerStatisticsUpdater, PlayerStatisticsUpdater>()

                // RaiseReactionAnalyzation
                .RegisterType<IReactionAnalyzationPreparer, ReactionAnalyzationPreparer>()
                .RegisterTypeAndConstructor<IRaiseReactionAnalyzer, RaiseReactionAnalyzer>()
                .RegisterType<IRaiseReactionsAnalyzer, RaiseReactionsAnalyzer>()
                .RegisterType<IRaiseReactionStatistics, RaiseReactionStatistics>()
                .RegisterType<IRaiseReactionStatisticsBuilder, RaiseReactionStatisticsBuilder>()

                // RepositoryHandBrowser
                .RegisterType<IRepositoryHandBrowser, RepositoryHandBrowser>()
                .RegisterType<IRepositoryHandBrowserViewModel, RepositoryHandBrowserViewModel>()
                
                // Detailed Statistics Describers
                .RegisterType<IDetailedPreFlopStatisticsDescriber, DetailedPreFlopStatisticsDescriber>()
                .RegisterType<IDetailedPostFlopHeroActsStatisticsDescriber, DetailedPostFlopHeroActsStatisticsDescriber>()
                .RegisterType<IDetailedPostFlopHeroReactsStatisticsDescriber, DetailedPostFlopHeroReactsStatisticsDescriber>()
                
                // Detailed Statistics ViewModels
                .RegisterType<IDetailedPreFlopStatisticsViewModel, DetailedPreFlopStatisticsViewModel>()
                .RegisterType<IDetailedPostFlopHeroActsStatisticsViewModel, DetailedPostFlopHeroActsStatisticsViewModel>()
                .RegisterType<IDetailedPostFlopHeroReactsStatisticsViewModel, DetailedPostFlopHeroReactsStatisticsViewModel>()

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
                .RegisterType<IPreFlopRaisingHandStrengthStatisticsViewModel, PreFlopRaisingHandStrengthStatisticsViewModel>()

                // Preflop HandStrengths Visualizing
                .RegisterType<IPreFlopStartingHandsVisualizer, PreFlopStartingHandsVisualizer>()
                .RegisterType<IPreFlopStartingHandsVisualizerViewModel, PreFlopStartingHandsVisualizerViewModel>()

                // Detailed Statistics Analyzer 
                .RegisterConstructor(() => _container.Resolve<IDetailedPreFlopStatisticsViewModel>())
                .RegisterConstructor(() => _container.Resolve<IDetailedPostFlopHeroActsStatisticsViewModel>())
                .RegisterConstructor(() => _container.Resolve<IDetailedPostFlopHeroReactsStatisticsViewModel>())
                .RegisterType<IDetailedStatisticsAnalyzerViewModel, DetailedStatisticsAnalyzerViewModel>()
                
                // Player Filter 
                .RegisterType<IFilterPopupViewModel, FilterPopupViewModel>()
                .RegisterConstructor<IAnalyzablePokerPlayersFilterViewModel, AnalyzablePokerPlayersFilterViewModel>()
                .RegisterType<IAnalyzablePokerPlayersFilterAdjustmentViewModel, AnalyzablePokerPlayersFilterAdjustmentViewModel>()

                //Repository Players Statistics ViewModel
                .RegisterType<IActiveAnalyzablePlayersSelector, ActiveAnalyzablePlayersSelector>()
                .RegisterType<IRepositoryPlayersStatisticsViewModel, RepositoryPlayersStatisticsViewModel>()
                ;
        }
    }
}
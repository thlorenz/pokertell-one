namespace PokerTell.Statistics
{
    using System;

    using Analyzation;

    using Detailed;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using ViewModels.Analyzation;

    public class StatisticsModule : IModule
    {
        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public StatisticsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
           _container
                .RegisterType<IPlayerStatistics, PlayerStatistics>()

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
                .RegisterType<IPreFlopStartingHandsVisualizerViewModel, PreFlopStartingHandsVisualizerViewModel>();
        }
    }
}
namespace PokerTell.LiveTracker.IntegrationTests
{
   using System;

   using DesignWindows;

   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Interfaces.Repository;
   using Infrastructure.Interfaces.Statistics;
   using Infrastructure.Services;

   using Microsoft.Practices.Composite.Events;
   using Microsoft.Practices.Unity;

   using Moq;

   using NUnit.Framework;

   using PokerHand.Analyzation;
   using PokerHand.Dao;

   using PokerTell.IntegrationTests;

   using Repository;
   using Repository.Interfaces;
   using Repository.NHibernate;

   using Statistics;
   using Statistics.Filters;
   using Statistics.Interfaces;
   using Statistics.ViewModels;
   using Statistics.ViewModels.StatisticsSetDetails;

   using ViewModels;

   public class TableStatisticsViewWithDatabaseTests : DatabaseConnectedPerformanceTests
   {
      const string PokerStars = "PokerStars";

      IUnityContainer _container;

      StubBuilder _stub;

      IHandBrowserViewModel _handBrowserViewModelStub;

      IRaiseReactionStatisticsBuilder _raiseReactionStatisticsBuilder;

      IPostFlopHeroActsRaiseReactionDescriber _raiseReactionDescriber;

      [Test]
      public void UpdateWith_NoFilterSet_ProducesPlayerStatisticsFromDatabase()
      {
         SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");

         Func<string, IPlayerStatistics> get =
            playerName => _container
                             .RegisterInstance(_sessionFactoryManagerStub.Object)
                             .Resolve<IPlayerStatistics>()
                             .InitializePlayer(playerName, PokerStars)
                             .UpdateStatistics();

         const string salemorguy = "salemorguy";
         const string greystoke = "Greystoke-11";
         const string renniweg = "renniweg";

         var eventAggregator = new EventAggregator();
         new PlayerStatisticsService(eventAggregator);

         var detailedStatisticsAnalyzerViewModel = new DetailedStatisticsAnalyzerViewModel(
            new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPreFlopStatisticsViewModel(_handBrowserViewModelStub, _raiseReactionStatisticsBuilder, _raiseReactionDescriber)),
            new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPostFlopActionStatisticsViewModel(_handBrowserViewModelStub,_raiseReactionStatisticsBuilder, _raiseReactionDescriber)),
            new Constructor<IDetailedStatisticsViewModel>(() => new DetailedPostFlopReactionStatisticsViewModel(_handBrowserViewModelStub, _raiseReactionStatisticsBuilder, _raiseReactionDescriber)));

         var tableStatisticsViewModel = new PokerTableStatisticsViewModel(
            eventAggregator,
            new Constructor<IPlayerStatisticsViewModel>(() => new PlayerStatisticsViewModel()),
            detailedStatisticsAnalyzerViewModel);
         var designWindow = new TableStatisticsDesignWindow(eventAggregator, _handBrowserViewModelStub, _raiseReactionStatisticsBuilder, _raiseReactionDescriber)
         { Topmost = true, DataContext = tableStatisticsViewModel };

         tableStatisticsViewModel.UpdateWith(new[] { get(renniweg), get(greystoke), get(salemorguy) });

         designWindow.ShowDialog();
      }

      [SetUp]
      public void _Init()
      {
         _stub = new StubBuilder();

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
            .RegisterType<IPlayerStatistics, PlayerStatistics>();

         _handBrowserViewModelStub = new Mock<IHandBrowserViewModel>().Object;
         _raiseReactionStatisticsBuilder = new Mock<IRaiseReactionStatisticsBuilder>().Object;
         _raiseReactionDescriber = new Mock<IPostFlopHeroActsRaiseReactionDescriber>().Object;

      }
   }
}
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

    using Repository.Interfaces;
    using Repository.NHibernate;

    using Repository;

    using Statistics;
    using Statistics.Filters;
    using Statistics.ViewModels;

    using ViewModels;

    public class TableStatisticsViewWithDatabaseTests : DatabaseConnectedPerformanceTests
    {
        #region Constants and Fields

        const string PokerStars = "PokerStars";

        IUnityContainer _container;

        StubBuilder _stub;

        #endregion

        #region Public Methods

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
        }

        [Test]
        public void UpdateWith_NoFilterSet_ProducesPlayerStatisticsFromDatabase()
        {
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");

            var greystoke = _container
                .RegisterInstance(_sessionFactoryManagerStub.Object)
                .Resolve<IPlayerStatistics>();
            greystoke.InitializePlayer("Greystoke-11", PokerStars)
                .UpdateStatistics();

            var renniweg = _container
                .RegisterInstance(_sessionFactoryManagerStub.Object)
                .Resolve<IPlayerStatistics>();
            renniweg.InitializePlayer("renniweg", PokerStars)
                .UpdateStatistics();

            var eventAggregator = new EventAggregator();
            new PlayerStatisticsService(eventAggregator);

            var tableStatisticsViewModel = new TableStatisticsViewModel(eventAggregator, new Constructor<IPlayerStatisticsViewModel>(() => new PlayerStatisticsViewModel()));
            var designWindow = new TableStatisticsDesignWindow(eventAggregator) { Topmost = true, DataContext = tableStatisticsViewModel };
            
            tableStatisticsViewModel.UpdateWith(new[] { greystoke, renniweg });

            designWindow.ShowDialog();
        }

        #endregion
    }
}
namespace PokerTell.StatisticsIntegrationTests
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;
    using IntegrationTests;
    using PokerHand.Analyzation;
    using PokerHand.Dao;
    using Repository;
    using Repository.Interfaces;
    using Repository.NHibernate;
    using Statistics;

    public class PlayerStatisticsTests : DatabaseConnectedPerformanceTests
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

            var playerStatistics = _container
                .RegisterInstance(_sessionFactoryManagerStub.Object)
                .Resolve<IPlayerStatistics>();

            Timed("UpdateWith_NoFilterSet_ProducesPlayerStatisticsFromDatabase", 
                  () => playerStatistics
                            .InitializePlayer("renniweg", PokerStars)
                            .UpdateStatistics());

            Console.WriteLine(playerStatistics);
        }

        #endregion
    }
}
namespace PokerTell.StatisticsIntegrationTests
{
    using System;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.PokerHandParsers;
    using Infrastructure.Interfaces.Repository;

    using IntegrationTests;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerHand.Analyzation;
    using PokerHand.Dao;

    using Repository;
    using Repository.Interfaces;
    using Repository.NHibernate;

    using Statistics;
    using Statistics.Interfaces;

    public class PlayerStatisticsTests : DatabaseConnectedPerformanceTests
    {
        IUnityContainer _container;

        StubBuilder _stub;

        const string PokerStars = "PokerStars";

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
                            .UpdateFrom(_container.Resolve<IRepository>()));

            playerStatistics
                    .OppBIntoHeroInPosition[(int)Streets.Flop]
                    .ActionSequenceStatistics.Last()
                    .Percentages.ToList().ForEach(Console.WriteLine);
        }
    }
}
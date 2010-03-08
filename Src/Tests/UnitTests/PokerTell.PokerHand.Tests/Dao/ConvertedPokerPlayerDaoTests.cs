namespace PokerTell.PokerHand.Tests.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Base;

    using Fakes;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Moq;

    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Dao;

    using UnitTests.Tools;

    [TestFixture]
    public class ConvertedPokerPlayerDaoTests : InMemoryDatabaseTest
    {
        #region Constants and Fields

        IConvertedPokerPlayerDao _sut;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerPlayerDaoTests()
            : base(typeof(ConvertedPokerPlayerDao).Assembly)
        {
        }

        #endregion

        [SetUp]
        public void _Init()
        {
            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);
            FlushAndClearSession();

            var sessionFactoryManager = new Mock<ISessionFactoryManager>();
            sessionFactoryManager
                .SetupGet(sfm => sfm.CurrentSession)
                .Returns(_session);

            _sut = new ConvertedPokerPlayerDao(sessionFactoryManager.Object);
        }

        [Test]
        public void FindByPlayerIdentity_DatabaseEmpty_ReturnsEmpty()
        {
            var foundPlayers = _sut.FindByPlayerIdentity(1);
            
            foundPlayers.ShouldBeEmpty();
        }

        [Test]
        public void FindByPlayerIdentity_DatabaseContainsOtherPlayersOnly_ReturnsEmpty()
        {
            var players = new List<IConvertedPokerPlayer> { SavePlayer("p1"), SavePlayer("p2") };

            SaveHandWithPlayers(1, players);

            var foundPlayers = _sut.FindByPlayerIdentity(3);

            foundPlayers.ShouldBeEmpty();
        }

        [Test]
        public void FindByPlayerIdentity_DatabaseContainsThisPlayer_ReturnsListContainingPlayer()
        {
            var player1 = SavePlayer("p1");
            var players = new List<IConvertedPokerPlayer> { player1, SavePlayer("p2") };

            SaveHandWithPlayers(1, players);

            var foundPlayers = _sut.FindByPlayerIdentity(player1.PlayerIdentity.Id);

            foundPlayers.ShouldContain(player1);
        }

         [Test]
        public void FindAnalyzablePlayersWith_DatabaseEmpty_ReturnsEmptyList()
        {
             var analyzedPlayers = _sut.FindAnalyzablePlayersWithLegacy(1, 0);

             analyzedPlayers.ShouldBeEmpty();
        }

         [Test]
         public void FindAnalyzablePlayersWith_DatabaseContainsPlayer_ReturnsListContainingAnalyzedPlayerWithSameId()
         {
             var player1 = SavePlayer("p1");
             var players = new List<IConvertedPokerPlayer> { player1, SavePlayer("p2") };

             SaveHandWithPlayers(1, players);

             var foundPlayers = _sut.FindAnalyzablePlayersWith(player1.PlayerIdentity.Id, 0);

             foundPlayers.First().Id.ShouldBeEqualTo(player1.Id);
         }

         [Test]
         public void FindAnalyzablePlayersWith_DatabaseContainsPlayerTwice_ReturnsListContainingTwoAnalyzedPlayers()
         {
             var player1 = SavePlayer("p1");
             var players = new List<IConvertedPokerPlayer> { player1, SavePlayer("p2") };

             SaveHandWithPlayers(1, players);
             
             _session.Save(player1);

             var foundPlayers = _sut.FindAnalyzablePlayersWith(player1.PlayerIdentity.Id, 0);

             foundPlayers.ShouldHaveCount(2);
         }

         [Test]
         public void FindAnalyzablePlayersWith_LastQueriedIdIsOneDatabaseContainsPlayerTwice_ReturnsListContainingOneAnalyzedPlayer()
         {
             var player1 = SavePlayer("p1");
             var players = new List<IConvertedPokerPlayer> { player1, SavePlayer("p2") };

             SaveHandWithPlayers(1, players);

             _session.Save(player1);

             var foundPlayers = _sut.FindAnalyzablePlayersWith(player1.PlayerIdentity.Id, 1);

             foundPlayers.ShouldHaveCount(1);
         }

        IConvertedPokerPlayer SavePlayer(string name)
        {
            var playerIdentity = new PlayerIdentity(name, "someSite");
            _session.Save(playerIdentity);
            return new ConvertedPokerPlayer
                { Name = name, PlayerIdentity = playerIdentity };
        }

        void SaveHandWithPlayers(ulong gameId, IEnumerable<IConvertedPokerPlayer> players)
        {
            var hand = new ConvertedPokerHand("someSite", gameId, DateTime.MinValue, 10, 5, players.Count());
            foreach (var player in players)
            {
                hand.AddPlayer(player);
            }

            _session.Save(hand);

            FlushAndClearSession();
        }
    }
}
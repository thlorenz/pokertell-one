namespace PokerTell.PokerHand.Tests.Mapping
{
    using System;
    using System.Linq;

    using Base;

    using Infrastructure.Interfaces.PokerHand;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests.Tools;

    public class PlayerIdentityMapTests : InMemoryDatabaseTest
    {
        IPlayerIdentity _identity;

        IConvertedPokerHand _hand;

        public PlayerIdentityMapTests()
            : base(typeof(PlayerIdentity).Assembly)
        {
        }

        const string Name = "someName";

        const string Site = "someSite";

        [SetUp]
        public void _Init()
        {
            _session.Clear();

            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);

            _identity = new PlayerIdentity(Name, Site);

            _hand = new ConvertedPokerHand("someSite", 1, DateTime.MinValue, 2, 1, 2);
        }

        [Test]
        public void Save_UnsavedPlayerIdentity_SetsId()
        {
            _session.Save(_identity);

            _identity.Id.IsNotEqualTo(UnsavedValue);
        }

        [Test]
        public void Get_SavedPlayerIdentity_ReturnsSavedIdentity()
        {
            _session.Save(_identity);

            FlushAndClearSession();

            var retrievedIdentity = _session.Get<PlayerIdentity>(_identity.Id);

            retrievedIdentity.IsEqualTo(_identity);
        }

        [Test]
        public void Get_SavedPlayerIdentityOnePlayerWithThatIdentityInDatabase_RestoresPlayerLazy()
        {
            _session.Save(_identity);

            var player = new ConvertedPokerPlayer { Name = "somePlayer", PlayerIdentity = _identity };
            _hand.AddPlayer(player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedIdentity = _session.Get<PlayerIdentity>(_identity.Id);

            FlushAndClearSession();

            Assert.Throws<LazyInitializationException>(() => 
                retrievedIdentity.ConvertedPlayers.First());
        }

        [Test]
        public void Get_SavedPlayerIdentityOnePlayerWithThatIdentityInDatabase_RestoredPlayerIsEqualStoredPlayer()
        {
            _session.Save(_identity);

            var player = new ConvertedPokerPlayer { Name = "somePlayer", PlayerIdentity = _identity };
            _hand.AddPlayer(player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedIdentity = _session.Get<PlayerIdentity>(_identity.Id);

            retrievedIdentity.ConvertedPlayers.First().IsEqualTo(player);
        }

        [Test]
        public void Get_SavedPlayerIdentityTwoPlayersFromDifferentHandsWithThatIdentityInDatabase_RestoredPlayersAreEqualToStoredPlayers()
        {
            _session.Save(_identity);

            var player1 = new ConvertedPokerPlayer { Name = "somePlayer", PlayerIdentity = _identity };
            _hand.AddPlayer(player1);
            _session.Save(_hand);

            FlushAndClearSession();

            var otherHand = new ConvertedPokerHand("someSite", 1, DateTime.MinValue, 2, 1, 2);

            var player2 = new ConvertedPokerPlayer { Name = "somePlayer", PlayerIdentity = _identity };
            otherHand.AddPlayer(player2);
            _session.Save(otherHand);

            var retrievedIdentity = _session.Get<PlayerIdentity>(_identity.Id);

            retrievedIdentity.ConvertedPlayers.First().IsEqualTo(player1);
            retrievedIdentity.ConvertedPlayers.Last().IsEqualTo(player2);
        }
    }
}
namespace PokerTell.PokerHand.Tests.Mapping
{
    using System;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests.Tools;

    public class ConvertedPokerPlayerMapTests : InMemoryDatabaseTest
    {
        public ConvertedPokerPlayerMapTests()
            : base(typeof(ConvertedPokerPlayer).Assembly)
        {
        }

        IConvertedPokerPlayer _player;

        IConvertedPokerHand _hand;

        const string Name = "someName";

        const string Site = "someSite";

        [SetUp]
        public void _Init()
        {
            _session.Clear();

            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);

            _hand = new ConvertedPokerHand("someSite", 1, DateTime.MinValue, 2, 1, 2);

            var playerIdentity = new PlayerIdentity(Name, Site);
            _session.Save(playerIdentity);
            _player = new ConvertedPokerPlayer
                {
                    PlayerIdentity = playerIdentity
                };
        }

        [Test]
        public void SaveParentHand_UnsavedPlayer_SetsId()
        {
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            _player.Id.ShouldNotBeEqualTo(UnsavedValue);
        }

        [Test]
        public void Get_SavedPlayer_RestoresPlayerPosition()
        {
            _player.Position = 1;
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.Position.ShouldBeEqualTo(_player.Position);
        }

        [Test]
        public void Get_SavedPlayer_RestoresPlayerHolecards()
        {
            _player.Holecards = "someHolecards";
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.Holecards.ShouldBeEqualTo(_player.Holecards);
        }

        [Test]
        public void Get_SavedPlayer_RestoresPlayerName()
        {
            _player.Name = "someName";
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.Name.ShouldBeEqualTo(_player.Name);
        }

        [Test]
        public void Get_SavedPlayer_RestoresMBefore()
        {
            _player.MBefore = 1;
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.MBefore.ShouldBeEqualTo(_player.MBefore);
        }

        [Test]
        public void Get_SavedPlayer_RestoresStategicPosition()
        {
            _player.Position = 1;
            _player.SetStrategicPosition(_hand.TotalPlayers);
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.StrategicPosition.ShouldBeEqualTo(_player.StrategicPosition);
        }

        [Test]
        public void Get_SavedPlayer_RestoresInPositionArray()
        {
            _player.InPosition[(int)Streets.PreFlop] = true;
            _player.InPosition[(int)Streets.Flop] = true;
            _player.InPosition[(int)Streets.Turn] = true;
            _player.InPosition[(int)Streets.River] = true;

            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.InPosition.ShouldBeEqualTo(_player.InPosition);
        }

        [Test]
        public void Get_SavedPlayer_RestoresRoundsArray()
        {
            var sampleRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(ActionTypes.B, 1.0));
          
            _player
                .Add(sampleRound)
                .Add(sampleRound)
                .Add(sampleRound)
                .Add(sampleRound);

            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.Rounds.ShouldBeEqualTo(_player.Rounds);
        }

        [Test]
        public void Get_SavedPlayer_RestoresActionSequencesArray()
        {
            _player.ActionSequences[(int)Streets.PreFlop] = ActionSequences.HeroF;
            _player.ActionSequences[(int)Streets.Flop] = ActionSequences.HeroB;
            _player.ActionSequences[(int)Streets.Turn] = ActionSequences.HeroXOppBHeroF;
            _player.ActionSequences[(int)Streets.River] = ActionSequences.HeroB;

            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.ActionSequences.ShouldBeEqualTo(_player.ActionSequences);
        }

        [Test]
        public void Get_SavedPlayer_RestoresBetSizeIndexesArray()
        {
            _player.BetSizeIndexes[(int)Streets.Flop] = 1;
            _player.BetSizeIndexes[(int)Streets.Turn] = 2;
            _player.BetSizeIndexes[(int)Streets.River] = 3;

            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            IConvertedPokerPlayer retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.BetSizeIndexes.ShouldBeEqualTo(_player.BetSizeIndexes);
        }

        [Test]
        public void Get_SavedPlayer_RestoresPlayerParentHandAsProxy()
        {
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);
            
            retrievedPlayer.ParentHand.GetType().Name.Contains("Proxy").ShouldBeTrue();
        }

        [Test]
        public void Get_SavedPlayer_ProxiedParentHandIsEqualToPlayersHand()
        {
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);
         
            FlushAndClearSession();

            var proxiedHandRetrieved = _session.Get<ConvertedPokerHand>(retrievedPlayer.ParentHand.Id);

            proxiedHandRetrieved.ShouldBeEqualTo(_hand);
        }

        [Test]
        public void Get_SavedHandWithOnePlayer_RestoresPlayersPlayerIdentityAsProxy()
        {
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            retrievedPlayer.PlayerIdentity.GetType().Name.Contains("Proxy").ShouldBeTrue();
        }

        [Test]
        public void Get_SavedPlayer_ProxiedPlayerIdentityIsEqualToPlayersPlayerIdentity()
        {
            _hand.AddPlayer(_player);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedPlayer = _session.Get<ConvertedPokerPlayer>(_player.Id);

            FlushAndClearSession();

            var proxiedIdentityRetrieved = _session.Get<PlayerIdentity>(retrievedPlayer.PlayerIdentity.Id);

            proxiedIdentityRetrieved.ShouldBeEqualTo(_player.PlayerIdentity);
        }
    }
}
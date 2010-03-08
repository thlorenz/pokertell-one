namespace PokerTell.PokerHand.Tests.Mapping
{
    using System;
    using System.Linq;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using log4net.Core;

    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests;
    using UnitTests.Tools;

    public class ConvertedPokerHandMapTests : InMemoryDatabaseTest
    {
        public ConvertedPokerHandMapTests()
            : base(typeof(ConvertedPokerHand).Assembly)
        {
        }

        const double BB = 20.0;

        const ulong GameId = 1;

        const double SB = 10.0;

        const string Name1 = "player1";
        const string Name2 = "player2";

        const string Site = "PokerStars";

        const int TotalPlayers = 9;

        readonly DateTime _timeStamp = DateTime.MinValue;

        IConvertedPokerHand _hand;

        IPlayerIdentity _playerIdentity1;
        IPlayerIdentity _playerIdentity2;

        [SetUp]
        public void _Init()
        {
            _session.Clear();

            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);
           
            _hand = new ConvertedPokerHand(Site, GameId, _timeStamp, BB, SB, TotalPlayers);
            
            _playerIdentity1 = new PlayerIdentity(Name1, Site);
            _playerIdentity2 = new PlayerIdentity(Name2, Site);
        }

        [Test]
        public void Save_UnsavedHand_SetsId()
        {
            _session.Save(_hand);

            _hand.Id.ShouldNotBeEqualTo(UnsavedValue);
        }

        [Test]
        public void Get_SavedHand_ReturnsThatHand()
        {
            _hand.Ante = 1;
            _hand.Board = "someBoard";
            _hand.TableName = "someTable";
            _hand.TotalSeats = 1;
            _hand.TournamentId = 1;

            _session.Save(_hand);

            FlushAndClearSession();
           
            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.ShouldBeEqualTo(_hand);
            
        }

        [Test]
        public void Get_SavedHand_RestoresPlayersInRoundArray()
        {
            _hand.PlayersInRound[(int)Streets.Flop] = 3;
            _hand.PlayersInRound[(int)Streets.Turn] = 2;
            _hand.PlayersInRound[(int)Streets.River] = 1;

            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.PlayersInRound.ShouldBeEqualTo(_hand.PlayersInRound);
        }

        [Test]
        public void Get_SavedHand_RestoresSequencesArray()
        {
            var samplePreflopRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(ActionTypes.C, 1.0));
            var sampleFlopRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(ActionTypes.B, 1.0));
            var sampleTurnRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(ActionTypes.R, 1.0));
            var sampleRiverRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(ActionTypes.B, 2.0));

            _hand.Sequences[(int)Streets.PreFlop] = samplePreflopRound;
            _hand.Sequences[(int)Streets.Flop] = sampleFlopRound;
            _hand.Sequences[(int)Streets.Turn] = sampleTurnRound;
            _hand.Sequences[(int)Streets.River] = sampleRiverRound;

            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.Sequences.ShouldBeEqualTo(_hand.Sequences);
        }

        [Test]
        public void Get_SavedHandWithTwoPlayers_RestoresPlayers()
        {
            _session.Save(_playerIdentity1);
            _session.Save(_playerIdentity2);

            var player1 = new ConvertedPokerPlayer { PlayerIdentity = _playerIdentity1 };
            var player2 = new ConvertedPokerPlayer { PlayerIdentity = _playerIdentity2 };
            _hand
                .AddPlayer(player1)
                .AddPlayer(player2);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.Players.First().Id.ShouldBeEqualTo(player1.Id);
            retrievedHand.Players.Last().Id.ShouldBeEqualTo(player2.Id);
        }

        [Test]
        public void Get_SavedHandWithOnePlayer_RestoresPlayersProperties()
        {
            _session.Save(_playerIdentity1);

            var player1 = new ConvertedPokerPlayer { PlayerIdentity = _playerIdentity1, Holecards = "hisCards", Position = 1 };
            _hand
                .AddPlayer(player1);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.Players.First().Holecards.ShouldBeEqualTo(player1.Holecards);
            retrievedHand.Players.First().Position.ShouldBeEqualTo(player1.Position);
        }

        [Test]
        public void Get_SavedHandWithOnePlayer_RestoresPlayersParentHand()
        {
            _session.Save(_playerIdentity1);

            var player1 = new ConvertedPokerPlayer { PlayerIdentity = _playerIdentity1 };
            _hand
                .AddPlayer(player1);
            _session.Save(_hand);

            FlushAndClearSession();

            var retrievedHand = _session.Get<ConvertedPokerHand>(_hand.Id);

            retrievedHand.Players.First().ParentHand.ShouldBeEqualTo(_hand);
        }

        
    }
}
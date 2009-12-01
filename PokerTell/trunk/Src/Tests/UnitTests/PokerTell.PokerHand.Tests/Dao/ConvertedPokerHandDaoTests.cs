namespace PokerTell.PokerHand.Tests.Dao
{
    using System;
    using System.Linq;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Dao;

    using UnitTests.Tools;

    [TestFixture]
    public class ConvertedPokerHandDaoTests : InMemoryDatabaseTest
    {
        #region Constants and Fields

        const double BB = 20.0;

        const ulong GameId = 1;

        const double SB = 10.0;

        const string Site = "PokerStars";

        const int TotalPlayers = 9;

        readonly DateTime _timeStamp = DateTime.MinValue;

        IConvertedPokerHand _hand;

        ConvertedPokerHandDao _sut;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandDaoTests()
            : base(typeof(ConvertedPokerHand).Assembly)
        {
        }

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            new SchemaExport(_configuration).Execute(false, true, false, _session.Connection, null);

            _hand = new ConvertedPokerHand(Site, GameId, _timeStamp, BB, SB, TotalPlayers);

            _sut = new ConvertedPokerHandDao(_session);
        }

        [TearDown]
        public void _Dispose()
        {
            _session.Clear();
        }

        [Test]
        public void Insert_HandWithoutPlayers_DoesNotInsertItIntoDatabase()
        {
            _sut.Insert(_hand);

            _hand.Id.IsEqualTo(UnsavedValue);
        }

        [Test]
        public void Insert_HandWithTwoPlayers_InsertsItIntoDatabase()
        {
            var player1 = new ConvertedPokerPlayer { Name = "player1" };
            var player2 = new ConvertedPokerPlayer { Name = "player2" };

            _hand
                .AddPlayer(player1)
                .AddPlayer(player2);
                
            _sut.Insert(_hand);
            
            FlushAndClearSession();

            _hand.Id.IsNotEqualTo(UnsavedValue);
        }

        [Test]
        public void Insert_HandWithTwoPlayers_SavesBothPlayers()
        {
            var player1 = new ConvertedPokerPlayer { Name = "player1" };
            var player2 = new ConvertedPokerPlayer { Name = "player2" };

            _hand
                .AddPlayer(player1)
                .AddPlayer(player2);

            _sut.Insert(_hand);

            FlushAndClearSession();

            player1.Id.IsNotEqualTo(UnsavedValue);
            player2.Id.IsNotEqualTo(UnsavedValue);
        }

        [Test]
        public void Insert_HandWithTwoPlayers_SavesBothPlayerIdentities()
        {
            var player1 = new ConvertedPokerPlayer { Name = "player1" };
            var player2 = new ConvertedPokerPlayer { Name = "player2" };

            _hand
                .AddPlayer(player1)
                .AddPlayer(player2);

            _sut.Insert(_hand);

            FlushAndClearSession();

            player1.PlayerIdentity.Id.IsNotEqualTo(UnsavedValue);
            player2.PlayerIdentity.Id.IsNotEqualTo(UnsavedValue);
        }

        [Test]
        public void GetHandWith_HandNotInDatabase_ReturnsNull()
        {
            var retrievedHand = _sut.GetHandWith(GameId, Site);
            retrievedHand.IsNull();
        }

        [Test]
        public void Get_HandNotInDatabase_ReturnsNull()
        {
            const int someNonExistentId = 1;
            var retrievedHand = _sut.Get(someNonExistentId);
            retrievedHand.IsNull();
        }

        [Test]
        public void Get_HandWasSaved_ReturnsSavedHand()
        {
            _session.Save(_hand);
            FlushAndClearSession();

            var retrievedHand = _sut.Get(_hand.Id);
            retrievedHand.IsEqualTo(_hand);
        }

        [Test]
        public void GetHandWith_HandWasSaved_ReturnsSavedHand()
        {
            _session.Save(_hand);
            FlushAndClearSession();
            
            var retrievedHand = _sut.GetHandWith(GameId, Site);
           
            retrievedHand.IsEqualTo(_hand);
        }

        [Test]
        public void Get_HandWithSequencesWasSaved_RestoresSequences()
        {
            var sampleAction = new ConvertedPokerAction(ActionTypes.C, 1.0);
            _hand.Sequences[(int)Streets.PreFlop] =
                new ConvertedPokerRound().Add(new ConvertedPokerActionWithId(sampleAction, 0));
            _hand.Sequences[(int)Streets.Flop] =
                new ConvertedPokerRound().Add(new ConvertedPokerActionWithId(sampleAction, 1));
            _hand.Sequences[(int)Streets.Turn] =
                new ConvertedPokerRound().Add(new ConvertedPokerActionWithId(sampleAction, 2));
            _hand.Sequences[(int)Streets.River] =
                new ConvertedPokerRound().Add(new ConvertedPokerActionWithId(sampleAction, 3));

            _session.Save(_hand);
            FlushAndClearSession();

            IConvertedPokerHand retrievedHand = _sut.Get(_hand.Id);

            retrievedHand.Sequences.AreEqualTo(_hand.Sequences);
        }

        [Test]
        public void Get_HandWithPlayersInRoundWasSaved_RestoresPlayersInRound()
        {
            _hand.PlayersInRound[(int)Streets.Flop] = 3;
            _hand.PlayersInRound[(int)Streets.Turn] = 2;
            _hand.PlayersInRound[(int)Streets.River] = 1;
            
            _session.Save(_hand);
            FlushAndClearSession();

            IConvertedPokerHand retrievedHand = _sut.Get(_hand.Id);

            retrievedHand.PlayersInRound.AreEqualTo(_hand.PlayersInRound);
        }

        [Test]
        public void Get_HandWithTwoPlayerWasSaved_RestoresBothPlayers()
        {
            _hand
                .AddPlayer(SavePlayerIdentityAndReturnPlayerNamed("player1"))
                .AddPlayer(SavePlayerIdentityAndReturnPlayerNamed("player2"));

            _session.Save(_hand);
            FlushAndClearSession();

            IConvertedPokerHand retrievedHand = _sut.Get(_hand.Id);
           
            retrievedHand.Players.AreEqualTo(_hand.Players);
        }

        [Test]
        public void Get_HandWithTwoPlayerWasSaved_RestoresBothPlayerIdentitiesAsProxies()
        {
            _hand
                .AddPlayer(SavePlayerIdentityAndReturnPlayerNamed("player1"))
                .AddPlayer(SavePlayerIdentityAndReturnPlayerNamed("player2"));

            _session.Save(_hand);
            FlushAndClearSession();

            IConvertedPokerHand retrievedHand = _sut.Get(_hand.Id);

            retrievedHand.Players.First().PlayerIdentity.GetType().Name.Contains("Proxy").IsTrue();
            retrievedHand.Players.Last().PlayerIdentity.GetType().Name.Contains("Proxy").IsTrue();
        }

        [Test]
        public void Get_HandWithTwoPlayerWasSaved_FirstRestoredPlayerIdentityProxyPropertiesAreEqualToStoredPlayerIdentity()
        {
            var player1 = SavePlayerIdentityAndReturnPlayerNamed("player1");
            var player2 = SavePlayerIdentityAndReturnPlayerNamed("player2");

            _hand
                .AddPlayer(player1)
                .AddPlayer(player2);

            _session.Save(_hand);
            FlushAndClearSession();

            IConvertedPokerHand retrievedHand = _sut.Get(_hand.Id);

            var proxiedPlayerIdentity1 = retrievedHand.Players.First().PlayerIdentity;

            proxiedPlayerIdentity1.Id.IsEqualTo(player1.PlayerIdentity.Id);
            proxiedPlayerIdentity1.Name.IsEqualTo(player1.PlayerIdentity.Name);
            proxiedPlayerIdentity1.Site.IsEqualTo(player1.PlayerIdentity.Site);
        }

        IConvertedPokerPlayer SavePlayerIdentityAndReturnPlayerNamed(string name)
        {
            IConvertedPokerPlayer player = new ConvertedPokerPlayer
                {
                    Name = name,
                    PlayerIdentity = new PlayerIdentity(name, Site)
                };

            _session.Save(player.PlayerIdentity);

            return player;
        }

        #endregion
    }
}
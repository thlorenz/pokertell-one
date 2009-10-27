namespace PokerTell.PokerHand.Tests
{
    using System;

    using Factories;

    using Infrastructure.Enumerations.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Services;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.UnitTests;

    using UnitTests.Tools;

    public class ThatConvertedPokerHand : TestWithLog
    {
        #region Constants and Fields

        IConvertedPokerHand _convertedHand;

        IConstructor<IConvertedPokerPlayer> _convertedPlayerMake;

        IAquiredPokerHand _aquiredHand;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
           
            _aquiredHand = new AquiredPokerHand().InitializeWith(
                _stub.Valid(For.Site, "site"),
                _stub.Out<ulong>(For.GameId),
                _stub.Out<DateTime>(For.TimeStamp),
                _stub.Valid(For.SB, 1.0),
                _stub.Valid(For.BB, 2.0),
                _stub.Valid(For.TotalPlayers, 2));

            _convertedHand = new ConvertedPokerHand(_aquiredHand);

            _convertedPlayerMake
                = new Constructor<IConvertedPokerPlayer>(() => new ConvertedPokerPlayer());
        }

        [Test]
        public void AddPlayersFrom_NoPlayers_AddsNoPlayers()
        {
            _convertedHand.AddPlayersFrom(_aquiredHand, _stub.Out<double>(For.StartingPot), _convertedPlayerMake);

            Assert.That(_convertedHand.Players.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddPlayersFrom_TwoPlayers_AddsTwoPlayers()
        {
            _stub.Value(For.HoleCards).Is(string.Empty);

            var player1Stub = _stub.Setup<IAquiredPokerPlayer>()
                 .Get(p => p.Name).Returns("player1")
                 .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards));
            var player2Stub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player2")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards));
          
            _aquiredHand
                .AddPlayer(player1Stub.Out)
                .AddPlayer(player2Stub.Out);

            _convertedHand.AddPlayersFrom(_aquiredHand, _stub.Out<double>(For.StartingPot), _convertedPlayerMake);

            Assert.That(_convertedHand.Players.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddPlayersFrom_ThreePlayers_AddsThreePlayersInSameOrder()
        {
            _stub.Value(For.HoleCards).Is(string.Empty);

            var player1Stub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player1")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards)).Out;
            var player2Stub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player2")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards)).Out;
            var player3Stub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player3")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards)).Out;
              
            _aquiredHand
                .AddPlayer(player1Stub)
                .AddPlayer(player2Stub)
                .AddPlayer(player3Stub);

            _convertedHand
                .AddPlayersFrom(_aquiredHand, _stub.Out<double>(For.StartingPot), _convertedPlayerMake);
            
            var addedInSameOrder =
                _convertedHand[0].Name.Equals(_aquiredHand[0].Name)
                && _convertedHand[1].Name.Equals(_aquiredHand[1].Name)
                && _convertedHand[2].Name.Equals(_aquiredHand[2].Name);
                
            Assert.That(addedInSameOrder);
        }

        [Test]
        public void AddPlayersFrom_OnePlayer_CalculatesMBeforeCorrectly()
        {
            _stub
                .Value(For.HoleCards).Is(string.Empty)
                .Value(For.StartingPot).Is(5.0)
                .Value(For.StackBefore).Is(100.7);

            var playerStub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player1")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards))
                .Get(p => p.StackBefore).Returns(_stub.Out<double>(For.StackBefore)).Out;

            _aquiredHand
                .AddPlayer(playerStub);

            _convertedHand
                .AddPlayersFrom(_aquiredHand, _stub.Out<double>(For.StartingPot), _convertedPlayerMake);

            var expectedValue = (int) playerStub.StackBefore / _stub.Get<double>(For.StartingPot);
            Assert.That(_convertedHand[0].MBefore, Is.EqualTo(expectedValue));
        }

        [Test]
        public void AddPlayersFrom_OnePlayer_CalculatesMAfterCorrectly()
        {
            _stub
                .Value(For.HoleCards).Is(string.Empty)
                .Value(For.StartingPot).Is(5.0)
                .Value(For.StackAfter).Is(75.0);

            var playerStub = _stub.Setup<IAquiredPokerPlayer>()
                .Get(p => p.Name).Returns("player1")
                .Get(p => p.Holecards).Returns(_stub.Out<string>(For.HoleCards))
                .Get(p => p.StackAfter).Returns(_stub.Out<double>(For.StackAfter)).Out;

            _aquiredHand
                .AddPlayer(playerStub);

            _convertedHand
                .AddPlayersFrom(_aquiredHand, _stub.Out<double>(For.StartingPot), _convertedPlayerMake);

            var expectedValue = (int) playerStub.StackAfter / _stub.Get<double>(For.StartingPot);
            Assert.That(_convertedHand[0].MAfter, Is.EqualTo(expectedValue));
        }

        [Test]
        public void RemoveInactivePlayers_PlayerHasNoRound_RemovesHim()
        {
            var convertedPlayer = new ConvertedPokerPlayer();

            _convertedHand
                .AddPlayer(convertedPlayer)

                .RemoveInactivePlayers();

            Assert.That(_convertedHand.Players.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveInactivePlayers_PlayerHasOneRoundWithoutActions_RemovesHim()
        {
            var convertedPlayer =
                new ConvertedPokerPlayer()
                    .Add();

            _convertedHand
                .AddPlayer(convertedPlayer)

                .RemoveInactivePlayers();

            Assert.That(_convertedHand.Players.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveInactivePlayers_PlayerHasOneRoundWithOneAction_DoesntRemoveHim()
        {
            var convertedPlayer =
                new ConvertedPokerPlayer()
                    .Add(
                    new ConvertedPokerRound()
                        .Add(new ConvertedPokerAction(ActionTypes.F, 1.0)));
                    
            _convertedHand
                .AddPlayer(convertedPlayer)

                .RemoveInactivePlayers();

            Assert.That(_convertedHand.Players.Count, Is.EqualTo(1));
        }

        [Test]
        public void SetNumberOfPlayersInEachRound_ThreePreflopPlayers_SetsPlayersInPreflopRoundToThree()
        {
            _convertedHand
                .AddPlayer(new ConvertedPokerPlayer().Add())
                .AddPlayer(new ConvertedPokerPlayer().Add())
                .AddPlayer(new ConvertedPokerPlayer().Add())
                .SetNumOfPlayersInEachRound();

            Assert.That(_convertedHand.PlayersInRound[(int)Streets.PreFlop], Is.EqualTo(3));
        }

        [Test]
        public void SetNumberOfPlayersInEachRound_ThreePlayersTwoHaveFlopRound_SetsPlayersInFlopRoundToTwo()
        {
            _convertedHand
               .AddPlayer(new ConvertedPokerPlayer().Add().Add())
               .AddPlayer(new ConvertedPokerPlayer().Add())
               .AddPlayer(new ConvertedPokerPlayer().Add().Add())

               .SetNumOfPlayersInEachRound();

            Assert.That(_convertedHand.PlayersInRound[(int)Streets.Flop], Is.EqualTo(2));
        }

        [Test]
        public void SetWhoHasPositionInEachRound_TwoPlayersPreflop_SetsSecondPlayerInPositionPreflopToOne()
        {
            var player1 = new ConvertedPokerPlayer().Add();
            var player2 = new ConvertedPokerPlayer().Add();

            _convertedHand
                .AddPlayer(player1)
                .AddPlayer(player2)

                .SetWhoHasPositionInEachRound();

            Assert.That(player2.InPosition[(int)Streets.PreFlop], Is.EqualTo(1));
        }

        [Test]
        public void SetWhoHasPositionInEachRound_TwoPlayersPreflop_SetsFirstPlayerInPositionPreflopToZero()
        {
            var player1 = new ConvertedPokerPlayer().Add();
            var player2 = new ConvertedPokerPlayer().Add();

            _convertedHand
                .AddPlayer(player1)
                .AddPlayer(player2)

                .SetWhoHasPositionInEachRound();

            Assert.That(player1.InPosition[(int)Streets.PreFlop], Is.EqualTo(0));
        }

        [Test]
        public void AffirmIsEqualTo_InitializedHandsAreEqual_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = InitializeConvertedHandWithSomeValidValues();

            Affirm.That(hand1).IsEqualTo(hand2);
        }

        [Test]
        public void AffirmIsEqualTo_HandsWithSamePlayersAreEqual_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(new ConvertedPokerPlayer());
            var hand2 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(new ConvertedPokerPlayer());

            Affirm.That(hand1).IsEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsWithDifferentNumberOfPlayersAreEqual_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(new ConvertedPokerPlayer())
                .AddPlayer(new ConvertedPokerPlayer());
            var hand2 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(new ConvertedPokerPlayer());

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsWithDifferentPlayersAreEqual_Passes()
        {
            var player1 = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            var player2 = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player2.Name = player2.Name + "difference";
            
            var hand1 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(player1);
            var hand2 = InitializeConvertedHandWithSomeValidValues()
                .AddPlayer(player2);

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsGameIdsAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = InitializeConvertedHandWithSomeValidValues();
            hand2.HandId = hand2.HandId + 1;

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsSitesAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = new ConvertedPokerHand(
                hand1.Site + "different", hand1.GameId, hand1.TimeStamp, hand1.BB, hand1.SB, hand1.TotalPlayers);

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsSmallBlindssAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = new ConvertedPokerHand(
                hand1.Site, hand1.GameId, hand1.TimeStamp, hand1.BB, hand1.SB + 1, hand1.TotalPlayers);

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsTotalPlayerssAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = new ConvertedPokerHand(
                hand1.Site, hand1.GameId, hand1.TimeStamp, hand1.BB, hand1.SB, hand1.TotalPlayers + 1);

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsBigBlindsAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = new ConvertedPokerHand(
                hand1.Site, hand1.GameId, hand1.TimeStamp, hand1.BB + 1, hand1.SB, hand1.TotalPlayers);

            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsTableNamesAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = InitializeConvertedHandWithSomeValidValues();
            hand2.TableName = hand1.TableName + "difference";
            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsTournamentIdsAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = InitializeConvertedHandWithSomeValidValues();
            hand2.TournamentId = hand1.TournamentId + 1;
            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void AffirmIsNotEqualTo_HandsAntesAreDifferent_Passes()
        {
            var hand1 = InitializeConvertedHandWithSomeValidValues();
            var hand2 = InitializeConvertedHandWithSomeValidValues();
            hand2.Ante = hand1.Ante + 1;
            Affirm.That(hand1).IsNotEqualTo(hand2);
        }

        [Test]
        public void BinaryDeserialize_UnInitializedConvertedHand_ReturnsSameHand()
        {
            Affirm.That(_convertedHand.BinaryDeserializedInMemory()).IsEqualTo(_convertedHand);
        }

        [Test]
        public void BinaryDeserialize_InitializedConvertedHand_ReturnsSameHand()
        {
            var convertedHand = InitializeConvertedHandWithSomeValidValues();
            Affirm.That(convertedHand.BinaryDeserializedInMemory()).IsEqualTo(convertedHand);
        }

        [Test]
        public void BinaryDeserialize_ConvertedHandWithEmptyPlayer_ReturnsSameHand()
        {
            var convertedHand = InitializeConvertedHandWithSomeValidValues();
            convertedHand.AddPlayer(new ConvertedPokerPlayer());
            Affirm.That(convertedHand.BinaryDeserializedInMemory()).IsEqualTo(convertedHand);
        }

        [Test]
        public void BinaryDeserialize_ConvertedHandWithInitializedPlayer_ReturnsSameHand()
        {
            var convertedHand = InitializeConvertedHandWithSomeValidValues();
            convertedHand.AddPlayer(ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues());
            Affirm.That(convertedHand.BinaryDeserializedInMemory()).IsEqualTo(convertedHand);
        }

        [Test]
        public void BinaryDeserialize_ConvertedHandTwoInitializedPlayers_ReturnsSameHand()
        {
            var convertedHand = InitializeConvertedHandWithSomeValidValues();
            convertedHand
                .AddPlayer(ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues())
                .AddPlayer(ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues());

            Affirm.That(convertedHand.BinaryDeserializedInMemory()).IsEqualTo(convertedHand);
        }

        #endregion

        static IConvertedPokerHand InitializeConvertedHandWithSomeValidValues()
        {
            return new ConvertedPokerHand().InitializeWith("someSite", 1, DateTime.MinValue, 2.0, 1.0, 6);
        }
    }
}
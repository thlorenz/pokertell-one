namespace PokerTell.PokerHand.Tests
{
    using System;

    using Factories;

    using Fakes;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.UnitTests.Tools;

    [TestFixture]
    internal class ConvertedPokerPlayerTests
    {
        #region Constants and Fields

        const int NinePlayers = 9;

        const int SixPlayers = 6;

        const int TwoPlayers = 2;

        ConvertedPokerPlayer _convertedPlayer;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _convertedPlayer = new ConvertedPokerPlayer();
            _stub = new StubBuilder();
        }

        [Test]
        public void AffirmIsEqualTo_AreEqual_Passes()
        {
            var player1 = new ConvertedPokerPlayer { Name = "player1" };
            var player2 = new ConvertedPokerPlayer { Name = "player1" };

            Affirm.That(player1).IsEqualTo(player2);
        }

        [Test]
        public void AffirmIsEqualTo_SameRoundCount_Passes()
        {
            IConvertedPokerPlayer player1 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(new ConvertedPokerRound());
            IConvertedPokerPlayer player2 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(new ConvertedPokerRound());

            Affirm.That(player1).IsEqualTo(player2);
        }

        [Test]
        public void AffirmIsEqualTo_SameRounds_Passes()
        {
            IConvertedPokerPlayer player1 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.F, 1.0)));
            IConvertedPokerPlayer player2 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.F, 1.0)));

            Affirm.That(player1).IsEqualTo(player2);
        }

        [Test]
        public void AffirmIsNotEqualTo_DifferentNames_Passes()
        {
            IConvertedPokerPlayer player1 = new ConvertedPokerPlayer { Name = "player1" };
            IConvertedPokerPlayer player2 = new ConvertedPokerPlayer { Name = "player2" };

            Affirm.That(player1).IsNotEqualTo(player2);
        }

        [Test]
        public void AffirmIsNotEqualTo_DifferentRoundCount_Passes()
        {
            IConvertedPokerPlayer player1 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(new ConvertedPokerRound());
            IConvertedPokerPlayer player2 = new ConvertedPokerPlayer { Name = "player1" };

            Affirm.That(player1).IsNotEqualTo(player2);
        }

        [Test]
        public void AffirmIsNotEqualTo_SameRoundCountButDifferentRounds_Passes()
        {
            IConvertedPokerPlayer player1 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.F, 1.0)));
            IConvertedPokerPlayer player2 = new ConvertedPokerPlayer { Name = "player1" }
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.C, 1.0)));

            Affirm.That(player1).IsNotEqualTo(player2);
        }

        [Test]
        public void BinaryDeserialized_SerializedInitializedPlayer_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithNonEmptyRound_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.R, 2.0)));

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithOneEmptyRound_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.Add(new ConvertedPokerRound());

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithInPosition_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.InPosition = new[] { _stub.Some(1), _stub.Some(0), _stub.Some(1) };

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithMBefore_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.MBefore = _stub.Some(20);

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_PreflopRaiseInFrontPositionIsNonSerialized_DoesNotSerializeIt()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.PreflopRaiseInFrontPos = _stub.Some(1);

            Assert.That(
                player.BinaryDeserializedInMemory().PreflopRaiseInFrontPos,
                Is.Not.EqualTo(player.PreflopRaiseInFrontPos));
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithMAfter_SerializesIt()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.MAfter = _stub.Some(20);

            Assert.That(
                player.BinaryDeserializedInMemory().MAfter,
                Is.EqualTo(player.MAfter));
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithStrategicPosition_SerializesIt()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.StrategicPosition = _stub.Some(StrategicPositions.BU);

            Assert.That(
                player.BinaryDeserializedInMemory().StrategicPosition,
                Is.EqualTo(player.StrategicPosition));
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithSequence_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.Sequence = new[] { "some", "someMore" };

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedPlayerWithTwoNonEmptyRounds_ReturnsSamePlayer()
        {
            DecapsulatedConvertedPlayer player = ConvertedFactory.InitializeConvertedPokerPlayerWithSomeValidValues();
            player.Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.R, 2.0))
                    .Add(new ConvertedPokerAction(ActionTypes.C, 1.0)));

            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void BinaryDeserialized_SerializedUnInitializedPlayer_ReturnsSamePlayer()
        {
            var player = new ConvertedPokerPlayer();
            Affirm.That(player.BinaryDeserializedInMemory()).IsEqualTo(player);
        }

        [Test]
        public void SetStrategicPosition_HeadsUpPositionOne_SetsStrategicPositionToBigBlind()
        {
            _convertedPlayer.Position = 1;

            _convertedPlayer.SetStrategicPosition(TwoPlayers);

            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(StrategicPositions.BB));
        }

        [Test]
        public void SetStrategicPosition_HeadsUpPositionZero_SetsStrategicPositionToButton()
        {
            _convertedPlayer.Position = 0;

            _convertedPlayer.SetStrategicPosition(TwoPlayers);

            // Special case, since in a headsup situation small blind acts last post Flop 
            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(StrategicPositions.BU));
        }

        [Test]
        [Sequential]
        public void SetStrategicPosition_NinePlayersVaryPositions_SetsStrategicPositionCorrectly(
            [Values(0, 1, 2, 3, 4, 5, 6, 7, 8)] int postion, 
            [Values(
                StrategicPositions.SB, StrategicPositions.BB, StrategicPositions.EA, StrategicPositions.EA, 
                StrategicPositions.MI, StrategicPositions.MI, StrategicPositions.LT, StrategicPositions.CO, 
                StrategicPositions.BU)] StrategicPositions expectedStrategicPosition)
        {
            _convertedPlayer.Position = postion;

            _convertedPlayer.SetStrategicPosition(NinePlayers);

            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(expectedStrategicPosition));
        }

        [Test]
        public void SetStrategicPosition_PositionSmallerZero_SetsPositionToEarly()
        {
            const int illegalPosition = -1;
            _convertedPlayer.Position = illegalPosition;

            _convertedPlayer.SetStrategicPosition(SixPlayers);

            // Logs the error and sets default Position
            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(StrategicPositions.EA));
        }

        [Test]
        [Sequential]
        public void SetStrategicPosition_SixPlayersVaryPositions_SetsStrategicPositionCorrectly(
            [Values(0, 1, 2, 3, 4, 5)] int postion, 
            [Values(
                StrategicPositions.SB, StrategicPositions.BB, StrategicPositions.MI,
                StrategicPositions.LT, StrategicPositions.CO, StrategicPositions.BU)] 
                StrategicPositions expectedStrategicPosition)
        {
            _convertedPlayer.Position = postion;

            _convertedPlayer.SetStrategicPosition(SixPlayers);

            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(expectedStrategicPosition));
        }

        #endregion
    }
    
}
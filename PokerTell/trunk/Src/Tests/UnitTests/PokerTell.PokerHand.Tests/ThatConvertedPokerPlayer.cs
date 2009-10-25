namespace PokerTell.PokerHand.Tests
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NUnit.Framework;

    using Tools.Serialization;

    using UnitTests.Tools;

    [TestFixture]
    internal class ThatConvertedPokerPlayer
    {
        #region Constants and Fields

        const int TwoPlayers = 2;

        const int NinePlayers = 9;
        
        const int SixPlayers = 6;

        const bool WriteXmlToConsole = true;

        ConvertedPokerPlayer _convertedPlayer;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _convertedPlayer = new ConvertedPokerPlayer();
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
        public void SetStrategicPosition_HeadsUpPositionOne_SetsStrategicPositionToBigBlind()
        {
            _convertedPlayer.Position = 1;
            
            _convertedPlayer.SetStrategicPosition(TwoPlayers);

            Assert.That(_convertedPlayer.StrategicPosition, Is.EqualTo(StrategicPositions.BB));
        }

        [Test, Sequential]
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

        [Test, Sequential]
        public void SetStrategicPosition_SixPlayersVaryPositions_SetsStrategicPositionCorrectly(
            [Values(0, 1, 2, 3, 4, 5)] int postion,
            [Values(
                StrategicPositions.SB, StrategicPositions.BB, StrategicPositions.MI,
                StrategicPositions.LT, StrategicPositions.CO, StrategicPositions.BU)] StrategicPositions expectedStrategicPosition)
        {
            _convertedPlayer.Position = postion;

            _convertedPlayer.SetStrategicPosition(SixPlayers);

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
        public void Deserialize_SerializedEmptyPlayer_ReturnsSamePlayer()
        {
            Assert.That(_convertedPlayer.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(_convertedPlayer));
        }

        [Test]
        public void Deserialize_SerializedPlayerWithOneEmptyRound_ReturnsSamePlayer()
        {
            _convertedPlayer
                .Add(new ConvertedPokerRound());

            Assert.That(_convertedPlayer.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(_convertedPlayer));
        }

        #endregion
    }
}
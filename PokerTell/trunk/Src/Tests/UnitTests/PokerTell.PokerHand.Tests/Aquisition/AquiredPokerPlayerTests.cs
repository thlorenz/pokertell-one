namespace PokerTell.PokerHand.Tests.Aquisition
{
    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Aquisition;

    using UnitTests;

    [TestFixture]
    public class AquiredPokerPlayerTests : TestWithLog
    {
        AquiredPokerPlayer _aquiredPlayer;

        const int SmallBlind = 0;

        const int BigBlind = 1;

        #region SetUp/TearDown

        [SetUp]
        public void _Init()
        {
            const double someStack = 1.0;
            const string someName = "test";
            _aquiredPlayer = new AquiredPokerPlayer(someName, someStack);
        }

        #endregion

        [Test]
        public void AddRound_FirstRound_AddsTheRound()
        {
            var aquiredPokerRound = new AquiredPokerRound();

            _aquiredPlayer.AddRound(aquiredPokerRound);

            Assert.That(_aquiredPlayer[0], Is.EqualTo(aquiredPokerRound));
        }

        [Test]
        public void AddRound_FirstRound_IncreasesCountToOne()
        {
            var aquiredPokerRound = new AquiredPokerRound();

            _aquiredPlayer.AddRound(aquiredPokerRound);

            Assert.That(_aquiredPlayer.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddRound_FourRoundsAddedAlready_PreventsRoundFromBeingAdded()
        {
            _aquiredPlayer.AddRound();
            _aquiredPlayer.AddRound();
            _aquiredPlayer.AddRound();
            _aquiredPlayer.AddRound();

            NotLogged(() => _aquiredPlayer.AddRound());

            const int maximumNumberOfRounds = 4;
            Assert.That(_aquiredPlayer.Count, Is.EqualTo(maximumNumberOfRounds));
        }

        [Test]
        public void ChipsGained_NoRoundsAdded_ReturnsZero()
        {
            var aquiredPlayerMock = new AquiredPokerPlayerMock();
            
            Assert.That(aquiredPlayerMock.ChipsGainedProp, Is.EqualTo(0));
        }

        [Test]
        public void ChipsGained_OneRoundAdded_ReturnsChipsGainedInThatRound()
        {
            const double chipsGainedInRound = 1.0;
           
            var roundStub = new Mock<IAquiredPokerRound>();
            roundStub.SetupGet(get => get.ChipsGained).Returns(chipsGainedInRound);
           
            var aquiredPlayerMock = new AquiredPokerPlayerMock();

            aquiredPlayerMock.AddRound(roundStub.Object);

            Assert.That(aquiredPlayerMock.ChipsGainedProp, Is.EqualTo(chipsGainedInRound));
        }

        [Test]
        public void ChipsGained_TwoRoundsAdded_ReturnsSumOfChipsGainedInThoseRounds()
        {
            const double chipsGainedInFirstRound = 1.0;
            const double chipsGainedInSecondRound = -0.5;
            const double expectedGain = chipsGainedInFirstRound + chipsGainedInSecondRound;

            var firstRoundStub = new Mock<IAquiredPokerRound>();   
            var secondRoundStub = new Mock<IAquiredPokerRound>();
            firstRoundStub.SetupGet(get => get.ChipsGained).Returns(chipsGainedInFirstRound);
            secondRoundStub.SetupGet(get => get.ChipsGained).Returns(chipsGainedInSecondRound);

            var aquiredPlayerMock = new AquiredPokerPlayerMock();
            aquiredPlayerMock.AddRound(firstRoundStub.Object);
            aquiredPlayerMock.AddRound(secondRoundStub.Object);

            Assert.That(aquiredPlayerMock.ChipsGainedProp, Is.EqualTo(expectedGain));
        }

        [Test, Sequential]
        public void SetPosition_RelativeSeatIsSmallBlindNoHeadsUpVaryTotalPlayers_SetsPositionToSmallBlind(
            [Values(2, 4, 6, 9)] int totalPlayers,
            [Values(SmallBlind, SmallBlind, SmallBlind, SmallBlind)] int expectedPosition)
        {
            const int sbPosition = 0;
            _aquiredPlayer.RelativeSeatNumber = sbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test, Sequential]
        public void SetPosition_RelativeSeatIsSmallBlindNoHeadsUpVaryTotalPlayers_SetsPositionToButton(
            [Values(4, 6, 9)] int totalPlayers,
            [Values(3, 5, 8)] int expectedPosition)
        {
            int button = totalPlayers - 1;
            const int sbPosition = 0;
            
            _aquiredPlayer.RelativeSeatNumber = button;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test, Sequential]
        public void SetPosition_NinePlayersRelativeSeatIsSmallBlindVarySmallBlindPosition_SetsPositionToSmallBlind(
            [Values(4, 6, 8)] int sbPosition,
            [Values(SmallBlind, SmallBlind, SmallBlind, SmallBlind)] int expectedPosition)
        {
            const int totalPlayers = 9;
            _aquiredPlayer.RelativeSeatNumber = sbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test, Sequential]
        public void SetPosition_NinePlayersRelativeSeatIsButtonAndVarySmallBlindPosition_SetsPositionToButton(
            [Values(4, 6, 9)] int sbPosition,
            [Values(8, 8, 8)] int expectedPosition)
        {
            const int totalPlayers = 9;
            int button = sbPosition - 1;

            _aquiredPlayer.RelativeSeatNumber = button;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SetPosition_HeadsUpSmallBlindIsZeroRelativeSeatIsZero_SetsPositionToSmallBlind()
        {
            const int totalPlayers = 2;
            const int sbPosition = 0;
            const int expectedPosition = SmallBlind;

            _aquiredPlayer.RelativeSeatNumber = sbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SetPosition_HeadsUpSmallBlindIsOneRelativeSeatIsOne_SetsPositionToSmallBlind()
        {
            const int totalPlayers = 2;
            const int sbPosition = 1;
            const int expectedPosition = SmallBlind;

            _aquiredPlayer.RelativeSeatNumber = sbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SetPosition_HeadsUpSmallBlindIsZeroRelativeSeatIsOne_SetsPositionToBigBlind()
        {
            const int totalPlayers = 2;
            const int sbPosition = 0;
            const int bbPosition = 1;
            const int expectedPosition = BigBlind;

            _aquiredPlayer.RelativeSeatNumber = bbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }
        
        [Test]
        public void SetPosition_HeadsUpSmallBlindIsOneRelativeSeatIsZero_SetsPositionToBigBlind()
        {
            const int totalPlayers = 2;
            const int sbPosition = 1;
            const int bbPosition = 0;
            const int expectedPosition = BigBlind;

            _aquiredPlayer.RelativeSeatNumber = bbPosition;
            _aquiredPlayer.SetPosition(sbPosition, totalPlayers);

            Assert.That(_aquiredPlayer.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void SetPosition_RelativeSeatBiggerThanTotalPlayers_ReturnsFalse()
        {
            const int totalPlayers = 6;
            const int sbPosition = 0;

            _aquiredPlayer.RelativeSeatNumber = totalPlayers + 1;
            bool returnedValue = true;
            NotLogged(() => returnedValue = _aquiredPlayer.SetPosition(sbPosition, totalPlayers));

            Assert.That(returnedValue, Is.False);
        }
    }

    internal class AquiredPokerPlayerMock : AquiredPokerPlayer
    {
        public double ChipsGainedProp
        {
            get { return ChipsGained(); }
        }
    }
}
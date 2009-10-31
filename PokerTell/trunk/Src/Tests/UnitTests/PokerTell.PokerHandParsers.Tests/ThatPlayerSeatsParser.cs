namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatPlayerSeatsParser
    {
        PlayerSeatsParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetPlayerSeatsParser();
        }

        [Test]
        public void Parse_EmptyString_PlayerSeatsAreEmpty()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.PlayerSeats.Count, Is.EqualTo(0));
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_OneSeatWithPlayer_IsValidIsFalse()
        {
            const int seatNumber = 1;
            var playerData = CreateSomePlayerData();

            string handHistory = OneSeatWithPlayer(seatNumber, playerData);
          
            _parser.Parse(handHistory);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_OneSeatWithPlayer_AddsPlayerToPlayerSeats()
        {
            const int seatNumber = 1;
            var playerData = CreateSomePlayerData();

            string handHistory = OneSeatWithPlayer(seatNumber, playerData);
           
            _parser.Parse(handHistory);
            Assert.That(_parser.PlayerSeats[seatNumber], Is.EqualTo(playerData));
        }

        [Test]
        public void Parse_OneSeatWithPlayerThatIsOutOfHand_DoesNotAddPlayerToPlayerSeats()
        {
            const int seatNumber = 1;
            var playerData = CreateSomePlayerData();

            string handHistory = OneSeatWithOutOfHandPlayer(seatNumber, playerData);

            _parser.Parse(handHistory);
            Assert.That(_parser.PlayerSeats.Count, Is.EqualTo(0));
        }

        [Test]
        public void Parse_TwoSeatsWithPlayers_IsValidIsTrue()
        {
            const int seatNumber1 = 1;
            const int seatNumber2 = 2;
            const double ratio1 = 1.0;
            const double ratio2 = 2.0;
         
            var playerData1 = CreatePlayerDataFor("player1", ratio1);
            var playerData2 = CreatePlayerDataFor("player2", ratio2);

            string handHistory = TwoSeatsWithPlayers(seatNumber1, playerData1, seatNumber2, playerData2);

            _parser.Parse(handHistory);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_TwoSeatsWithPlayers_AddsFirstPlayerToPlayerSeats()
        {
            const int seatNumber1 = 1;
            const int seatNumber2 = 2;
            const double ratio1 = 1.0;
            const double ratio2 = 2.0;

            var playerData1 = CreatePlayerDataFor("player1", ratio1);
            var playerData2 = CreatePlayerDataFor("player2", ratio2);

            string handHistory = TwoSeatsWithPlayers(seatNumber1, playerData1, seatNumber2, playerData2);

            _parser.Parse(handHistory);
            Assert.That(_parser.PlayerSeats[seatNumber1], Is.EqualTo(playerData1));
        }

        [Test]
        public void Parse_TwoSeatsWithPlayers_AddsSecondPlayerToPlayerSeats()
        {
            const int seatNumber1 = 1;
            const int seatNumber2 = 2;
            const double ratio1 = 1.0;
            const double ratio2 = 2.0;

            var playerData1 = CreatePlayerDataFor("player1", ratio1);
            var playerData2 = CreatePlayerDataFor("player2", ratio2);

            string handHistory = TwoSeatsWithPlayers(seatNumber1, playerData1, seatNumber2, playerData2);

            _parser.Parse(handHistory);
            Assert.That(_parser.PlayerSeats[seatNumber2], Is.EqualTo(playerData2));
        }

        static PlayerSeatsParser.PlayerData CreateSomePlayerData()
        {
            const string playerName = "somePlayer";
            const double ratio = 1.0;
            return CreatePlayerDataFor(playerName, ratio);
        }

        static PlayerSeatsParser.PlayerData CreatePlayerDataFor(string playerName, double ratio)
        {
            return new PlayerSeatsParser.PlayerData(playerName, ratio);
        }


        protected abstract PlayerSeatsParser GetPlayerSeatsParser();

        protected abstract string OneSeatWithPlayer(int seatNumber, PlayerSeatsParser.PlayerData playerData);
        protected abstract string OneSeatWithOutOfHandPlayer(int seatNumber, PlayerSeatsParser.PlayerData playerData);
        protected abstract string TwoSeatsWithPlayers(int seatNumber1, PlayerSeatsParser.PlayerData playerData1, int seatNumber2, PlayerSeatsParser.PlayerData playerData2);
    }
}
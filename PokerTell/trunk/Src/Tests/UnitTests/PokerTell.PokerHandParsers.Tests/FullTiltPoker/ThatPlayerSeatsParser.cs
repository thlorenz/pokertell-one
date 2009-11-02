namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    public class ThatPlayerSeatsParser : Tests.ThatPlayerSeatsParser
    {
        protected override PlayerSeatsParser GetPlayerSeatsParser()
        {
            return new PokerHandParsers.FullTiltPoker.PlayerSeatsParser();
        }

        protected override string OneSeatWithPlayer(int seatNumber, PlayerSeatsParser.PlayerData player)
        {
            // Seat 2: poacher from 71 ($5.25)
            return string.Format("Seat {0}: {1} (${2})", seatNumber, player.Name, player.Stack);
        }

        protected override string OneSeatWithOutOfHandPlayer(int seatNumber, PlayerSeatsParser.PlayerData playerData)
        {
            // Seat 3: Senor Parker (2,730), is sitting out
            return OneSeatWithPlayer(seatNumber, playerData) + ", is sitting out";
        }

        protected override string TwoSeatsWithPlayers(int seatNumber1, PlayerSeatsParser.PlayerData playerData1, int seatNumber2, PlayerSeatsParser.PlayerData playerData2)
        {
            return OneSeatWithPlayer(seatNumber1, playerData1) + "\n" + OneSeatWithPlayer(seatNumber2, playerData2);
        }
    }
}
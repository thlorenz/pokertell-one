namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatPlayerSeatsParser : Tests.ThatPlayerSeatsParser
    {
        protected override PlayerSeatsParser GetPlayerSeatsParser()
        {
            return new PokerHandParsers.PokerStars.PlayerSeatsParser();
        }

        protected override string OneSeatWithPlayer(int seatNumber, PlayerSeatsParser.PlayerData player)
        {
            // Seat 1: Roy`ll flush ($15.95 in chips) 
            return string.Format("Seat {0}: {1} (${2} in chips)", seatNumber, player.Name, player.Stack);
        }

        protected override string OneSeatWithOutOfHandPlayer(int seatNumber, PlayerSeatsParser.PlayerData playerData)
        {
            // Seat 7: brrekk (4985 in chips) out of hand (moved from another table into small blind)
            return OneSeatWithPlayer(seatNumber, playerData) + " out of hand";
        }

        protected override string TwoSeatsWithPlayers(int seatNumber1, PlayerSeatsParser.PlayerData playerData1, int seatNumber2, PlayerSeatsParser.PlayerData playerData2)
        {
            return OneSeatWithPlayer(seatNumber1, playerData1) + "\n" + OneSeatWithPlayer(seatNumber2, playerData2);
        }
    }
}
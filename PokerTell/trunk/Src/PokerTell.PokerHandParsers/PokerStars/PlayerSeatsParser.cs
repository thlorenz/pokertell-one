namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class PlayerSeatsParser : PokerHandParsers.PlayerSeatsParser
    {
        const string SeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \(" 
            + SharedPatterns.RatioPattern 
            + @" in chips\) *(?<OutOfHand>out of hand)*";	

        public override PokerHandParsers.PlayerSeatsParser Parse(string handHistory)
        {
            PlayerSeats = new Dictionary<int, PlayerData>();

            MatchCollection players = MatchAllPlayerSeats(handHistory);

            IsValid = players.Count > 1;

            ExtractAllPlayers(players);

            return this;
        }

        void ExtractAllPlayers(MatchCollection players)
        {
            foreach (Match player in players)
            {
                ExtractPlayer(player);
            }
        }

        void ExtractPlayer(Match player)
        {
            bool playerWasOutOfHand = player.Groups["OutOfHand"].Success;
            if (playerWasOutOfHand)
            {
                return;
            }

            int seatNumber = Convert.ToInt32(player.Groups["SeatNumber"].Value);
           
            if (!PlayerSeats.ContainsKey(seatNumber))
            {
                string playerName = player.Groups["PlayerName"].Value;
                double ratio = Convert.ToDouble(player.Groups["Ratio"].Value);
                PlayerSeats.Add(seatNumber, new PlayerData(playerName, ratio));
            }
        }

        static MatchCollection MatchAllPlayerSeats(string handHistory)
        {
            return Regex.Matches(handHistory, SeatPattern, RegexOptions.IgnoreCase);
        }
    }
}
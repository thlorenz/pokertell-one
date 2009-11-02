using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System.Reflection;

    using log4net;

    public class PlayerSeatsParser : PokerHandParsers.PlayerSeatsParser
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        const string SeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \(" 
            + SharedPatterns.RatioPattern
            + @"\) *(?<OutOfHand>, is sitting out){0,1}";	

        public override PokerHandParsers.PlayerSeatsParser Parse(string handHistory)
        {
            PlayerSeats = new Dictionary<int, PlayerData>();

            MatchCollection players = MatchAllPlayerSeats(handHistory);

            IsValid = players.Count > 1;

            if (IsValid)
            {
                ExtractAllPlayers(players);
            }
            else
            {
                Log.Debug("Found only " + players.Count + " players.");
            }

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
                double ratio = Convert.ToDouble(player.Groups["Ratio"].Value.Replace(",",string.Empty));
                PlayerSeats.Add(seatNumber, new PlayerData(playerName, ratio));
            }
        }

        static MatchCollection MatchAllPlayerSeats(string handHistory)
        {
            return Regex.Matches(handHistory, SeatPattern, RegexOptions.IgnoreCase);
        }
    }
}
namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class SmallBlindSeatNumberParser : PokerHandParsers.SmallBlindSeatNumberParser
    {
        const string SmallBlindPattern = @"Seat (?<SeatNumber>\d{1,2}): .+ \(small blind\)";

        public override void Parse(string handHistory)
        {
            Match smallBlindSeatNumber = MatchSmallBlindSeatNumber(handHistory);

            IsValid = smallBlindSeatNumber.Success;

            if (IsValid)
            {
               ExtractSmallBlindSeatNumber(smallBlindSeatNumber); 
            }
        }

        static Match MatchSmallBlindSeatNumber(string handHistory)
        {
            return Regex.Match(handHistory, SmallBlindPattern, RegexOptions.IgnoreCase);
        }

        void ExtractSmallBlindSeatNumber(Match smallBlindSeatNumber)
        {
            SmallBlindSeatNumber = Convert.ToInt32(smallBlindSeatNumber.Groups["SeatNumber"].Value);
        }
    }
}
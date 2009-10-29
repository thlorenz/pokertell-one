namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class TotalSeatsParser : PokerHandParsers.TotalSeatsParser
    {

        const string TotalSeatsPattern = 
            TableNameParser.TableNamePattern 
            + @" +(?<TotalSeats>[0-9]{1,2})-max";

        public override void Parse(string handHistory)
        {
            Match totalSeats = MatchTotalSeats(handHistory);
            IsValid = totalSeats.Success;

            if (IsValid)
            {
                ExtractTotalSeats(totalSeats);
            }
        }

        static Match MatchTotalSeats(string handHistory)
        {
            return Regex.Match(handHistory, TotalSeatsPattern, RegexOptions.IgnoreCase);
        }

        void ExtractTotalSeats(Match totalSeats)
        {
            TotalSeats = Convert.ToInt32(totalSeats.Groups["TotalSeats"].Value);
        }
    }
}
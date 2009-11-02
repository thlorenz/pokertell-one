using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class TotalSeatsParser : PokerHandParsers.TotalSeatsParser
    {

        const string TotalSeatsPattern = 
            TableNameParser.TableNamePattern + @"\((?<TotalSeats>[0-9]{1,2}) max\) ";

        const string HeadsUpPattern = TableNameParser.TableNamePattern + @"\(heads up\) ";

        public override PokerHandParsers.TotalSeatsParser Parse(string handHistory)
        {
            IsValid = !string.IsNullOrEmpty(handHistory);

            Match totalSeats = MatchTotalSeats(handHistory);
            if (totalSeats.Success)
            {
                ExtractTotalSeats(totalSeats);
                return this;
            }

            Match headsUp = MatchHeadsUp(handHistory);
            if (headsUp.Success)
            {
                TotalSeats = 2;
                return this;
            }

            DefaultToStandard();
            return this;
        }

        void DefaultToStandard()
        {
            TotalSeats = 9;
        }

        static Match MatchHeadsUp(string handHistory)
        {
            return Regex.Match(handHistory, HeadsUpPattern, RegexOptions.IgnoreCase);
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
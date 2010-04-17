namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    /*
     * Full Tilt is not very consistent in indicating total Seats here some examples:
      
        CashGame
        	Headsup:
        	Full Tilt Poker Game #14861357336: Table Flash (heads up) - $0.25/$0.50 - No Limit Hold'em - 10:41:30 ET - 2009/09/22

        	6-max:
        	Full Tilt Poker Game #15705378958: Table Mascot (6 max) - $8/$16 - Limit Hold'em - 15:15:09 ET - 2009/10/31
        	Full Tilt Poker Game #15702319915: Table Chord (6 max) - $0.02/$0.05 - No Limit Hold'em - 12:26:53 ET - 2009/10/31
        	
        	Full Tilt Poker Game #8455423700: Table Ash (deep 6) - $0.25/$0.50 - No Limit Hold'em - 9:13:55 ET - 2008/10/12

        	Full Tilt Poker Game #15568531231: Table Spur (6 max, deep) - $0.05/$0.10 - No Limit Hold'em - 8:21:39 ET - 2009/10/25


        Tournament
        	Headsup:
        	Full Tilt Poker Game #15311084867: $10 + $0.50 Heads Up Sit & Go (112266702), Table 1 - 20/40 - No Limit Hold'em - 11:33:59 ET - 2009/10/13

        	6-max:
        	Full Tilt Poker Game #20123762015: 250 Play Money Sit & Go (154480242), Table 1 - 15/30 - No Limit Hold'em - 15:55:40 ET - 2010/04/16
        	
        	9-max
        	Full Tilt Poker Game #20098684607: 250 Play Money Sit & Go (154251582), Table 1 - 15/30 - No Limit Hold'em - 13:43:35 ET - 2010/04/15
     *  
     *  the examples marked with a (*) are considered special cases for cash games
     *  
     */

    using System;
    using System.Text.RegularExpressions;

    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class TotalSeatsParser : Base.TotalSeatsParser, IFullTiltPokerTotalSeatsParser
    {
        readonly IFullTiltPokerPlayerSeatsParser _playerSeatsParser;

        const string SeatsMaxPattern = @"\((?<TotalSeats>\d{1,2}) max(, deep){0,1}\) ";

        const string DeepSeatsPattern = @"\(deep (?<TotalSeats>\d{1,2})\) ";

        const string CashGameSeatsPattern = SeatsMaxPattern + "|" + DeepSeatsPattern;
        const string FullTiltCashTableTotalSeatsPattern =
            TableNameParser.FullTiltCashTableNamePattern + CashGameSeatsPattern;

        const string HeadsUpPattern = TableNameParser.FullTiltCashTableNamePattern + @"\(heads up\) ";

        public bool IsTournament { get; set; }

        public int TotalSeatsRecord { get; set; }

        public TotalSeatsParser(IFullTiltPokerPlayerSeatsParser playerSeatsParser)
        {
            _playerSeatsParser = playerSeatsParser;
        }

        protected override string TotalSeatsPattern
        {
            get { return FullTiltCashTableTotalSeatsPattern; }
        }

        public override ITotalSeatsParser Parse(string handHistory)
        {
            IsValid = !string.IsNullOrEmpty(handHistory);
            if (IsValid)
                if (IsTournament)
                    ParseTournament(handHistory);
                else
                    ParseCashGame(handHistory);
            return this;
        }

        static Match MatchHeadsUp(string handHistory)
        {
            return Regex.Match(handHistory, HeadsUpPattern, RegexOptions.IgnoreCase);
        }

        void InCaseNoMatchWasFoundDefaultToNinePlayersSinceFullTiltDoesNotExplicitlyStateNinePlayerTables()
        {
            TotalSeats = 9;
        }

        void ParseCashGame(string handHistory)
        {
            Match totalSeats = MatchTotalSeats(handHistory);
            if (totalSeats.Success)
            {
                ExtractTotalSeats(totalSeats);
                return;
            }

            Match headsUp = MatchHeadsUp(handHistory);
            if (headsUp.Success)
            {
                TotalSeats = 2;
                return;
            }

            InCaseNoMatchWasFoundDefaultToNinePlayersSinceFullTiltDoesNotExplicitlyStateNinePlayerTables();
            return;
        }

        void ParseTournament(string handHistory)
        {
            if (_playerSeatsParser.Parse(handHistory).IsValid)
            {
                TotalSeats = TotalSeatsRecord < _playerSeatsParser.HighestSeatNumber
                    ? TotalSeatsMappedTo2Or6Or9(_playerSeatsParser.HighestSeatNumber)
                    : TotalSeatsRecord;
                IsValid = true;    
            }
            else
            {
                IsValid = TotalSeatsRecord > 0;
                TotalSeats = TotalSeatsRecord;
            }

        }

        static int TotalSeatsMappedTo2Or6Or9(int highestSeatNumber)
        {
            if (highestSeatNumber > 6)
                return 9;
            return highestSeatNumber > 2 ? 6 : 2;
        }
    }
}
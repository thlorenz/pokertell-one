namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    public class StreetsParser : PokerHandParsers.StreetsParser
    {
        #region Constants and Fields

        const string FlopPattern = @"\*\*\* FLOP \*\*\*";

        const string SummaryPattern = @"\*\*\* SUMMARY \*\*\*";

        const string TurnPattern = @"\*\*\* TURN \*\*\*";

        const string RiverPattern = @"\*\*\* RIVER \*\*\*";

        Match _flop;

        string _handHistory;

        Match _river;

        Match _summary;

        Match _turn;

        #endregion

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Public Methods

        public override PokerHandParsers.StreetsParser Parse(string handHistory)
        {
            _handHistory = handHistory;
            _summary = MatchSummary();

            IsValid = _summary.Success;

            if (IsValid)
            {
                try
                {
                    FindStreets();
                    ExtractStreets();
                }
                catch (Exception excep)
                {
                    IsValid = false;
                    Log.Error(excep);
                }
                
            }

            return this;
        }

        #endregion

        #region Methods

        void FindStreets()
        {
            _flop = MatchFlop();
            HasFlop = _flop.Success;
            if (! HasFlop)
            {
                return;
            }

            _turn = MatchTurn();
            HasTurn = _turn.Success;
            if (!HasTurn)
            {
                return;
            }

            _river = MatchRiver();
            HasRiver = _river.Success;
        }

        void ExtractStreets()
        {
            ExtractPreflop();
           
            if (!HasFlop)
            {
                return;
            }

            ExtractFlop();

            if (!HasTurn)
            {
                return;
            }

            ExtractTurn();

            if (!HasRiver)
            {
                return;
            }

            ExtractRiver();
        }

        void ExtractPreflop()
        {
            int length = HasFlop ? _flop.Index : _summary.Index;
            Preflop = _handHistory.Substring(0, length);
        }

        void ExtractFlop()
        {
            int end = HasTurn ? _turn.Index : _summary.Index;
            int length = end - _flop.Index;
            Flop = _handHistory.Substring(_flop.Index, length);
        }

        void ExtractTurn()
        {
            int end = HasRiver ? _river.Index : _summary.Index;
            int length = end - _turn.Index;
            Turn = _handHistory.Substring(_turn.Index, length);
        }

        void ExtractRiver()
        {
            int length = _summary.Index - _river.Index;
            River = _handHistory.Substring(_river.Index, length);
        }

        Match MatchFlop()
        {
            return Regex.Match(_handHistory, FlopPattern, RegexOptions.IgnoreCase);
        }

        Match MatchSummary()
        {
            return Regex.Match(_handHistory, SummaryPattern, RegexOptions.IgnoreCase);
        }

        Match MatchTurn()
        {
            return Regex.Match(_handHistory, TurnPattern, RegexOptions.IgnoreCase);
        }

        Match MatchRiver()
        {
            return Regex.Match(_handHistory, RiverPattern, RegexOptions.IgnoreCase);
        }



        #endregion
    }
}
namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    public abstract class StreetsParser
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Match _flop;

        string _handHistory;

        Match _river;

        Match _summary;

        Match _turn;

        public string Flop { get; protected set; }

        public bool HasFlop { get; protected set; }

        public bool HasRiver { get; protected set; }

        public bool HasTurn { get; protected set; }

        public bool IsValid { get; protected set; }

        public string Preflop { get; protected set; }

        public string River { get; protected set; }

        public string Turn { get; protected set; }

        protected abstract string FlopPattern { get; }

        protected abstract string RiverPattern { get; }

        protected abstract string SummaryPattern { get; }

        protected abstract string TurnPattern { get; }

        public StreetsParser Parse(string handHistory)
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

        public override string ToString()
        {
            return
                string.Format(
                    "Preflop: {0}\n Flop: {1}\n Turn: {2}\n River: {3}\n HasFlop: {4}, HasTurn: {5}, HasRiver: {6}, IsValid: {7}", 
                    Preflop, 
                    Flop, 
                    Turn, 
                    River, 
                    HasFlop, 
                    HasTurn, 
                    HasRiver, 
                    IsValid);
        }

        void ExtractFlop()
        {
            int end = HasTurn ? _turn.Index : _summary.Index;
            int length = end - _flop.Index;
            Flop = _handHistory.Substring(_flop.Index, length);
        }

        void ExtractPreflop()
        {
            int length = HasFlop ? _flop.Index : _summary.Index;
            Preflop = _handHistory.Substring(0, length);
        }

        void ExtractRiver()
        {
            // River will include the remainder of the history. This is important in order to find winning actions contained in the summary portion.
            River = _handHistory.Substring(_river.Index);
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

        void ExtractTurn()
        {
            int end = HasRiver ? _river.Index : _summary.Index;
            int length = end - _turn.Index;
            Turn = _handHistory.Substring(_turn.Index, length);
        }

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
            if (! HasTurn)
            {
                return;
            }

            _river = MatchRiver();
            HasRiver = _river.Success;
        }

        Match MatchFlop()
        {
            return Regex.Match(_handHistory, FlopPattern, RegexOptions.IgnoreCase);
        }

        Match MatchRiver()
        {
            return Regex.Match(_handHistory, RiverPattern, RegexOptions.IgnoreCase);
        }

        Match MatchSummary()
        {
            return Regex.Match(_handHistory, SummaryPattern, RegexOptions.IgnoreCase);
        }

        Match MatchTurn()
        {
            return Regex.Match(_handHistory, TurnPattern, RegexOptions.IgnoreCase);
        }
    }
}
using System;
using System.Reflection;
using System.Text.RegularExpressions;

using log4net;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class StreetsParser
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Match _flop;

        string _handHistory;

        Match _river;

        Match _summary;

        Match _turn;

        #endregion

        #region Properties

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

        #endregion

        #region Public Methods

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
                    "Preflop: {0}, Flop: {1}, Turn: {2}, River: {3}, HasFlop: {4}, HasTurn: {5}, HasRiver: {6}, IsValid: {7}",
                    Preflop,
                    Flop,
                    Turn,
                    River,
                    HasFlop,
                    HasTurn,
                    HasRiver,
                    IsValid);
        }

        #endregion

        #region Methods

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
            int length = _summary.Index - _river.Index;
            River = _handHistory.Substring(_river.Index, length);
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
            if (!HasTurn)
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

        #endregion
    }
}
namespace PokerTell.Statistics.Tests.Fakes
{
    using System;

    using PokerTell.Statistics.Filters;

    public class AnalyzablePokerPlayersFilterTester : AnalyzablePokerPlayersFilter
    {
        #region Constructors and Destructors

        public AnalyzablePokerPlayersFilterTester()
        {
            CurrentTimeUsed = DateTime.MinValue;
        }

        #endregion

        #region Properties

        public DateTime CurrentTimeUsed { get; set; }

        protected override DateTime CurrentTime
        {
            get { return CurrentTimeUsed; }
        }

        #endregion
    }
}
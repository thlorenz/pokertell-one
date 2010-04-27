namespace PokerTell.Repository.Tests.Database
{
    using System;
    using System.Collections.Generic;

    using Base;

    // Resharper disable InconsistentNaming

    public class SimpleHandHistoryRetrieverSut : SimpleHandHistoryRetrieverBase
    {
        public int MinForWhichHandHistoriesWereQueried;

        public int MaxForWhichHandHistoriesWereQueried;

        public int NumberOfHandHistories
        {
            get { return _numberOfHandHistories; }
            set { _numberOfHandHistories = value; }
        }

        public int LastRowRetrieved
        {
            get { return _lastRowRetrieved; }
            set { _lastRowRetrieved = value; }
        }

        public override string NumberOfHandHistoriesQuery
        {
            get { return "some query"; }
        }

        protected override string RetrieveNextBatchOfHandHistoriesQuery
        {
            get { return "some query"; }
        }

        protected override IEnumerable<string> QueryHandHistories(int min, int max)
        {
            MinForWhichHandHistoriesWereQueried = min;
            MaxForWhichHandHistoriesWereQueried = max;
            return new List<string>();
        }

        public SimpleHandHistoryRetrieverSut Set_IsDone(bool isDone)
        {
            IsDone = isDone;
            return this;
        }
    }
}
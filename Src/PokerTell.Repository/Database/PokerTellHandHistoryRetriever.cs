namespace PokerTell.Repository.Database
{
    using Base;

    using Interfaces;

    public class PokerTellHandHistoryRetriever : SimpleHandHistoryRetrieverBase, IPokerTellHandHistoryRetriever
    {
        const string PokerTellNumberOfHandHistoriesQuery = "Select Max(Id) From convertedpokerhands;";

        const string PokerTellRetrieveNextBatchOfHandHistoriesQuery = "Select handhistory From convertedpokerhands Where id Between {0} And {1};";

        public override string NumberOfHandHistoriesQuery
        {
            get { return PokerTellNumberOfHandHistoriesQuery; }
        }

        protected override string RetrieveNextBatchOfHandHistoriesQuery
        {
            get { return PokerTellRetrieveNextBatchOfHandHistoriesQuery; }
        }
    }

}
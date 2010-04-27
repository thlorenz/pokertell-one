namespace PokerTell.Repository.Database
{
    using Base;

    using Interfaces;

    public class PokerTrackerHandHistoryRetriever : SimpleHandHistoryRetrieverBase, IPokerTrackerHandHistoryRetriever
    {
        const string PokerTrackerNumberOfHandHistoriesQuery = "Select Count(*) From holdem_hand_histories;";

        const string PokerTrackerRetrieveNextBatchOfHandHistoriesQuery = 
            "Select history From holdem_hand_histories Where id_hand Between {0} And {1};";

        public override string NumberOfHandHistoriesQuery
        {
            get { return PokerTrackerNumberOfHandHistoriesQuery; }
        }

        protected override string RetrieveNextBatchOfHandHistoriesQuery
        {
            get { return PokerTrackerRetrieveNextBatchOfHandHistoriesQuery; }
        }
    }
}
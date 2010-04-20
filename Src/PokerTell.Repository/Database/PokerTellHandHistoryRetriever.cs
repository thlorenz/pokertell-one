namespace PokerTell.Repository.Database
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    public class PokerTellHandHistoryRetriever : IPokerTellHandHistoryRetriever
    {
        public const string NumberOfHandHistoriesQuery = "Select Max(Id) From convertedpokerhands;";

        IDataProvider _dataProvider;

        protected int _numberOfHandHistories;

        protected int _lastRowRetrieved;

        public bool IsDone { get; protected set; }

        public int HandHistoriesCount
        {
            get 
            {
                return _numberOfHandHistories;
            }
        }

        public IEnumerable<string> GetNext(int batchSize)
        {
            var min = _lastRowRetrieved + 1;
            var max = _lastRowRetrieved + 1 + batchSize;

            var nextHandHistories = QueryHandHistories(min, max);
          
            _lastRowRetrieved = max;

            IsDone = _lastRowRetrieved + 1 >= _numberOfHandHistories;
            
            return nextHandHistories;
        }

        protected virtual IEnumerable<string> QueryHandHistories(int min, int max)
        {
            var query = string.Format("Select handhistory From convertedpokerhands Where id Between {0} And {1};", min, max);
    
            return _dataProvider.ExecuteQueryGetColumn<string>(query, 0);
        }

        public IHandHistoryRetriever Using(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            IsDone = false;
            _lastRowRetrieved = -1;

            _numberOfHandHistories = (int)_dataProvider.ExecuteScalar(NumberOfHandHistoriesQuery);

            return this;
        }
    }
}
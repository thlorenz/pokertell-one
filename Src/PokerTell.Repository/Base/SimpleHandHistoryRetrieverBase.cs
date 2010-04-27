namespace PokerTell.Repository.Base
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    public abstract class SimpleHandHistoryRetrieverBase : IHandHistoryRetriever
    {
        IDataProvider _dataProvider;

        protected int _numberOfHandHistories;

        protected int _lastRowRetrieved;

        public abstract string NumberOfHandHistoriesQuery { get; }

        protected abstract string RetrieveNextBatchOfHandHistoriesQuery { get; }

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
            var query = string.Format(RetrieveNextBatchOfHandHistoriesQuery, min, max);
    
            return _dataProvider.ExecuteQueryGetColumn<string>(query, 0);
        }

        public IHandHistoryRetriever Using(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            IsDone = false;
            _lastRowRetrieved = -1;

            if (!int.TryParse(_dataProvider.ExecuteScalar(NumberOfHandHistoriesQuery).ToString(), out _numberOfHandHistories))
                _numberOfHandHistories = 0;

            return this;
        }
    }
}
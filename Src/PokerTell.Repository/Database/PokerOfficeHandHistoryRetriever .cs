namespace PokerTell.Repository.Database
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    public class PokerOfficeHandHistoryRetriever : IPokerOfficeHandHistoryRetriever
    {
        public const string NumberOfCashGameHistoriesQuery = "Select Count(*) From handhistory;";

        public const string NumberOfTournamentHistoriesQuery = "Select Count(*) From thandhistory;";

        IDataProvider _dataProvider;

        protected int _numberOfCashGameHandHistories;

        protected int _numberOfTournamentHandHistories;

        protected bool _doneWithCashGameHandHistories;

        protected bool _doneWithTournamentHandHistories;

        protected int _lastCashGameTableRowRetrieved;

        protected int _lastTournamentTableRowRetrieved;

        public bool IsDone 
        {
            get { return _doneWithCashGameHandHistories && _doneWithTournamentHandHistories; }
        }

        public int HandHistoriesCount
        {
            get 
            {
                return _numberOfCashGameHandHistories + _numberOfTournamentHandHistories;
            }
        }

        public IEnumerable<string> GetNext(int batchSize)
        {
            return !_doneWithCashGameHandHistories
            ? GetNextCashGameHandHistories(batchSize)
            : GetNextTournamentHandHistories(batchSize);
        }

        IEnumerable<string> GetNextCashGameHandHistories(int batchSize)
        {
            var min = _lastCashGameTableRowRetrieved + 1;
            var max = _lastCashGameTableRowRetrieved + 1 + batchSize;

            var nextHandHistories = QueryHandHistories(min, max);
          
            _lastCashGameTableRowRetrieved = max;

            _doneWithCashGameHandHistories = _lastCashGameTableRowRetrieved + 1 >= _numberOfCashGameHandHistories;
            
            return nextHandHistories;
        }

        IEnumerable<string> GetNextTournamentHandHistories(int batchSize)
        {
            var min = _lastTournamentTableRowRetrieved + 1;
            var max = _lastTournamentTableRowRetrieved + 1 + batchSize;

            var nextHandHistories = QueryHandHistories(min, max);
          
            _lastTournamentTableRowRetrieved = max;

            _doneWithTournamentHandHistories = _lastTournamentTableRowRetrieved + 1 >= _numberOfTournamentHandHistories;
            
            return nextHandHistories;
        }

        protected virtual IEnumerable<string> QueryHandHistories(int min, int max)
        {
            string tableName = !_doneWithCashGameHandHistories ? "handhistory" : "thandhistory";

            var query = string.Format("Select handhistory From {0} Limit {1}, {2};", tableName, min, max);
    
            return _dataProvider.ExecuteQueryGetColumn<string>(query, 0);
        }

        public IHandHistoryRetriever Using(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            _doneWithCashGameHandHistories = false;
            _doneWithTournamentHandHistories = false;
            _lastCashGameTableRowRetrieved = -1;
            _lastTournamentTableRowRetrieved = -1;

            _numberOfCashGameHandHistories = (int)_dataProvider.ExecuteScalar(NumberOfCashGameHistoriesQuery);
            _numberOfTournamentHandHistories = (int) _dataProvider.ExecuteScalar(NumberOfTournamentHistoriesQuery);

            return this;
        }
    }
}
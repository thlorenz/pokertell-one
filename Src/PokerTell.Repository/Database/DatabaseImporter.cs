namespace PokerTell.Repository.Database
{
    using System.Linq;

    using Infrastructure.Events;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;

    using Tools.Interfaces;

    public class DatabaseImporter : IDatabaseImporter
    {
        public const int BatchSize = 50;

        readonly IBackgroundWorker _backgroundWorker;

        readonly IEventAggregator _eventAggregator;

        readonly IPokerOfficeHandHistoryRetriever _pokerOfficeHandHistoryRetriever;

        readonly IPokerTellHandHistoryRetriever _pokerTellHandHistoryRetriever;

        readonly IPokerTrackerHandHistoryRetriever _pokerTrackerHandHistoryRetriever;

        readonly IRepository _repository;

        protected int _numberOfHandsToImport;


        public DatabaseImporter(
            IEventAggregator eventAggregator, 
            IBackgroundWorker backgroundWorker, 
            IRepository repository, 
            IPokerTellHandHistoryRetriever pokerTellHandHistoryRetriever, 
            IPokerOfficeHandHistoryRetriever pokerOfficeHandHistoryRetriever, 
            IPokerTrackerHandHistoryRetriever pokerTrackerHandHistoryRetriever)
        {
            _eventAggregator = eventAggregator;
            _backgroundWorker = backgroundWorker;
            _repository = repository;
            _pokerTellHandHistoryRetriever = pokerTellHandHistoryRetriever;
            _pokerOfficeHandHistoryRetriever = pokerOfficeHandHistoryRetriever;
            _pokerTrackerHandHistoryRetriever = pokerTrackerHandHistoryRetriever;
        }

        public bool IsBusy { get; protected set; }

        public IDatabaseImporter ImportFrom(PokerStatisticsApplications pokerStatisticsApplication, string databaseName, IDataProvider dataProvider)
        {
            IsBusy = true;
            dataProvider.ExecuteNonQuery(string.Format("Use `{0}`;", databaseName));

            switch (pokerStatisticsApplication)
            {
                case PokerStatisticsApplications.PokerTell:
                    ImportHandHistoriesUsing(_pokerTellHandHistoryRetriever.Using(dataProvider));
                    break;
                case PokerStatisticsApplications.PokerOffice:
                    ImportHandHistoriesUsing(_pokerOfficeHandHistoryRetriever.Using(dataProvider));
                    break;
                case PokerStatisticsApplications.PokerTracker:
                    ImportHandHistoriesUsing(_pokerTrackerHandHistoryRetriever.Using(dataProvider));
                    break;
            }

            return this;
        }

        protected virtual void ImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            _numberOfHandsToImport = handHistoryRetriever.HandHistoriesCount;
            
            ReportProgress(0);

            var nextBatchOfHandHistories =
                handHistoryRetriever
                    .GetNext(BatchSize)
                    .Aggregate(string.Empty, (collectedSoFar, currentHandHistory) => collectedSoFar + "\n\n" + currentHandHistory);
          
            var convertedHands =
                _repository
                    .RetrieveHandsFromString(nextBatchOfHandHistories);
            _repository.InsertHands(convertedHands);
        }

        void ReportProgress(int percentage)
        {
            var progressUpdate = new ProgressUpdateEventArgs(ProgressTypes.DatabaseImport, percentage);
            _eventAggregator
                .GetEvent<ProgressUpdateEvent>()
                .Publish(progressUpdate);
        }
    }
}
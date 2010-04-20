namespace PokerTell.Repository.Database
{
    using System.Linq;
    using System.Threading;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;
    using PokerTell.Repository.Properties;

    using Tools.Interfaces;

    public class DatabaseImporter : IDatabaseImporter
    {
        protected string _databaseName;

        protected int _numberOfHandsAttemptedToImport;

        protected int _numberOfHandsSuccessfullyImported;

        protected int _numberOfHandsToImport;

        const int DefaultBatchSize = 50;

        readonly IBackgroundWorker _backgroundWorker;

        readonly IEventAggregator _eventAggregator;

        readonly IPokerOfficeHandHistoryRetriever _pokerOfficeHandHistoryRetriever;

        readonly IPokerTellHandHistoryRetriever _pokerTellHandHistoryRetriever;

        readonly IPokerTrackerHandHistoryRetriever _pokerTrackerHandHistoryRetriever;

        readonly IRepository _repository;

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

            BatchSize = DefaultBatchSize;
        }

        public int BatchSize { get; set; }

        public bool IsBusy { get; protected set; }

        /// <summary>
        /// Assumes that the dataprovider has been connected to the given database.
        /// </summary>
        /// <param name="pokerStatisticsApplication"></param>
        /// <param name="databaseName"></param>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        public IDatabaseImporter ImportFrom(PokerStatisticsApplications pokerStatisticsApplication, string databaseName, IDataProvider dataProvider)
        {
            _databaseName = databaseName;

            IsBusy = true;

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

        protected void FinishedImportingHandHistories()
        {
            PublishUserMessageAboutFinishedImport();

            ReportProgress(100);
            IsBusy = false;
        }

        protected virtual void ImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            PrepareToImportHandHistoriesUsing(handHistoryRetriever);

            _backgroundWorker.DoWork += (s, e) => {
                if (_numberOfHandsToImport > 0)
                {
                    while (!handHistoryRetriever.IsDone)
                    {
                        ImportNextBatchOfHandHistoriesUsing(handHistoryRetriever);
                    }
                }
            };
            _backgroundWorker.RunWorkerCompleted += (s, e) => FinishedImportingHandHistories();

            _backgroundWorker.RunWorkerAsync();
        }

        protected void ImportNextBatchOfHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            var nextBatchOfHandHistories =
                handHistoryRetriever
                    .GetNext(BatchSize)
                    .Aggregate(string.Empty, (collectedSoFar, currentHandHistory) => collectedSoFar + "\n\n" + currentHandHistory);

            _numberOfHandsAttemptedToImport += BatchSize;

            var convertedHands =
                _repository
                    .RetrieveHandsFromString(nextBatchOfHandHistories);
            _repository.InsertHands(convertedHands);

            _numberOfHandsSuccessfullyImported += convertedHands.Count();

            ReportProgress(_numberOfHandsAttemptedToImport * 100 / _numberOfHandsToImport);
        }

        protected void PrepareToImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            _numberOfHandsToImport = handHistoryRetriever.HandHistoriesCount;
            ReportProgress(0);
            IsBusy = true;
        }

        void PublishUserMessageAboutFinishedImport()
        {
            var msg = string.Format(Resources.Info_DatabaseImportCompleted, _numberOfHandsSuccessfullyImported, _databaseName);
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Publish(new UserMessageEventArgs(UserMessageTypes.Info, msg));
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
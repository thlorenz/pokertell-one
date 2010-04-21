namespace PokerTell.Repository.Tests.Database
{
    using System.Collections.Generic;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using Moq;

    using PokerTell.Repository.Database;

    using Tools.Interfaces;

    using UnitTests.Fakes;

    using It=Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class DatabaseImporterSpecs
    {
        static IEventAggregator _eventAggregator;

        static Mock<IPokerTrackerHandHistoryRetriever> _pokerTrackerHandHistoryRetriever_Mock;

        static Mock<IPokerOfficeHandHistoryRetriever> _pokerOfficeHandHistoryRetriever_Mock;

        static Mock<IPokerTellHandHistoryRetriever> _pokerTellHandHistoryRetriever_Mock;

        static Mock<IRepository> _repository_Mock;

        static Mock<IDataProvider> _dataProvider_Mock;

        static BackgroundWorkerMock _backgroundWorker_Mock;

        static DatabaseImporterSut _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _backgroundWorker_Mock = new BackgroundWorkerMock();
            _repository_Mock = new Mock<IRepository>();

            _pokerTrackerHandHistoryRetriever_Mock = new Mock<IPokerTrackerHandHistoryRetriever>();
            _pokerTrackerHandHistoryRetriever_Mock
                .Setup(hr => hr.Using(Moq.It.IsAny<IDataProvider>()))
                .Returns(_pokerTrackerHandHistoryRetriever_Mock.Object);

            _pokerOfficeHandHistoryRetriever_Mock = new Mock<IPokerOfficeHandHistoryRetriever>();
            _pokerOfficeHandHistoryRetriever_Mock
                .Setup(hr => hr.Using(Moq.It.IsAny<IDataProvider>()))
                .Returns(_pokerOfficeHandHistoryRetriever_Mock.Object);

            _pokerTellHandHistoryRetriever_Mock = new Mock<IPokerTellHandHistoryRetriever>();
            _pokerTellHandHistoryRetriever_Mock
                .Setup(hr => hr.Using(Moq.It.IsAny<IDataProvider>()))
                .Returns(_pokerTellHandHistoryRetriever_Mock.Object);

            _sut = new DatabaseImporterSut(_eventAggregator,
                                        _backgroundWorker_Mock, 
                                        _repository_Mock.Object,
                                        _pokerTellHandHistoryRetriever_Mock.Object,
                                        _pokerOfficeHandHistoryRetriever_Mock.Object,
                                        _pokerTrackerHandHistoryRetriever_Mock.Object);

            _dataProvider_Mock = new Mock<IDataProvider>();
        };

        [Subject(typeof(DatabaseImporter), "ImportFrom")]
        public class when_told_to_import_from_a_PokerOffice_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerOffice;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_set_the_database_name_to_the_given_database = () => _sut.DatabaseName.ShouldEqual(databaseName);

            It should_tell_the_PokerOffice_retriever_to_use_the_DataProvider 
                = () => _pokerOfficeHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerOffice_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImported.ShouldEqual(_pokerOfficeHandHistoryRetriever_Mock.Object);
        }

        [Subject(typeof(DatabaseImporter), "ImportFrom")]
        public class when_told_to_import_from_a_PokerTracker_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerTracker;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_set_the_database_name_to_the_given_database = () => _sut.DatabaseName.ShouldEqual(databaseName);

            It should_tell_the_PokerTracker_retriever_to_use_the_DataProvider 
                = () => _pokerTrackerHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerTracker_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImported.ShouldEqual(_pokerTrackerHandHistoryRetriever_Mock.Object);
        }

        [Subject(typeof(DatabaseImporter), "ImportFrom")]
        public class when_told_to_import_from_a_PokerTell_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerTell;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_set_the_database_name_to_the_given_database = () => _sut.DatabaseName.ShouldEqual(databaseName);

            It should_tell_the_PokerTell_retriever_to_use_the_DataProvider 
                = () => _pokerTellHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerTell_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImported.ShouldEqual(_pokerTellHandHistoryRetriever_Mock.Object);
        }

        [Subject(typeof(DatabaseImporter), "PrepareToImportHandHistories")]
        public class when_told_to_prepare_to_import_handhistories_using_a_given_hand_history_retriever : DatabaseImporterSpecs
        {
            const int handHistoriesCount = 1;
            static int percentageForWhichProgressWasReported;

            static Mock<IHandHistoryRetriever> handHistoryRetriever_Mock;

            Establish context = () => {
                handHistoryRetriever_Mock = new Mock<IHandHistoryRetriever>();
                handHistoryRetriever_Mock
                    .SetupGet(hr => hr.HandHistoriesCount)
                    .Returns(handHistoriesCount);
                _eventAggregator
                    .GetEvent<ProgressUpdateEvent>()
                    .Subscribe(args => percentageForWhichProgressWasReported = args.PercentCompleted);
            };

            Because of = () => _sut.Invoke_PrepareToImportHandHistoriesUsing(handHistoryRetriever_Mock.Object);

            It should_become_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_set_the_number_of_hands_to_import_to_the_hand_histories_count_of_the_retrieve
                = () => _sut.NumberOfHandsToImport.ShouldEqual(handHistoriesCount);

            It should_report_zero_progress_to_show_the_progress_bar = () => percentageForWhichProgressWasReported.ShouldEqual(0);
        }

        [Subject(typeof(DatabaseImporter), "ImportNextBatchOfHandHistories")]
        public class when_told_to_import_next_batch_of_total_of_10_hand_histories_from_a_given_hand_history_retriever_which_returns_2_assuming_that_both_are_successfully_converted_and_2_were_attempted_to_import_previously_and_batchsize_is_3
            : DatabaseImporterSpecs
        {
            const string firstRetrievedHandhistory = "firstHandHistory";
            const string secondRetrievedHandhistory = "secondHandHistory";

            static int percentageForWhichProgressWasReported;

            const int batchSize = 3;

            const int numberOfHandsAttemptedToImportSoFar = 2;

            const int numberOfHandsSuccessfullyImportedSoFar = 1;

            const int handHistoriesCount = 10;

            static Mock<IHandHistoryRetriever> handHistoryRetriever_Mock;

            static IEnumerable<string> retrievedHandHistories;

            static IEnumerable<IConvertedPokerHand> retrievedConvertedHands;

            static IList<int> percentagesForWhichProgressWasReported;

            Establish context = () => {
                _eventAggregator
                    .GetEvent<ProgressUpdateEvent>()
                    .Subscribe(args => percentageForWhichProgressWasReported = args.PercentCompleted);

                retrievedHandHistories = new[] { firstRetrievedHandhistory, secondRetrievedHandhistory };
                    
                handHistoryRetriever_Mock = new Mock<IHandHistoryRetriever>();
                handHistoryRetriever_Mock
                    .Setup(hr => hr.GetNext(batchSize))
                    .Returns(retrievedHandHistories);
                handHistoryRetriever_Mock
                    .SetupGet(hr => hr.HandHistoriesCount)
                    .Returns(handHistoriesCount);

                retrievedConvertedHands = new[] { new Mock<IConvertedPokerHand>().Object, new Mock<IConvertedPokerHand>().Object };
                _repository_Mock
                    .Setup(r => r.RetrieveHandsFromString(Moq.It.IsAny<string>()))
                    .Returns(retrievedConvertedHands);

                percentagesForWhichProgressWasReported = new List<int>();
                _eventAggregator
                    .GetEvent<ProgressUpdateEvent>()
                    .Subscribe(args => percentagesForWhichProgressWasReported.Add(args.PercentCompleted));

                _sut.BatchSize = batchSize;
                _sut.NumberOfHandsToImport = handHistoriesCount;
                _sut.NumberOfHandsAttemptedToImport = numberOfHandsAttemptedToImportSoFar;
                _sut.NumberOfHandsSuccessfullyImported = numberOfHandsSuccessfullyImportedSoFar;
            };

            Because of = () => _sut.Invoke_ImportNextBatchOfHandHistoriesUsing(handHistoryRetriever_Mock.Object);

            It should_tell_the_retriever_to_get_the_next_batch_of_hand_histories
                = () => handHistoryRetriever_Mock.Verify(hr => hr.GetNext(batchSize));

            It should_tell_the_Repository_to_parse_the_combined_hand_histories_returned_by_the_retriever 
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromString(Moq.It.Is<string>(
                    str => str.Contains(firstRetrievedHandhistory) && str.Contains(secondRetrievedHandhistory))));

            It should_tell_the_Repository_to_insert_the_converted_hands_it_returned_when_it_parsed_the_hand_histories
                = () => _repository_Mock.Verify(r => r.InsertHands(retrievedConvertedHands));

            It should_add_the_batchSize_to_the_NumberOfHandsAttemptedToImport 
                = () => _sut.NumberOfHandsAttemptedToImport.ShouldEqual(numberOfHandsAttemptedToImportSoFar + batchSize);

            It should_add_2_to_the_NumberOfHandsSuccessfullyImported 
                = () => _sut.NumberOfHandsSuccessfullyImported.ShouldEqual(numberOfHandsSuccessfullyImportedSoFar + 2);

            It should_report_progress_of_50_percent_to_hide_progressbar = () => percentageForWhichProgressWasReported.ShouldEqual(50);
        }

        [Subject(typeof(DatabaseImporter), "FinishedImporting")]
        public class when_it_finishes_importing_hand_histories : DatabaseImporterSpecs
        {
            static int percentageForWhichProgressWasReported;

            static string userMessageAboutImportedHands;

            const int numberOfHandsSuccessfullyImported = 1;

            const string databaseName = "some database";

            Establish context = () => {
                _eventAggregator
                    .GetEvent<ProgressUpdateEvent>()
                    .Subscribe(args => percentageForWhichProgressWasReported = args.PercentCompleted);
                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => userMessageAboutImportedHands = args.UserMessage,
                               ThreadOption.PublisherThread,
                               false,
                               args => args.MessageType == UserMessageTypes.Info);
                _sut
                    .Set_IsBusy(true)
                    .NumberOfHandsSuccessfullyImported = numberOfHandsSuccessfullyImported;
                _sut.DatabaseName = databaseName;
            };

            Because of = () => _sut.Invoke_FinishedImportingHandHistories();

            It should_become_not_busy = () => _sut.IsBusy.ShouldBeFalse();

            It should_report_progress_of_100_percent_to_hide_progressbar = () => percentageForWhichProgressWasReported.ShouldEqual(100);

            It should_let_the_user_know_how_many_hands_it_successfully_imported 
                = () => userMessageAboutImportedHands.ShouldContain(numberOfHandsSuccessfullyImported + " hands");

            It should_let_the_user_know_what_database_it_imported_the_hands_from
                = () => userMessageAboutImportedHands.ShouldContain(databaseName);
        }

        [Subject(typeof(DatabaseImporter), "IsBusy changed")]
        public class when_the_busy_status_changed : DatabaseImporterSpecs
        {
            static bool isBusyChangedWasRaised;

            static bool isBusyChangedWasRaisedWith;
            const bool newBusyState = true;


            Establish context = () => _sut.IsBusyChanged += arg => {
                isBusyChangedWasRaised = true;
                isBusyChangedWasRaisedWith = arg;
            };

            Because of = () => _sut.Set_IsBusy(newBusyState);

            It should_let_me_know = () => isBusyChangedWasRaised.ShouldBeTrue();

            It should_pass_the_new_busy_state = () => isBusyChangedWasRaisedWith.ShouldEqual(newBusyState);
        }
    }

    public class DatabaseImporterSut : DatabaseImporter
    {
        public IHandHistoryRetriever RetrieverWithWhichHandHistoriesWereImported;

        public int NumberOfHandsToImport
        {
            get { return _numberOfHandsToImport; }
            set { _numberOfHandsToImport = value; }
        }

        public int NumberOfHandsSuccessfullyImported
        {
            get { return _numberOfHandsSuccessfullyImported; }
            set { _numberOfHandsSuccessfullyImported = value; }
        }
        
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public int NumberOfHandsAttemptedToImport
        {
            get { return _numberOfHandsAttemptedToImport; }
            set { _numberOfHandsAttemptedToImport = value; }
        }

        public DatabaseImporterSut Set_IsBusy(bool isBusy)
        {
            IsBusy = true;
            return this;
        }

        public DatabaseImporterSut(IEventAggregator eventAggregator, IBackgroundWorker backgroundWorker, IRepository repository, IPokerTellHandHistoryRetriever pokerTellHandHistoryRetriever, IPokerOfficeHandHistoryRetriever pokerOfficeHandHistoryRetriever, IPokerTrackerHandHistoryRetriever pokerTrackerHandHistoryRetriever)
            : base(eventAggregator, backgroundWorker, repository, pokerTellHandHistoryRetriever, pokerOfficeHandHistoryRetriever, pokerTrackerHandHistoryRetriever)
        {
        }

        public void Invoke_ImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            ImportHandHistoriesUsing(handHistoryRetriever);
        }

        public void Invoke_PrepareToImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            PrepareToImportHandHistoriesUsing(handHistoryRetriever);
        }

        public void Invoke_ImportNextBatchOfHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            ImportNextBatchOfHandHistoriesUsing(handHistoryRetriever);
        }

        public void Invoke_FinishedImportingHandHistories()
        {
           FinishedImportingHandHistories(); 
        }

        protected override void ImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            // Don't call base here since it will end up in a while loop which will never end since our HandHistory retriever mock will never be done.
            RetrieverWithWhichHandHistoriesWereImported = handHistoryRetriever;
        }
    }
}
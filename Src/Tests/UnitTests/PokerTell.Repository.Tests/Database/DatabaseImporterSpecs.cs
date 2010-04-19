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

        [Subject(typeof(DatabaseImporter), "InmportFrom")]
        public class when_told_to_import_from_a_PokerOffice_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerOffice;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_tell_the_DataProvider_to_use_the_given_database
                = () => _dataProvider_Mock.Verify(dp => dp.ExecuteNonQuery("Use `" + databaseName + "`;"));

            It should_tell_the_PokerOffice_retriever_to_use_the_DataProvider 
                = () => _pokerOfficeHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerOffice_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImportedWith.ShouldEqual(_pokerOfficeHandHistoryRetriever_Mock.Object);
        }

        [Subject(typeof(DatabaseImporter), "InmportFrom")]
        public class when_told_to_import_from_a_PokerTracker_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerTracker;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_tell_the_DataProvider_to_use_the_given_database
                = () => _dataProvider_Mock.Verify(dp => dp.ExecuteNonQuery("Use `" + databaseName + "`;"));

            It should_tell_the_PokerTracker_retriever_to_use_the_DataProvider 
                = () => _pokerTrackerHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerTracker_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImportedWith.ShouldEqual(_pokerTrackerHandHistoryRetriever_Mock.Object);
        }

        [Subject(typeof(DatabaseImporter), "InmportFrom")]
        public class when_told_to_import_from_a_PokerTell_database_given_a_connected_dataprovider : DatabaseImporterSpecs
        {
            const string databaseName = "someName";

            const PokerStatisticsApplications application = PokerStatisticsApplications.PokerTell;

            Because of = () => _sut.ImportFrom(application, databaseName, _dataProvider_Mock.Object);

            It should_be_busy = () => _sut.IsBusy.ShouldBeTrue();

            It should_tell_the_DataProvider_to_use_the_given_database
                = () => _dataProvider_Mock.Verify(dp => dp.ExecuteNonQuery("Use `" + databaseName + "`;"));

            It should_tell_the_PokerTell_retriever_to_use_the_DataProvider 
                = () => _pokerTellHandHistoryRetriever_Mock.Verify(hr => hr.Using(_dataProvider_Mock.Object));

            It should_import_handhistories_using_the_PokerTell_retriever 
                = () => _sut.RetrieverWithWhichHandHistoriesWereImportedWith.ShouldEqual(_pokerTellHandHistoryRetriever_Mock.Object);
        }
        [Subject(typeof(DatabaseImporter), "ImportHandHistoriesUsing")]
        public class when_told_to_import_a_hand_histories_from_a_given_hand_history_retriever : DatabaseImporterSpecs
        {
            const string firstRetrievedHandhistory = "firstHandHistory";
            const string secondRetrievedHandhistory = "secondHandHistory";

            const int handHistoriesCount = 1000;

            static Mock<IHandHistoryRetriever> handHistoryRetriever_Mock;

            static IEnumerable<string> retrievedHandHistories;

            static IEnumerable<IConvertedPokerHand> retrievedConvertedHands;

            static IList<int> percentagesForWhichProgressWasReported;

            Establish context = () => {
                retrievedHandHistories = new[] { firstRetrievedHandhistory, secondRetrievedHandhistory };
                    
                handHistoryRetriever_Mock = new Mock<IHandHistoryRetriever>();
                handHistoryRetriever_Mock
                    .Setup(hr => hr.GetNext(DatabaseImporter.BatchSize))
                    .Returns(retrievedHandHistories);
                handHistoryRetriever_Mock
                    .SetupGet(hr => hr.HandHistoriesCount)
                    .Returns(handHistoriesCount);

                retrievedConvertedHands = new[] { new Mock<IConvertedPokerHand>().Object };
                _repository_Mock
                    .Setup(r => r.RetrieveHandsFromString(Moq.It.IsAny<string>()))
                    .Returns(retrievedConvertedHands);

                percentagesForWhichProgressWasReported = new List<int>();
                _eventAggregator
                    .GetEvent<ProgressUpdateEvent>()
                    .Subscribe(args => percentagesForWhichProgressWasReported.Add(args.PercentCompleted));
            };

            Because of = () => _sut.Invoke_ImportDatabaseUsing(handHistoryRetriever_Mock.Object);

            It should_set_the_number_of_hands_to_import_to_the_hand_histories_count_of_the_retrieve
                = () => _sut.NumberOfHandsToImport.ShouldEqual(handHistoriesCount);

            It should_report_zero_progress_to_show_the_progress_bar = () => percentagesForWhichProgressWasReported.ShouldContain(0);

            It should_tell_the_retriever_to_get_the_next_batch_of_hand_histories
                = () => handHistoryRetriever_Mock.Verify(hr => hr.GetNext(DatabaseImporter.BatchSize));

            It should_tell_the_Repository_to_parse_the_combined_hand_histories_returned_by_the_retriever 
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromString(Moq.It.Is<string>(
                    str => str.Contains(firstRetrievedHandhistory) && str.Contains(secondRetrievedHandhistory))));

            It should_tell_the_Repository_to_insert_the_converted_hands_it_returned_when_it_parsed_the_hand_histories
                = () => _repository_Mock.Verify(r => r.InsertHands(retrievedConvertedHands));
        }
    }

    public class DatabaseImporterSut : DatabaseImporter
    {
        public IHandHistoryRetriever RetrieverWithWhichHandHistoriesWereImportedWith;

        public int NumberOfHandsToImport
        {
            get { return _numberOfHandsToImport; }
        }

        public DatabaseImporterSut(IEventAggregator eventAggregator, IBackgroundWorker backgroundWorker, IRepository repository, IPokerTellHandHistoryRetriever pokerTellHandHistoryRetriever, IPokerOfficeHandHistoryRetriever pokerOfficeHandHistoryRetriever, IPokerTrackerHandHistoryRetriever pokerTrackerHandHistoryRetriever)
            : base(eventAggregator, backgroundWorker, repository, pokerTellHandHistoryRetriever, pokerOfficeHandHistoryRetriever, pokerTrackerHandHistoryRetriever)
        {
        }

        public void Invoke_ImportDatabaseUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            ImportHandHistoriesUsing(handHistoryRetriever);
        }

        protected override void ImportHandHistoriesUsing(IHandHistoryRetriever handHistoryRetriever)
        {
            base.ImportHandHistoriesUsing(handHistoryRetriever);
            RetrieverWithWhichHandHistoriesWereImportedWith = handHistoryRetriever;
        }


    }
}
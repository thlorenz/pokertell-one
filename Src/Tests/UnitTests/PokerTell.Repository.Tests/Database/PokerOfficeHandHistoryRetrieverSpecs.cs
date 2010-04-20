namespace PokerTell.Repository.Tests.Database
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Repository.Database;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PokerOfficeHandHistoryRetrieverSpecs
    {
        static Mock<IDataProvider> _dataProvider_Mock;

        static PokerOfficeHandHistoryRetrieverSut _sut;

        Establish specContext = () => {
            _sut = new PokerOfficeHandHistoryRetrieverSut();

            _dataProvider_Mock = new Mock<IDataProvider>();
        };

        [Subject(typeof(PokerOfficeHandHistoryRetriever), "Using")]
        public class when_told_to_use_a_dataprovider : PokerOfficeHandHistoryRetrieverSpecs
        {
            const int numberOfCashGameHistories = 1;
            const int numberOfTournamentHistories = 2;

            Establish context = () => {
                _sut.DoneWithCashGameHandHistories = true;
                _sut.DoneWithTournamentHandHistories = true;

                _dataProvider_Mock
                    .Setup(dp => dp.ExecuteScalar(PokerOfficeHandHistoryRetriever.NumberOfCashGameHistoriesQuery))
                    .Returns(numberOfCashGameHistories);
                _dataProvider_Mock
                    .Setup(dp => dp.ExecuteScalar(PokerOfficeHandHistoryRetriever.NumberOfTournamentHistoriesQuery))
                    .Returns(numberOfTournamentHistories);
            };

            Because of = () => _sut.Using(_dataProvider_Mock.Object);

            It should_not_be_done_with_CashGameHistories = () => _sut.DoneWithCashGameHandHistories.ShouldBeFalse();

            It should_not_be_done_with_TournamentHistories = () => _sut.DoneWithTournamentHandHistories.ShouldBeFalse();

            It should_reset_last_retrieved_cash_game_table_row = () => _sut.LastCashGameTableRowRetrieved.ShouldEqual(-1);

            It should_reset_last_retrieved_tournament_table_row = () => _sut.LastTournamentTableRowRetrieved.ShouldEqual(-1);

            It should_set_the_number_of_cash_game_histories_to_the_once_returned_from_the_dataprovider_when_queried
                = () => _sut.NumberOfCashGameHandHistories.ShouldEqual(numberOfCashGameHistories);

            It should_set_the_number_of_tournament_histories_to_the_once_returned_from_the_dataprovider_when_queried
                = () => _sut.NumberOfTournamentHandHistories.ShouldEqual(numberOfTournamentHistories);
        }

        [Subject(typeof(PokerOfficeHandHistoryRetriever), "GetNext")]
        public class when_not_done_with_cash_game_last_cash_game_hand_retrieved_is_1_number_of_CashGameHands_is_10_and_told_to_get_the_next_2_hands : PokerOfficeHandHistoryRetrieverSpecs
        {
            const int numberOfCashGameHistories = 10;

            const int lastCashGameRowRetrieved = 1;

            const int batchSize = 2;

            Establish context = () => {
                _sut.NumberOfCashGameHandHistories = numberOfCashGameHistories;
                _sut.LastCashGameTableRowRetrieved = lastCashGameRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_2_and_4 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(2);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(4);
            };

            It should_update_the_last_cash_game_hand_retrieved_to_4 = () => _sut.LastCashGameTableRowRetrieved.ShouldEqual(4);

            It should_not_be_done_with_CashGameHistories = () => _sut.DoneWithCashGameHandHistories.ShouldBeFalse();
        }

        [Subject(typeof(PokerOfficeHandHistoryRetriever), "GetNext")]
        public class when_not_done_with_cash_game_last_cash_game_hand_retrieved_is_7_number_of_CashGameHands_is_10_and_told_to_get_the_next_5_hands : PokerOfficeHandHistoryRetrieverSpecs
        {
            const int numberOfCashGameHistories = 10;

            const int lastCashGameRowRetrieved = 7;

            const int batchSize = 5;

            Establish context = () => {
                _sut.NumberOfCashGameHandHistories = numberOfCashGameHistories;
                _sut.LastCashGameTableRowRetrieved = lastCashGameRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_8_and_13 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(8);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(13);
            };

            It should_update_the_last_cash_game_hand_retrieved_to_13 = () => _sut.LastCashGameTableRowRetrieved.ShouldEqual(13);

            It should_be_done_with_CashGameHistories = () => _sut.DoneWithCashGameHandHistories.ShouldBeTrue();
        }

        [Subject(typeof(PokerOfficeHandHistoryRetriever), "GetNext")]
        public class when_done_with_cash_game_last_tournament_hand_retrieved_is_1_number_of_TournamentHands_is_10_and_told_to_get_the_next_2_hands : PokerOfficeHandHistoryRetrieverSpecs
        {
            const int numberOfTournamentHistories = 10;

            const int lastTournamentRowRetrieved = 1;

            const int batchSize = 2;

            Establish context = () => {
                _sut.DoneWithCashGameHandHistories = true;
                _sut.NumberOfTournamentHandHistories = numberOfTournamentHistories;
                _sut.LastTournamentTableRowRetrieved = lastTournamentRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_2_and_4 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(2);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(4);
            };

            It should_update_the_last_tournament_hand_retrieved_to_4 = () => _sut.LastTournamentTableRowRetrieved.ShouldEqual(4);

            It should_not_be_done_with_TournamentHistories = () => _sut.DoneWithTournamentHandHistories.ShouldBeFalse();
        }

        [Subject(typeof(PokerOfficeHandHistoryRetriever), "GetNext")]
        public class when_done_with_cash_game_last_tournament_hand_retrieved_is_7_number_of_TournamentHands_is_10_and_told_to_get_the_next_5_hands : PokerOfficeHandHistoryRetrieverSpecs
        {
            const int numberOfTournamentHistories = 10;

            const int lastTournamentRowRetrieved = 7;

            const int batchSize = 5;

            Establish context = () => {
                _sut.DoneWithCashGameHandHistories = true;
                _sut.NumberOfTournamentHandHistories = numberOfTournamentHistories;
                _sut.LastTournamentTableRowRetrieved = lastTournamentRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_8_and_13 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(8);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(13);
            };

            It should_update_the_last_tournament_hand_retrieved_to_13 = () => _sut.LastTournamentTableRowRetrieved.ShouldEqual(13);

            It should_be_done_with_TournamentHistories = () => _sut.DoneWithTournamentHandHistories.ShouldBeTrue();
        }
    }

    public class PokerOfficeHandHistoryRetrieverSut : PokerOfficeHandHistoryRetriever
    {
        public int MinForWhichHandHistoriesWereQueried;

        public int MaxForWhichHandHistoriesWereQueried;

        public int NumberOfCashGameHandHistories
        {
            get { return _numberOfCashGameHandHistories; }
            set { _numberOfCashGameHandHistories = value; }
        }

        public int NumberOfTournamentHandHistories
        {
            get { return _numberOfTournamentHandHistories; }
            set { _numberOfTournamentHandHistories = value; }
        }

        public bool DoneWithCashGameHandHistories
        {
            get { return _doneWithCashGameHandHistories; }
            set { _doneWithCashGameHandHistories = value; }
        }

        public bool DoneWithTournamentHandHistories
        {
            get { return _doneWithTournamentHandHistories; }
            set { _doneWithTournamentHandHistories = value; }
        }

        public int LastCashGameTableRowRetrieved
        {
            get { return _lastCashGameTableRowRetrieved; }
            set { _lastCashGameTableRowRetrieved = value; }
        }

        public int LastTournamentTableRowRetrieved
        {
            get { return _lastTournamentTableRowRetrieved; }
            set { _lastTournamentTableRowRetrieved = value; }
        }

        protected override IEnumerable<string> QueryHandHistories(int min, int max)
        {
            MinForWhichHandHistoriesWereQueried = min;
            MaxForWhichHandHistoriesWereQueried = max;
            return new List<string>();
        }

    }
}
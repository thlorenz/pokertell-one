namespace PokerTell.Repository.Tests.Database
{
    using Base;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Machine.Specifications;

    using Moq;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class SimpleHandHistoryRetrieverBaseSpecs
    {
        
        static Mock<IDataProvider> _dataProvider_Mock;

        static SimpleHandHistoryRetrieverSut _sut;

        Establish specContext = () => {
            _sut = new SimpleHandHistoryRetrieverSut();

            _dataProvider_Mock = new Mock<IDataProvider>();
        };


        [Subject(typeof(SimpleHandHistoryRetrieverBase), "Using")]
        public class when_told_to_use_a_dataprovider : SimpleHandHistoryRetrieverBaseSpecs
        {
            const int numberOfHandHistories = 1;

            Establish context = () => {
                _sut.Set_IsDone(true);

                _dataProvider_Mock
                    .Setup(dp => dp.ExecuteScalar(_sut.NumberOfHandHistoriesQuery))
                    .Returns(numberOfHandHistories);
            };

            Because of = () => _sut.Using(_dataProvider_Mock.Object);

            It should_not_be_done = () => _sut.IsDone.ShouldBeFalse();

            It should_reset_last_retrieved_row = () => _sut.LastRowRetrieved.ShouldEqual(-1);

            It should_set_the_number_of_histories_to_the_once_returned_from_the_dataprovider_when_queried
                = () => _sut.NumberOfHandHistories.ShouldEqual(numberOfHandHistories);
        }

        [Subject(typeof(SimpleHandHistoryRetrieverBase), "GetNext")]
        public class when_last_hand_retrieved_is_1_number_of_Hands_is_10_and_told_to_get_the_next_2_hands : SimpleHandHistoryRetrieverBaseSpecs
        {
            const int numberOfHistories = 10;

            const int lastRowRetrieved = 1;
 
            const int batchSize = 2;

            Establish context = () => {
                _sut.NumberOfHandHistories = numberOfHistories;
                _sut.LastRowRetrieved = lastRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_2_and_4 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(2);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(4);
            };

            It should_update_the_last_hand_retrieved_to_4 = () => _sut.LastRowRetrieved.ShouldEqual(4);

            It should_not_be_done = () => _sut.IsDone.ShouldBeFalse();
        }

        [Subject(typeof(SimpleHandHistoryRetrieverBase), "GetNext")]
        public class when_last_hand_retrieved_is_7_number_of_Hands_is_10_and_told_to_get_the_next_5_hands : SimpleHandHistoryRetrieverBaseSpecs
        {
            const int numberOfHistories = 10;

            const int lastRowRetrieved = 7;

            const int batchSize = 5;

            Establish context = () => {
                _sut.NumberOfHandHistories = numberOfHistories;
                _sut.LastRowRetrieved = lastRowRetrieved;
            };

            Because of = () => _sut.GetNext(batchSize);

            It should_query_the_next_handhistories_between_8_and_13 = () => {
                _sut.MinForWhichHandHistoriesWereQueried.ShouldEqual(8);
                _sut.MaxForWhichHandHistoriesWereQueried.ShouldEqual(13);
            };

            It should_update_the_last_hand_retrieved_to_13 = () => _sut.LastRowRetrieved.ShouldEqual(13);

            It should_be_done_with = () => _sut.IsDone.ShouldBeTrue();
        }
    }
}
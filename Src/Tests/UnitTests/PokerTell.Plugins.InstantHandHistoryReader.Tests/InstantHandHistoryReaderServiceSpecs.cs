namespace PokerTell.Plugins.InstantHandHistoryReader.Tests
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using It=Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class InstantHandHistoryReaderServiceSpecs
    {
        protected static Mock<IRepository> _repository_Mock;

        protected static Mock<IHandHistoryReader> _handHistoryReader_Mock;

        const string FoundHandHistories = "some data";

        protected static InstantHandHistoryReaderService _sut;

        Establish specContext = () =>
        {
            _repository_Mock = new Mock<IRepository>();
            _handHistoryReader_Mock = new Mock<IHandHistoryReader>();
            _handHistoryReader_Mock
                .Setup(r => r.FindNewInstantHandHistories())
                .Returns(new[] { FoundHandHistories });

            _sut = new InstantHandHistoryReaderService(_repository_Mock.Object, _handHistoryReader_Mock.Object);
        };

        [Subject(typeof(InstantHandHistoryReaderService), "ReadHandHistoriesFromMemory")]
        public class when_the_hand_History_Reader_has_not_been_initialized_yet : InstantHandHistoryReaderServiceSpecs
        {
            Establish context = () => _handHistoryReader_Mock
                .SetupGet(r => r.WasSuccessfullyInitialized)
                .Returns(false);

            Because of = () => _sut.ReadInstantHandHistoriesFromMemory(null);

            It should_initialize_the_hand_history_reader
                = () => _handHistoryReader_Mock.Verify(r => r.InitializeWith(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()));
        }

        [Subject(typeof(InstantHandHistoryReaderService), "ReadHandHistoriesFromMemory")]
        public class when_the_hand_History_Reader_has_been_initialized : InstantHandHistoryReaderServiceSpecs
        {
            Establish context = () => _handHistoryReader_Mock
                .SetupGet(r => r.WasSuccessfullyInitialized)
                .Returns(true);

            Because of = () => _sut.ReadInstantHandHistoriesFromMemory(null);

            It should_not_initialize_the_hand_history_reader_again
                = () => _handHistoryReader_Mock.Verify(r => r.InitializeWith(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()), Times.Never());
        }

        [Subject(typeof(InstantHandHistoryReaderService), "ReadHandHistoriesFromMemory")]
        public class when_hand_history_reader_throws_an_error_during_initialization : InstantHandHistoryReaderServiceSpecs
        {
            Establish context = () =>
            {
                _handHistoryReader_Mock
                    .SetupGet(r => r.WasSuccessfullyInitialized)
                    .Returns(false);
                _handHistoryReader_Mock
                    .Setup(r => r.InitializeWith(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                    .Throws(new Exception());
            };

            Because of = () => _sut.ReadInstantHandHistoriesFromMemory(null);

            It should_not_ask_the_hand_history_reader_to_find_new_hands = () => _handHistoryReader_Mock.Verify(r => r.FindNewInstantHandHistories(), Times.Never());

            It should_not_ask_the_repository_to_RetrieveHandsFromString = () => _repository_Mock.Verify(r => r.RetrieveHandsFromString(FoundHandHistories), Times.Never());

            It should_not_ask_the_repository_to_insert_any_hands
                = () => _repository_Mock.Verify(r => r.InsertHands(Moq.It.IsAny<IEnumerable<IConvertedPokerHand>>()), Times.Never());
        }

        [Subject(typeof(InstantHandHistoryReaderService), "ReadHandHistoriesFromMemory")]
        public class when_hand_history_reader_throws_an_error_when_trying_to_find_hands : InstantHandHistoryReaderServiceSpecs
        {
            Establish context = () => _handHistoryReader_Mock
                                          .Setup(r => r.FindNewInstantHandHistories())
                                          .Throws(new Exception());

            Because of = () => _sut.ReadInstantHandHistoriesFromMemory(null);

            It should_not_ask_the_repository_to_RetrieveHandsFromString = () => _repository_Mock.Verify(r => r.RetrieveHandsFromString(FoundHandHistories), Times.Never());

            It should_not_ask_the_repository_to_insert_any_hands
                = () => _repository_Mock.Verify(r => r.InsertHands(Moq.It.IsAny<IEnumerable<IConvertedPokerHand>>()), Times.Never());
        }


        [Subject(typeof(InstantHandHistoryReaderService), "ReadHandHistoriesFromMemory")]
        public class when_hand_history_reader_throws_no_errors : InstantHandHistoryReaderServiceSpecs
        {
            static IEnumerable<IConvertedPokerHand> pokerHands_Stub;

            Establish context = () => {
                pokerHands_Stub = new[] { new Mock<IConvertedPokerHand>().Object };

                _repository_Mock
                    .Setup(r => r.RetrieveHandsFromString(Moq.It.IsAny<string>()))
                    .Returns(pokerHands_Stub);
            };

            Because of = () => _sut.ReadInstantHandHistoriesFromMemory(null);

            It should_ask_the_repository_to_RetrieveHandsFromString = () => _repository_Mock.Verify(r => r.RetrieveHandsFromString("\n\n" + FoundHandHistories));

            It should_ask_the_repository_to_insert_the_retrieved_hands = () => _repository_Mock.Verify(r => r.InsertHands(pokerHands_Stub));
        }
    }
}
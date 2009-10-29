namespace PokerTell.Repository.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHandParsers;

    public class ThatRepositoryParser
    {
        #region Constants and Fields

        const string FileName = "some FileName";

        const string SomeHandHistoriesString = "handhistoriesContent";

        Mock<IPokerHandParser> _mockParser;

        StubBuilder _stub;

        AvailableParsers _parsers;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _mockParser = new Mock<IPokerHandParser>();
            _parsers = new AvailableParsers();
        }

        [Test]
        public void Constructor_EmptyParserList_ThrowsUnrecognizedHandHistoryFormatException()
        {
            Assert.Throws<ArgumentException>(
                () =>
                new RepositoryParser(_parsers, _stub.Out<IPokerHandConverter>()));
        }

        [Test]
        public void RetrieveAndConvert_EmptyString_ThrowsUnrecognizedHandHistoryFormatException()
        {
            _parsers.Add(_mockParser.Object);

            var repositoryParser = new RepositoryParser(_parsers, _stub.Out<IPokerHandConverter>());

            Assert.Throws<UnrecognizedHandHistoryFormatException>(
                () => repositoryParser.RetrieveAndConvert(string.Empty, FileName));
        }

        [Test]
        public void RetrieveAndConvert_NoParserRecognizesHandHistory_ThrowsUnrecognizedHandHistoryFormatException()
        {
            const bool recognizeHandHistories = false;
            _mockParser
                .Setup(parser => parser.RecognizesHandHistoriesIn(SomeHandHistoriesString))
                .Returns(recognizeHandHistories);
            _parsers.Add(_mockParser.Object);

            var repositoryParser = new RepositoryParser(_parsers, _stub.Out<IPokerHandConverter>());

            Assert.Throws<UnrecognizedHandHistoryFormatException>(
                () => repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName));
        }

        [Test]
        public void RetrieveAndConvert_ParserCanParseIt_ReturnsConvertedPokerHands()
        {
            const int extractedGameId = 1;

            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesAndSeparatesHandsAndParseReturnsAquiredHandWith(extractedGameId);

            _parsers.Add(stubParser.Object);

            Mock<IPokerHandConverter> mockConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, mockConverter.Object);
            IEnumerable<IConvertedPokerHand> convertedHands =
                repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            Assert.That(convertedHands.First().GameId, Is.EqualTo(extractedGameId));
        }

        [Test]
        public void RetrieveAndConvert_ParserRecognizesHandHistory_SeparatesHandHistories()
        {
            Mock<IPokerHandParser> mockParser =
                ParserThatRecognizesHandsButReturnsEmptyWhenExtractingHands(SomeHandHistoriesString);

            _parsers.Add(mockParser.Object);

            var repositoryParser = new RepositoryParser(_parsers, _stub.Out<IPokerHandConverter>());

            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockParser.Verify(parser => parser.ExtractSeparateHandHistories(SomeHandHistoriesString));
        }

        [Test]
        public void RetrieveAndConvert_ParserSeparateHandHistoriesReturnsEmpty_DoesNotCallConvertOnHandConverter()
        {
            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesHandsButReturnsEmptyWhenExtractingHands(SomeHandHistoriesString);

            _parsers.Add(stubParser.Object);

            var mockHandConverter = new Mock<IPokerHandConverter>();

            var repositoryParser = new RepositoryParser(_parsers, mockHandConverter.Object);

            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockHandConverter.Verify(
                converter => converter.ConvertAquiredHand(It.IsAny<IAquiredPokerHand>()), Times.Never());
        }

        [Test]
        public void RetrieveAndConvert_ParserSeparateHandHistoriesReturnsEmpty_DoesNotCallParseOnParser()
        {
            Mock<IPokerHandParser> mockParser =
                ParserThatRecognizesHandsButReturnsEmptyWhenExtractingHands(SomeHandHistoriesString);

            _parsers.Add(mockParser.Object);

            var repositoryParser = new RepositoryParser(_parsers, _stub.Out<IPokerHandConverter>());

            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockParser.Verify(parser => parser.ParseHand(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void
            RetrieveAndConvert_ParserSeparateHandHistoriesReturnsNonEmpty_CallsConvertOnConverterOnceWithThatHistory()
        {
            const int extractedGameId = 1;
            Expression<Predicate<IAquiredPokerHand>> handToBeConverted = h => h.GameId == extractedGameId;
            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesAndSeparatesHandsAndParseReturnsAquiredHandWith(extractedGameId);

            _parsers.Add(stubParser.Object);

            Mock<IPokerHandConverter> mockConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, mockConverter.Object);
            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockConverter.Verify(
                c => c.ConvertAquiredHand(It.Is(handToBeConverted)), Times.Once());
        }

        [Test]
        public void RetrieveAndConvert_ParserSeparateHandHistoriesReturnsNonEmpty_CallsParseOnParserOnceWithThatHistory(
            )
        {
            const string extractedHistory = "someHistory";
            const int extractedGameId = 1;
            var extractedHistories = new Dictionary<ulong, string> { { extractedGameId, extractedHistory } };

            Mock<IPokerHandParser> mockParser =
                ParserThatRecognizesHandsAndWhenExtractingHandsReturns(extractedHistories, SomeHandHistoriesString);
            _parsers.Add(mockParser.Object);

            Mock<IPokerHandConverter> stubConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, stubConverter.Object);
            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockParser.Verify(parser => parser.ParseHand(extractedHistory), Times.Once());
        }

        [Test]
        public void RetrieveAndConvert_SameHandTwice_CallsConvertOnConverterOnlyOnce()
        {
            const int extractedGameId = 1;
            Expression<Predicate<IAquiredPokerHand>> handToBeConverted = h => h.GameId == extractedGameId;
            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesAndSeparatesHandsAndParseReturnsAquiredHandWith(extractedGameId);

            _parsers.Add(stubParser.Object);

            Mock<IPokerHandConverter> mockConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, mockConverter.Object);
            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);
            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockConverter.Verify(
                c => c.ConvertAquiredHand(It.Is(handToBeConverted)), Times.Once());
        }

        [Test]
        public void RetrieveAndConvert_SameHandTwice_CallsParseOnParserOnlyOnce()
        {
            const string extractedHistory = "someHistory";
            const int extractedGameId = 1;
            var extractedHistories = new Dictionary<ulong, string> { { extractedGameId, extractedHistory } };

            Mock<IPokerHandParser> mockParser =
                ParserThatRecognizesHandsAndWhenExtractingHandsReturns(extractedHistories, SomeHandHistoriesString);
            _parsers.Add(mockParser.Object);

            Mock<IPokerHandConverter> stubConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, stubConverter.Object);

            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);
            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            mockParser.Verify(parser => parser.ParseHand(extractedHistory), Times.Once());
        }

        [Test]
        public void RetrieveAndConvert_SameHandTwice_ReturnsConvertedPokerHands()
        {
            const int extractedGameId = 1;

            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesAndSeparatesHandsAndParseReturnsAquiredHandWith(extractedGameId);

            _parsers.Add(stubParser.Object);

            Mock<IPokerHandConverter> mockConverter = ConverterThatReturnsConvertedHandWith(extractedGameId);

            var repositoryParser = new RepositoryParser(_parsers, mockConverter.Object);

            repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);
            IEnumerable<IConvertedPokerHand> convertedHands =
                repositoryParser.RetrieveAndConvert(SomeHandHistoriesString, FileName);

            Assert.That(convertedHands.First().GameId, Is.EqualTo(extractedGameId));
        }

        #endregion

        #region Methods

        static Mock<IPokerHandConverter> ConverterThatReturnsConvertedHandWith(ulong extractedGameId)
        {
            var stub = new StubBuilder();

            IConvertedPokerHand stubConvertedHand = stub.Setup<IConvertedPokerHand>()
                .Get(hand => hand.GameId)
                .Returns(extractedGameId).Out;

            var mockConverter = new Mock<IPokerHandConverter>();
            mockConverter
                .Setup(c => c.ConvertAquiredHand(It.IsAny<IAquiredPokerHand>()))
                .Returns(stubConvertedHand);

            return mockConverter;
        }

        static Mock<IPokerHandParser> ParserThatRecognizesAndSeparatesHandsAndParseReturnsAquiredHandWith(ulong gameId)
        {
            const string extractedHistory = "someHistory";
            const int extractedGameId = 1;
            var extractedHistories = new Dictionary<ulong, string> { { extractedGameId, extractedHistory } };

            Mock<IPokerHandParser> stubParser =
                ParserThatRecognizesHandsAndWhenExtractingHandsReturns(extractedHistories, SomeHandHistoriesString);

            var stub = new StubBuilder();
            IAquiredPokerHand stubAquiredHand = stub.Setup<IAquiredPokerHand>()
                .Get(p => p.GameId)
                .Returns(gameId).Out;

            stubParser
                .Setup(p => p.ParseHand(It.IsAny<string>()))
                .Returns(stubAquiredHand);

            return stubParser;
        }

        static Mock<IPokerHandParser> ParserThatRecognizesHandsAndWhenExtractingHandsReturns(
            IDictionary<ulong, string> extractedHistories, string handHistoriesContent)
        {
            const bool recognizeHandHistories = true;

            var mockParser = new Mock<IPokerHandParser>();

            mockParser
                .Setup(parser => parser.RecognizesHandHistoriesIn(handHistoriesContent))
                .Returns(recognizeHandHistories);
            mockParser
                .Setup(parser => parser.ExtractSeparateHandHistories(handHistoriesContent))
                .Returns(extractedHistories);
            return mockParser;
        }

        static Mock<IPokerHandParser> ParserThatRecognizesHandsButReturnsEmptyWhenExtractingHands(
            string handHistoriesContent)
        {
            const bool recognizeHandHistories = true;

            var mockParser = new Mock<IPokerHandParser>();

            mockParser
                .Setup(parser => parser.RecognizesHandHistoriesIn(handHistoriesContent))
                .Returns(recognizeHandHistories);
            mockParser
                .Setup(parser => parser.ExtractSeparateHandHistories(handHistoriesContent))
                .Returns(new Dictionary<ulong, string>());
            return mockParser;
        }

        #endregion
    }

    internal class AvailableParsers : IPokerHandParsers
    {
        #region Constants and Fields

        readonly IList<IPokerHandParser> _parsers;

        #endregion

        public AvailableParsers()
        {
            _parsers = new List<IPokerHandParser>();
        }

        #region Constructors and Destructors
        
        public IPokerHandParsers Add(IPokerHandParser parser)
        {
            _parsers.Add(parser);
            return this;
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<IPokerHandParser>

        public IEnumerator<IPokerHandParser> GetEnumerator()
        {
            return _parsers.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
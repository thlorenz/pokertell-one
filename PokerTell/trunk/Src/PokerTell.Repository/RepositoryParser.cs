namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Infrastructure.Interfaces.PokerHandParsers;

    using Interfaces;

    using log4net;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class RepositoryParser : IRepositoryParser
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        #region Constants and Fields

        readonly IDictionary<ulong, IConvertedPokerHand> _parsedHands;

        readonly IPokerHandConverter _pokerHandConverter;

        readonly IPokerHandParsers _pokerHandParsers;

        #endregion

        #region Constructors and Destructors

        public RepositoryParser(IPokerHandParsers pokerHandParsers, IPokerHandConverter pokerHandConverter)
        {
            if (pokerHandParsers.Count() < 1)
            {
                throw new ArgumentException("pokerHandParsers is empty");
            }
            
            _pokerHandConverter = pokerHandConverter;
            _pokerHandParsers = pokerHandParsers;

            _parsedHands = new Dictionary<ulong, IConvertedPokerHand>();
        }

        #endregion

        #region Methods

        public IEnumerable<IConvertedPokerHand> RetrieveAndConvert(string handHistories, string fileName)
        {
            IPokerHandParser parser = FindCorrectParserFor(handHistories);

            bool didNotFindParser = parser == null;

            if (didNotFindParser)
            {
                throw new UnrecognizedHandHistoryFormatException(fileName);
            }

            IDictionary<ulong, string> containedHandHistories = parser.ExtractSeparateHandHistories(handHistories);
            
            return CollectHandsContainedIn(containedHandHistories, parser);
        }

        IEnumerable<IConvertedPokerHand> CollectHandsContainedIn(
            IEnumerable<KeyValuePair<ulong, string>> containedHandHistories, IPokerHandParser parser)
        {
            var convertedPokerHands = new List<IConvertedPokerHand>();

            foreach (KeyValuePair<ulong, string> handHistory in containedHandHistories)
            {
                try
                {
                  IConvertedPokerHand convertedPokerHand =
                      GetConvertedHandHistoryFromPreviouslyParsedHandsOrParser(handHistory, parser);

                    convertedPokerHands.Add(convertedPokerHand);
                }
                catch (Exception excep)
                {
                    Log.Error(excep);
                }
               
            }
            return convertedPokerHands;
        }

        IConvertedPokerHand ConvertHandAndAddToParsedHands(IAquiredPokerHand aquiredPokerHand)
        {
            IConvertedPokerHand convertedPokerHand = _pokerHandConverter.ConvertAquiredHand(aquiredPokerHand);
            if (convertedPokerHand != null)
            {
                _parsedHands.Add(convertedPokerHand.GameId, convertedPokerHand);
                return convertedPokerHand;
            }

            throw new NullReferenceException("convertedPokerHand when converting: \n" + aquiredPokerHand);
        }

        IPokerHandParser FindCorrectParserFor(string handHistories)
        {
            foreach (IPokerHandParser parser in _pokerHandParsers)
            {
                if (parser.RecognizesHandHistoriesIn(handHistories))
                {
                    return parser;
                }
            }

            return null;
        }

        IConvertedPokerHand GetConvertedHandHistoryFromPreviouslyParsedHandsOrParser(
            KeyValuePair<ulong, string> handHistory, IPokerHandParser parser)
        {
            if (_parsedHands.ContainsKey(handHistory.Key))
            {
                return _parsedHands[handHistory.Key];
            }

            IAquiredPokerHand aquiredPokerHand;

            if (parser.ParseHand(handHistory.Value).IsValid)
            {
                aquiredPokerHand = parser.AquiredPokerHand;
            }
            else
            {
                throw new UnableToParseHandHistoryException("Parser: " + parser);
            }

            return ConvertHandAndAddToParsedHands(aquiredPokerHand);
        }

        #endregion
    }
}
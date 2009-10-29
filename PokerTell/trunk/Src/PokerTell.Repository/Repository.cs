namespace PokerTell.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Infrastructure.Interfaces.Repository;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class Repository : IRepository
    {
        #region Constants and Fields

        readonly IDictionary<int, IConvertedPokerHand> _cachedHands;

        readonly RepositoryParser _parser;

        #endregion

        #region Constructors and Destructors

        public Repository(RepositoryParser parser)
        {
            _parser = parser;
            _cachedHands = new Dictionary<int, IConvertedPokerHand>();
        }

        #endregion

        #region Properties

        public IDictionary<int, IConvertedPokerHand> CachedHands
        {
            get { return _cachedHands; }
        }

        #endregion

        #region Public Methods

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            string handHistories = ReadHandHistoriesFrom(fileName);

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        static string ReadHandHistoriesFrom(string fileName)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                // Use UTF7 encoding to ensure correct representation of Umlauts
                return new StreamReader(fileStream, Encoding.UTF7).ReadToEnd();
            }
        }
        #endregion
    }
}
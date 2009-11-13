namespace PokerTell.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class Repository : IRepository
    {
        #region Constants and Fields

        readonly IDictionary<int, IConvertedPokerHand> _cachedHands;

        readonly IRepositoryParser _parser;

        readonly IRepositoryDatabase _database;

        #endregion

        #region Constructors and Destructors

        public Repository(IEventAggregator eventAggregator, IRepositoryDatabase database, IRepositoryParser parser)
        {
            _database = database;
            _parser = parser;
            _cachedHands = new Dictionary<int, IConvertedPokerHand>();

            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dataProvider => {
                    _cachedHands.Clear();
                    Use(dataProvider);
                });
        }

        #endregion

        #region Properties

        public IDictionary<int, IConvertedPokerHand> CachedHands
        {
            get { return _cachedHands; }
        }

        #endregion

        #region Public Methods

        public IRepository Use(IDataProvider dataProvider)
        {
            _database.Use(dataProvider);
            return this;
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            string handHistories = ReadHandHistoriesFrom(fileName);

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        public IRepository InsertHand(IConvertedPokerHand convertedPokerHand)
        {
            if (_database.IsConnected && convertedPokerHand != null)
            {
               int handId = _database.InsertHandAndReturnHandId(convertedPokerHand);

               if (!CachedHands.ContainsKey(handId))
               {
                   CachedHands.Add(handId, convertedPokerHand);
               }
            }

            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            if (!CachedHands.ContainsKey(handId))
            {
                if (_database.IsConnected)
                {
                    IConvertedPokerHand retrievedHand = _database.RetrieveConvertedHand(handId);
                    CachedHands.Add(handId, retrievedHand);
                }
                else
                {
                    return null;
                }
            }

            return CachedHands[handId];
        }

        public IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds)
        {
            foreach (int handId in handIds)
            {
                yield return RetrieveConvertedHand(handId);
            }
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
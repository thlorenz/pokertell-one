namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class Repository : IRepository
    {
        #region Constants and Fields

        readonly IRepositoryParser _parser;

        ISessionFactory _sessionFactory;

        readonly IConvertedPokerHandDao _pokerHandDao;

        #endregion

        #region Constructors and Destructors

        public Repository(
            IEventAggregator eventAggregator, IConvertedPokerHandDao pokerHandDao, IRepositoryParser parser)
        {
            _pokerHandDao = pokerHandDao;
            _parser = parser;

            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dataProvider => Use(dataProvider));
        }

        #endregion

        #region Public Methods

        public IRepository Use(IDataProvider dataProvider)
        {
            _sessionFactory = dataProvider.BuildSessionFactory();
           return this;
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            string handHistories = ReadHandHistoriesFrom(fileName);

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        public IRepository InsertHand(IConvertedPokerHand convertedPokerHand)
        {
            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            return null;
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

        public IRepository InsertHands(IEnumerable<IConvertedPokerHand> handsToInsert)
        {
           return this;
        }
    }
}
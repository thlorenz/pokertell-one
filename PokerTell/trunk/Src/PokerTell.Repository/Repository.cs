namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;

    using Microsoft.Practices.Composite.Events;

    using global::NHibernate;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;

    public class Repository : IRepository
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRepositoryParser _parser;

        readonly IConvertedPokerHandDaoFactory _pokerHandDaoMake;

        readonly ITransactionManagerFactory _transactionManagerMake;

        ISessionFactory _sessionFactory;

        #endregion

        #region Constructors and Destructors

        public Repository(IEventAggregator eventAggregator, IConvertedPokerHandDaoFactory pokerHandDaoMake, ITransactionManagerFactory transactionManagerMake, IRepositoryParser parser)
        {
            _transactionManagerMake = transactionManagerMake;
            _pokerHandDaoMake = pokerHandDaoMake;
            _parser = parser;

            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dataProvider => Use(dataProvider));
        }

        #endregion

        #region Implemented Interfaces

        #region IRepository

        public IRepository InsertHand(IConvertedPokerHand convertedPokerHand)
        {
            if (_sessionFactory != null)
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    var pokerHandDao = _pokerHandDaoMake.New(session);
                    
                    _transactionManagerMake
                        .New(session.BeginTransaction())
                        .Execute(() => pokerHandDao.Insert(convertedPokerHand));
                }
            }

            return this;
        }

        public IRepository InsertHands(IEnumerable<IConvertedPokerHand> handsToInsert)
        {
            using (var statelessSession = _sessionFactory.OpenStatelessSession())
            {
                var pokerHandDao = _pokerHandDaoMake.New(statelessSession);
                
                using (ITransaction tx = statelessSession.BeginTransaction())
                {
                    foreach (IConvertedPokerHand convertedHand in handsToInsert)
                    {
                        pokerHandDao.InsertFast(convertedHand);
                    }

                    tx.Commit();
                }
            }
           
            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHandWith(ulong gameId, string site)
        {
            if (_sessionFactory != null)
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    return _pokerHandDaoMake.New(session)
                        .GetHandWith(gameId, site);
                }
            }

            return null;
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            if (_sessionFactory != null)
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    return _pokerHandDaoMake.New(session)
                        .Get(handId);
                }
            }

            return null;
        }

        public IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds)
        {
            foreach (int handId in handIds)
            {
                yield return RetrieveConvertedHand(handId);
            }
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            string handHistories = ReadHandHistoriesFrom(fileName);

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        public IRepository Use(IDataProvider dataProvider)
        {
            _sessionFactory = dataProvider.NewSessionFactory;
            return this;
        }

        #endregion

        #endregion

        #region Methods

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
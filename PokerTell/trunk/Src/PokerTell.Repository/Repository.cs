namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using log4net;

    using Microsoft.Practices.Composite.Events;

    using global::NHibernate;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;
    using PokerTell.Repository.NHibernate;

    public class Repository : IRepository
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRepositoryParser _parser;

        readonly IConstructor<IConvertedPokerHandDaoWithoutSession> _pokerHandDaoMake;

        readonly IConstructor<ITransactionManagerWithoutTransaction> _transactionManagerMake;

        IDataProvider _dataProvider;

        ISessionFactory _sessionFactory;

        #endregion

        #region Constructors and Destructors

        public Repository(IEventAggregator eventAggregator, IConstructor<IConvertedPokerHandDaoWithoutSession> pokerHandDaoMake, IConstructor<ITransactionManagerWithoutTransaction> transactionManagerMake, IRepositoryParser parser)
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
                    var pokerHandDao = _pokerHandDaoMake.New.InitializeWith(session);
                    _transactionManagerMake.New
                        .InitializeWith(session.BeginTransaction())
                        .Execute(() => pokerHandDao.Insert(convertedPokerHand));
                }
            }

            return this;
        }

        public IRepository InsertHands(IEnumerable<IConvertedPokerHand> handsToInsert)
        {
            if (_sessionFactory != null)
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    var pokerHandDao = _pokerHandDaoMake.New.InitializeWith(session);

                    ITransactionManagerWithTransaction initializedTransaction = _transactionManagerMake.New
                        .InitializeWith(session.BeginTransaction());

                    ITransactionManagerUncommitted uncommittedTransaction = null;

                    foreach (IConvertedPokerHand convertedHand in handsToInsert)
                    {
                        // Avoid access to modified closure
                        IConvertedPokerHand hand = convertedHand;

                        uncommittedTransaction = initializedTransaction
                            .ExecuteWithoutCommitting(() => pokerHandDao.Insert(hand));
                    }

                    if (uncommittedTransaction != null)
                    {
                        uncommittedTransaction.Commit();
                    }
                }
            }

            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHandWith(ulong gameId, string site)
        {
            return null;
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            if (_sessionFactory != null)
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    return _pokerHandDaoMake.New
                        .InitializeWith(session)
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
            _dataProvider = dataProvider;
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
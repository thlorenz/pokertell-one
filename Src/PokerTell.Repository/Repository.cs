namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using log4net;

    using global::NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;

    public class Repository : IRepository
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRepositoryParser _parser;

        readonly IConvertedPokerHandDao _pokerHandDao;

        readonly ITransactionManager _transactionManager;

        readonly IConvertedPokerPlayerDao _pokerPlayerDao;

        readonly IPlayerIdentityDao _playerIdentityDao;

        public Repository(
            IConvertedPokerHandDao pokerHandDao, 
            IConvertedPokerPlayerDao pokerPlayerDao, 
            IPlayerIdentityDao playerIdentityDao, 
            ITransactionManager transactionManager, 
            IRepositoryParser parser)
        {
            _pokerHandDao = pokerHandDao;
            _pokerPlayerDao = pokerPlayerDao;
            _playerIdentityDao = playerIdentityDao;
            _transactionManager = transactionManager;
            _parser = parser;
        }

        public IRepository InsertHand(IConvertedPokerHand convertedPokerHand)
        {
            _transactionManager.Execute((Action)(() => _pokerHandDao.Insert(convertedPokerHand)));

            return this;
        }

        public IRepository InsertHands(IEnumerable<IConvertedPokerHand> handsToInsert)
        {
            Action<IStatelessSession> insertHands = statelessSession => {
                foreach (IConvertedPokerHand convertedHand in handsToInsert)
                {
                    try
                    {
                        _pokerHandDao.Insert(convertedHand, statelessSession);
                    }
                    catch (Exception excep)
                    {
                        Log.Error(excep);
                    }
                }
            };
            _transactionManager.BatchExecute(insertHands);

            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHandWith(ulong gameId, string site)
        {
            return _transactionManager.Execute(() => _pokerHandDao.GetHandWith(gameId, site));
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            return _transactionManager.Execute(() => _pokerHandDao.Get(handId));
        }

        public IPlayerIdentity FindPlayerIdentityFor(string name, string site)
        {
            return _transactionManager.Execute(() => _playerIdentityDao.FindPlayerIdentityFor(name, site));
        }

        public IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds)
        {
            foreach (int handId in handIds)
            {
                yield return RetrieveConvertedHand(handId);
            }
        }

        public IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId)
        {
            return _transactionManager.Execute(
                () => _pokerPlayerDao.FindAnalyzablePlayersWith(playerIdentity, lastQueriedId));
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            string handHistories = ReadHandHistoriesFrom(fileName);

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        static string ReadHandHistoriesFrom(string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // Use UTF7 encoding to ensure correct representation of Umlauts
                    return new StreamReader(fileStream, Encoding.UTF7).ReadToEnd();
                }
            }
            catch (Exception excep)
            {
                // Should only throw during manual saving of file from notepad
                Log.Error(excep);
                
               // Try again
               return ReadHandHistoriesFrom(fileName);
            }
        }
    }
}
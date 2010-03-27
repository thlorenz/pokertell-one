namespace PokerTell.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;

    using global::NHibernate;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;

    public class Repository : IRepository
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRepositoryParser _parser;

        readonly IPlayerIdentityDao _playerIdentityDao;

        readonly IConvertedPokerHandDao _pokerHandDao;

        readonly IConvertedPokerPlayerDao _pokerPlayerDao;

        readonly ITransactionManager _transactionManager;

        public Repository(IConvertedPokerHandDao pokerHandDao, IConvertedPokerPlayerDao pokerPlayerDao, IPlayerIdentityDao playerIdentityDao, ITransactionManager transactionManager, IRepositoryParser parser)
        {
            _pokerHandDao = pokerHandDao;
            _pokerPlayerDao = pokerPlayerDao;
            _playerIdentityDao = playerIdentityDao;
            _transactionManager = transactionManager;
            _parser = parser;
        }

        public IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId)
        {
            return _transactionManager.Execute(
                () => _pokerPlayerDao.FindAnalyzablePlayersWith(playerIdentity, lastQueriedId));
        }

        public IPlayerIdentity FindPlayerIdentityFor(string name, string site)
        {
            return _transactionManager.Execute(() => _playerIdentityDao.FindPlayerIdentityFor(name, site));
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

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            return _transactionManager.Execute(() => _pokerHandDao.Get(handId));
        }

        public IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds)
        {
            foreach (int handId in handIds)
            {
                yield return RetrieveConvertedHand(handId);
            }
        }

        public IConvertedPokerHand RetrieveConvertedHandWith(ulong gameId, string site)
        {
            return _transactionManager.Execute(() => _pokerHandDao.GetHandWith(gameId, site));
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName)
        {
            const int maxTriesToReadFile = 5;
            string handHistories = ReadHandHistoriesFrom(fileName, maxTriesToReadFile);

            if (string.IsNullOrEmpty(handHistories))
            {
                return Enumerable.Empty<IConvertedPokerHand>();
            }

            return _parser.RetrieveAndConvert(handHistories, fileName);
        }

        public IEnumerable<IConvertedPokerHand> RetrieveHandsFromString(string handHistories)
        {
            if (string.IsNullOrEmpty(handHistories))
            {
                return Enumerable.Empty<IConvertedPokerHand>();
            }

            return _parser.RetrieveAndConvert(handHistories, "Memory");
        }

        static string ReadHandHistoriesFrom(string fileName, int remainingTries)
        {
            // Need to insure against reading our own logfile b/c it is locked and written to continously which leads to a crash
            if (fileName.EndsWith(@"\" + ApplicationProperties.LogFileName))
            {
                return string.Empty;
            }

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
                // Should only throw during manual saving of file from notepad or if trying to read some locked file
                // In the latter case the user must have set the tracked handhistory directory too broad e.g C:\
                Log.Error(excep);

                if (remainingTries <= 0)
                {
                    return string.Empty;
                }

                // Try again
                return ReadHandHistoriesFrom(fileName, --remainingTries);
            }
        }
    }
}
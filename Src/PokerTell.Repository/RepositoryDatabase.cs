namespace PokerTell.Repository
{
    using System.Collections.Generic;
    using System.Data;

    using global::NHibernate;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Repository.Interfaces;

    public class RepositoryDatabase : IRepositoryDatabase
    {
        readonly IConvertedPokerHandInserter _convertedPokerHandInserter;

        readonly IConvertedPokerHandRetriever _convertedPokerHandRetriever;

        readonly IDatabaseUtility _databaseUtility;

        IDataProvider _dataProvider;

        ISessionFactory _sessionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDatabase"/> class. 
        /// Creates a database repository.
        /// Requires that the data provider is connected to a database.
        /// All queries will be forwarded to the data provider.
        /// </summary>
        /// </param>
        /// </param>
        public RepositoryDatabase(
            IDatabaseUtility databaseUtility, 
            IConvertedPokerHandInserter convertedPokerHandInserter, 
            IConvertedPokerHandRetriever convertedPokerHandRetriever)
        {
            _databaseUtility = databaseUtility;
            _convertedPokerHandRetriever = convertedPokerHandRetriever;
            _convertedPokerHandInserter = convertedPokerHandInserter;
        }

        public bool IsConnected
        {
            get { return _dataProvider != null && _dataProvider.IsConnectedToDatabase; }
        }

        public IRepositoryDatabase Use(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            return this;
        }

        public int ExecuteNonQuery(string nonQuery)
        {
            return _dataProvider.ExecuteNonQuery(nonQuery);
        }

        public IDataReader ExecuteQuery(string query)
        {
            return _dataProvider.ExecuteQuery(query);
        }

        public IList<T> ExecuteQueryGetColumn<T>(string query, int column)
        {
            return _dataProvider.ExecuteQueryGetColumn<T>(query, column);
        }

        public IList<T> ExecuteQueryGetFirstColumn<T>(string query)
        {
            return ExecuteQueryGetColumn<T>(query, 0);
        }

        /// <summary>
        /// Tries to obtain an integer value from the query
        /// </summary>
        /// <param name="query">Query like Count, sum or max</param>
        /// <returns>Result or -1 if not possible to convert the resul to int</returns>
        public int? ExecuteScalar(string query)
        {
            object objResult = _dataProvider.ExecuteScalar(query);

            int result;
            return int.TryParse(objResult.ToString(), out result) ? new int?(result) : null;
        }

        public DataTable GetDataTableFor(string query)
        {
            return _dataProvider.GetDataTableFor(query);
        }

        /// <summary>
        /// Inserts Hand into database and returns the Id that was assigned to it
        /// </summary>
        /// <param name="convertedHand"></param>
        /// <returns>Id used in the identity column of the gamehhd table of the database</returns>
        public int? InsertHandAndReturnHandId(IConvertedPokerHand convertedHand)
        {
            _convertedPokerHandInserter
                .Use(_dataProvider)
                .Insert(convertedHand);

            return _databaseUtility
                .Use(_dataProvider)
                .GetHandIdForHandWith(convertedHand.GameId, convertedHand.Site);
        }

        public IRepositoryDatabase InsertHandsAndSetTheirHandIds(IEnumerable<IConvertedPokerHand> handsToInsert)
        {
            var insertedHands = new Dictionary<int, IConvertedPokerHand>();

            using (var transaction = _dataProvider.Connection.BeginTransaction())
            {
                foreach (var hand in handsToInsert)
                {
                    if (hand != null)
                    {
                        var handId = InsertHandAndReturnHandId(hand);
                        if (handId != null)
                        {
                            hand.Id = handId.Value;
                        }
                    }
                }

                transaction.Commit();
            }

            return this;
        }

        public IConvertedPokerHand RetrieveConvertedHand(int handId)
        {
            return _convertedPokerHandRetriever
                .Use(_dataProvider)
                .RetrieveHand(handId);
        }

        public IRepositoryDatabase Use(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            return this;
        }
    }
}
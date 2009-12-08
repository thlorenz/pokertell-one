namespace PokerTell.Repository.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    using NUnit.Framework;

    using PokerTell.DatabaseSetup;
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.PokerHand.Dao;

    using UnitTests;

    using Environment = global::NHibernate.Cfg.Environment;

    [TestFixture]
    public class QueryPerformanceTests //: TestWithLog
    {
        #region Constants and Fields

        ISession _session;

        ISessionFactory _sessionFactory;

        Mock<ISessionFactoryManager> _sessionFactoryManagerStub;

        DateTime _startTime;

        #endregion

        #region Public Methods

        [Test]
        public void FindConvertedPokerPlayerById_MySql()
        {
            // Took 10.5s for 22,000 hands
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindConvertedPokerPlayerById_MySql",
                  () => convertedPokerPlayerDao.FindByPlayerIdentity(8));
        }

        [Test]
        public void QueryConvertedPokerPlayersWith_MySql()
        {
            // Took 2.6s for 22,000 hands
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindHandIdsFromPlayerById_MySql",
                  () => convertedPokerPlayerDao.FindAnalyzablePlayersWith(8, 0));
        }

        [Test]
        public void UsingRawSqlQueryConvertedPokerPlayersWith_MySql()
        {
            // Took 3.1s for 22,000 hands
            SetupMySqlConnection("data source = localhost; user id = root; database=firstnh;");
            var convertedPokerPlayerDao = new ConvertedPokerPlayerDao(_sessionFactoryManagerStub.Object);

            Timed("FindStatValuesByPlayerIdentity_MySql", 
                () => convertedPokerPlayerDao.UsingRawSqlQueryConvertedPokerPlayersWith(8));
        }

        #endregion

        #region Methods

        void InitSessionFactoryManager()
        {
            _session = _sessionFactory.OpenSession();
            _sessionFactoryManagerStub = new Mock<ISessionFactoryManager>();
            _sessionFactoryManagerStub
                .SetupGet(sfm => sfm.CurrentSession)
                .Returns(_session);
        }

        void SetupMySqlConnection(string connectionString)
        {
            var dataProviderInfo = new MySqlInfo();
            Configuration configuration = new Configuration()
                .SetProperty(Environment.Dialect, dataProviderInfo.NHibernateDialect)
                .SetProperty(Environment.ConnectionDriver, dataProviderInfo.NHibernateConnectionDriver)
                .SetProperty(Environment.ConnectionString, connectionString)
                .SetProperty(Environment.ShowSql, "true")
                .AddAssembly(ApplicationProperties.MappingAssemblyName);
            _sessionFactory = configuration.BuildSessionFactory();

            InitSessionFactoryManager();
        }

        void Start()
        {
            _startTime = DateTime.Now;
        }

        void Stop()
        {
            TimeSpan duration = DateTime.Now - _startTime;
            Console.WriteLine("Took {0}s {1}ms", duration.Seconds, duration.Milliseconds);
        }

        void Timed(string methodName, Action action)
        {
            Console.WriteLine(methodName);
            Start();
            action();
            Stop();
        }

        #endregion
    }
}
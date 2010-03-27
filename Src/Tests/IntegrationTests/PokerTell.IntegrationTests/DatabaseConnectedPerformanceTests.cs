namespace PokerTell.IntegrationTests
{
    using System;

    using DatabaseSetup;

    using Infrastructure;
    using Infrastructure.Interfaces.Repository;

    using Moq;

    using NHibernate;
    using NHibernate.Cfg;

    using Environment = NHibernate.Cfg.Environment;

    /// <summary>
    /// Provides Methods to setup DatabaseConnection
    /// and to test Performance.
    /// 
    /// Note: To use requires an app.config that defines NHibernate configuration and adds data providers
    /// and references to MySqlData.dll / System.Data.SQLite.dll and NHibernate.ByteCode.Castle.dll
    /// </summary>
    public class DatabaseConnectedPerformanceTests
    {
        ISession _session;

        ISessionFactory _sessionFactory;

        protected Mock<ISessionFactoryManager> _sessionFactoryManagerStub;

        DateTime _startTime;

        void InitSessionFactoryManager()
        {
            _session = _sessionFactory.OpenSession();
          
            _sessionFactoryManagerStub = new Mock<ISessionFactoryManager>();
            _sessionFactoryManagerStub
                .SetupGet(sfm => sfm.CurrentSession)
                .Returns(_session);
            _sessionFactoryManagerStub
                .Setup(sfm => sfm.OpenSession())
                .Returns(_session);
            _sessionFactoryManagerStub
                .SetupGet(sfm => sfm.SessionFactory)
                .Returns(_sessionFactory);
        }

        protected void SetupMySqlConnection(string connectionString)
        {
            var dataProviderInfo = new MySqlInfo();
            Configuration configuration = new Configuration()
                .SetProperty(Environment.Dialect, dataProviderInfo.NHibernateDialect)
                .SetProperty(Environment.ConnectionDriver, dataProviderInfo.NHibernateConnectionDriver)
                .SetProperty(Environment.ConnectionString, connectionString)
                .SetProperty(Environment.ShowSql, "true")
                .AddAssembly(ApplicationProperties.PokerHandMappingAssemblyName);
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

        protected void Timed(string methodName, Action action)
        {
            Console.WriteLine(methodName);
            Start();
            action();
            Stop();
        }
    }
}
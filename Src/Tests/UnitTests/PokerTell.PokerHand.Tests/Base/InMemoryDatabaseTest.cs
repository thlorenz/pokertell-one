namespace PokerTell.PokerHand.Tests.Base
{
    using System;
    using System.Reflection;

    using NHibernate;
    using NHibernate.ByteCode.LinFu;
    using NHibernate.Cfg;
    using NHibernate.Dialect;
    using NHibernate.Driver;

    using UnitTests;

    using Environment = NHibernate.Cfg.Environment;

    public class InMemoryDatabaseTest : IDisposable
    {
        protected static Configuration _configuration;
        protected static ISessionFactory _sessionFactory;
        protected readonly ISession _session;

        protected readonly TestWithLog _log;

        protected readonly IStatelessSession _statelessSession;

        protected const int UnsavedValue = 0;

        public InMemoryDatabaseTest(Assembly assemblyContainingMapping)
            : this(assemblyContainingMapping, false)
        {
        }

        public InMemoryDatabaseTest(Assembly assemblyContainingMapping, bool showLog)
        {
            _log = new TestWithLog();
            if (showLog)
            {
                _log.EnableLogger();
            }

            if (_configuration == null)
            {
                _configuration = new Configuration()
                    .SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source = :memory:")
                    .SetProperty(Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                    .SetProperty(Environment.ShowSql, "false")
                    .AddAssembly(assemblyContainingMapping);

                _sessionFactory = _configuration.BuildSessionFactory();
            }

            _session = _sessionFactory.OpenSession();
            _statelessSession = _sessionFactory.OpenStatelessSession();

            // This needs to go into the [Setup] _Init() method for the derived test class
            // new SchemaExport(_configuration).Execute(true, true, false, _session.Connection, Console.Out);
        }

        protected ISession ClearedSession
        {
            get
            {
                FlushAndClearSession();
                return _session;
            }
        }

        protected void FlushAndClearSession()
        {
            _session.Flush();
            _session.Clear();
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}
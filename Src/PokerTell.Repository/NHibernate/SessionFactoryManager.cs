namespace PokerTell.Repository.NHibernate
{
    using System;

    using global::NHibernate;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Microsoft.Practices.Composite.Events;

    /// <summary>
    /// Singleton class responsible for manageing the NHibernate SessionFactory
    /// </summary>
    public class SessionFactoryManager : ISessionFactoryManager
    {
        ISessionFactory _sessionFactory;

        public SessionFactoryManager(IEventAggregator eventAggregator)
        {
            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dataProvider => Use(dataProvider));
        }

        public ISessionFactoryManager Use(IDataProvider dataProvider)
        {
            if (dataProvider.IsConnectedToDatabase)
            {
                _sessionFactory = dataProvider.BuildSessionFactory();
            }
            else
            {
                throw new ArgumentException("Cannot use unconnected Dataprovider. SessionFactory not recreated.");
            }

            return this;
        }

        public ISession CurrentSession
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
            protected set { _sessionFactory = value; }
        }

        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }

        public IStatelessSession OpenStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession();
        }
    }
}
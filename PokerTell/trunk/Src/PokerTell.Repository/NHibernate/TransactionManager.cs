namespace PokerTell.Repository.NHibernate
{
    using System;
    using System.Reflection;

    using log4net;

    using global::NHibernate;
    using global::NHibernate.Context;

    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;

    public class TransactionManager : ITransactionManager
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISessionFactoryManager _sessionFactoryManager;

        #endregion

        #region Constructors and Destructors

        public TransactionManager(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }

        #endregion

        #region Implemented Interfaces

        #region ITransactionManager

        public ITransactionManager Execute(Action executeTransaction)
        {
            try
            {
                OpenAndBindSession();

                using (_sessionFactoryManager.CurrentSession.BeginTransaction())
                {
                    executeTransaction();
                    _sessionFactoryManager.CurrentSession.Transaction.Commit();
                }
            }
            catch (Exception excep)
            {
                _sessionFactoryManager.CurrentSession.Transaction.Rollback();
                Log.Error(excep);
            }
            finally
            {
                UnbindSession();
            }

            return this;
        }

        public TResult Execute<TResult>(Func<TResult> executeRetrieval)
        {
            try
            {
                OpenAndBindSession();

                return executeRetrieval();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                return default(TResult);
            }
            finally
            {
                UnbindSession();
            }
        }

        public ITransactionManager BatchExecute(Action<IStatelessSession> executeTransaction)
        {
            IStatelessSession statelessSession = _sessionFactoryManager.OpenStatelessSession();
            try
            {
                using (statelessSession.BeginTransaction())
                {
                    executeTransaction(statelessSession);
                    statelessSession.Transaction.Commit();
                }
            }
            catch (Exception excep)
            {
                statelessSession.Transaction.Rollback();
                Log.Error(excep);
            }

            return this;
        }

        #endregion

        #endregion

        #region Methods

        protected virtual void OpenAndBindSession()
        {
            ISession session = _sessionFactoryManager.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected virtual void UnbindSession()
        {
            CurrentSessionContext.Unbind(_sessionFactoryManager.SessionFactory);
        }

        #endregion
    }
}
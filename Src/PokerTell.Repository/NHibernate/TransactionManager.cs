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

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISessionFactoryManager _sessionFactoryManager;



        public TransactionManager(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }




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
                try
                {
                  _sessionFactoryManager.CurrentSession.Transaction.Rollback();   
                }
                catch (TransactionException rollbackExcep)
                {
                    // Under ultra heavy loads we might get: NHibernate.TransactionException: Transaction not successfully started
                    // when trying to roll back due to another error (also due to heavy load)
                    // Although catching this here keeps the application running and the database can be read, we may want to handle this
                    // differently or at least inform the user b/c from now on nothing can be saved to the database.
                    // On the other hand it is very unlikely that it ever really occurs.
                    Log.Error(rollbackExcep);
                }

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




        protected virtual void OpenAndBindSession()
        {
            ISession session = _sessionFactoryManager.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected virtual void UnbindSession()
        {
            try
            {
                CurrentSessionContext.Unbind(_sessionFactoryManager.SessionFactory);
            }
            catch (TransactionException excep)
            {
                // Occurs under ultra heavy loads, so far only encountered during manual testing (sending 10+ hands in 1s)
                // If it happens there is nothing we can do, so log it and hope we can go on
                Log.Error(excep);
            }
        }

    }
}
namespace PokerTell.Repository.NHibernate
{
    using System;
    using System.Reflection;

    using global::NHibernate;

    using Interfaces;

    using log4net;

    public class TransactionManager : ITransactionManager
    {
        readonly ITransaction _transaction;

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionManager(ITransaction transaction)
        {
            _transaction = transaction;
        }

        public ITransactionManagerWithTransaction Execute(Action executeTransaction)
        {
            try
            {
                executeTransaction();
                _transaction.Commit();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                _transaction.Rollback();
            }

            return this;
        }

        public ITransactionManagerUncommitted ExecuteWithoutCommitting(Action executeTransaction)
        {
            try
            {
                executeTransaction();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }

            return this;
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }
        }
    }

    public class TransactionManagerFactory : ITransactionManagerFactory
    {
        public ITransactionManagerWithTransaction New(ITransaction transaction)
        {
            return new TransactionManager(transaction);
        }
    }
}
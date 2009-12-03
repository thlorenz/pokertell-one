namespace PokerTell.Repository.NHibernate
{
    using System;
    using System.Reflection;

    using global::NHibernate;

    using log4net;

    public class TransactionManager : ITransactionManager
    {
        ITransaction _transaction;

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ITransactionManagerWithTransaction InitializeWith(ITransaction transaction)
        {
            _transaction = transaction;
            return this;
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

        public ITransactionManagerWithoutTransaction Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }

            return this;
        }
    }
}
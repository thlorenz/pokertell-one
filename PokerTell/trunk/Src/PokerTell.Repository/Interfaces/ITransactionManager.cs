namespace PokerTell.Repository.Interfaces
{
    using System;

    using global::NHibernate;

    using Infrastructure.Interfaces;

    public interface ITransactionManager : ITransactionManagerWithTransaction, 
                                           ITransactionManagerUncommitted
    {
    }

    public interface ITransactionManagerWithTransaction : IFluentInterface
    {
        #region Public Methods

        ITransactionManagerWithTransaction Execute(Action executeTransaction);

        ITransactionManagerUncommitted ExecuteWithoutCommitting(Action executeTransaction);

        #endregion
    }

    public interface ITransactionManagerUncommitted : IFluentInterface
    {
        #region Public Methods

        void Commit();

        #endregion
    }

    public interface ITransactionManagerFactory : IFluentInterface
    {
        ITransactionManagerWithTransaction New(ITransaction transaction);
    }
}
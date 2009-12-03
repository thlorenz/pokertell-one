namespace PokerTell.Repository.NHibernate
{
    using System;

    using global::NHibernate;

    using PokerTell.Infrastructure.Interfaces;

    public interface ITransactionManager : ITransactionManagerWithoutTransaction, 
                                           ITransactionManagerWithTransaction, 
                                           ITransactionManagerUncommitted
    {
    }

    public interface ITransactionManagerWithoutTransaction : IFluentInterface
    {
        #region Public Methods

        ITransactionManagerWithTransaction InitializeWith(ITransaction transaction);

        #endregion
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

        ITransactionManagerWithoutTransaction Commit();

        #endregion
    }
}
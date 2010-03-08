namespace PokerTell.Repository.Interfaces
{
    using System;

    using global::NHibernate;

    public interface ITransactionManager
    {
        #region Public Methods

        ITransactionManager BatchExecute(Action<IStatelessSession> executeTransaction);

        ITransactionManager Execute(Action executeTransaction);

        TResult Execute<TResult>(Func<TResult> executeRetrieval);

        #endregion
    }
}
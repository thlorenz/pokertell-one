namespace PokerTell.Repository.Tests.NHibernate
{
    using System;

    using global::NHibernate;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Repository.NHibernate;

    public class TransactionManagerTests
    {
        Mock<ITransaction> _transactionMock;

        ITransactionManager _sut;

        [SetUp]
        public void _Init()
        {
            _transactionMock = new Mock<ITransaction>();
            _sut = new TransactionManager(_transactionMock.Object);
        }

        [Test]
        public void Execute_ExeceptionIsThrown_RollsBackTransaction()
        {
            _sut.Execute(() => { throw new Exception(); });
            _transactionMock.Verify(tx => tx.Rollback());
        }

        [Test]
        public void Execute_ExceptionIsThrown_DoesNotCommitTransaction()
        {
            _sut.Execute(() => { throw new Exception(); });
            _transactionMock.Verify(tx => tx.Commit(), Times.Never());
        }

        [Test]
        public void Execute_NoExeceptionIsThrown_CommitsTransaction()
        {
            _sut.Execute(() => { });
            _transactionMock.Verify(tx => tx.Commit());
        }

        [Test]
        public void Execute_NoExecptionIsThrown_DoesNotRollbackTransaction()
        {
            _sut.Execute(() => { });
            _transactionMock.Verify(tx => tx.Rollback(), Times.Never());
        }

        [Test]
        public void ExecuteWithoutCommitting_Never_Commits()
        {
            _sut.ExecuteWithoutCommitting(() => { });
            _transactionMock.Verify(tx => tx.Commit(), Times.Never());
        }

        [Test]
        public void Commit_Always_CommitsTransaction()
        {
            _sut.Commit();
            _transactionMock.Verify(tx => tx.Commit());
        }
    }
}
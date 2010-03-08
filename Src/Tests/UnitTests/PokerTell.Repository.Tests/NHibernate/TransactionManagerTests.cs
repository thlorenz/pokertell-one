namespace PokerTell.Repository.Tests.NHibernate
{
    using System;

    using global::NHibernate;

    using Infrastructure.Interfaces.Repository;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Repository.NHibernate;

    using UnitTests;
    using UnitTests.Tools;

    public class TransactionManagerTests : TestWithLog
    {
        StubBuilder _stub;

        Mock<ISessionFactoryManager> _sessionFactoryStub;

        Mock<ISession> _sessionStub;
        Mock<IStatelessSession> _statelessSessionStub;

        Mock<ITransaction> _transactionMock;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            _transactionMock = new Mock<ITransaction>();

            _sessionStub = new Mock<ISession>();
            _sessionStub
                .SetupGet(s => s.Transaction)
                .Returns(_transactionMock.Object);

            _statelessSessionStub = new Mock<IStatelessSession>();
            _statelessSessionStub
                .SetupGet(ss => ss.Transaction)
                .Returns(_transactionMock.Object);
                
            _sessionFactoryStub = new Mock<ISessionFactoryManager>();
            _sessionFactoryStub
                .SetupGet(sf => sf.CurrentSession)
                .Returns(_sessionStub.Object);
            _sessionFactoryStub
                .Setup(sf => sf.OpenStatelessSession())
                .Returns(_statelessSessionStub.Object);
        }

        [Test]
        public void Execute_NoError_BindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            sut.Execute(() => { });
           
            sut.SessionWasBound.ShouldBeTrue();
        }

        [Test]
        public void ExecuteRetrieval_NoError_BindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            sut.Execute(() => 1);

            sut.SessionWasBound.ShouldBeTrue();
        }

        [Test]
        public void Execute_Error_BindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.Execute(() => { throw new Exception(); }));

            sut.SessionWasBound.ShouldBeTrue();
        }

        [Test]
        public void ExecuteRetrieval_Error_BindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.Execute<int>(() => { throw new Exception(); }));

            sut.SessionWasBound.ShouldBeTrue();
        }

        [Test]
        public void ExecuteRetrieval_Error_ReturnsDefaultOfTResult()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            int result = 1;
            NotLogged(() => result = sut.Execute<int>(() => { throw new Exception(); }));

            result.ShouldBeEqualTo(0);
        }

        [Test]
        public void ExecuteRetrieval_NoError_ReturnsResultOfAction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            const int resultOfAction = 1;
            int result = 0;

            NotLogged(() => result = sut.Execute(() => resultOfAction));

            result.ShouldBeEqualTo(resultOfAction);
        }

        [Test]
        public void Execute_Always_BeginsTransactionUsingSessionFromSessionFactoryManager()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            sut.Execute(() => { });

            _sessionStub.Verify(s => s.BeginTransaction());
        }

        [Test]
        public void Execute_NoError_CommitsTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            sut.Execute(() => { });

            _transactionMock.Verify(tx => tx.Commit());
        }

        [Test]
        public void Execute_Error_DoesNotCommitTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            NotLogged(() => sut.Execute(() => { throw new Exception(); }));

            _transactionMock.Verify(tx => tx.Commit(), Times.Never());
        }

        [Test]
        public void Execute_Error_RollsBackTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            NotLogged(() => sut.Execute(() => { throw new Exception(); }));

            _transactionMock.Verify(tx => tx.Rollback());
        }

        [Test]
        public void Execute_NoError_UnbindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            sut.Execute(() => { });

            sut.SessionWasUnbound.ShouldBeTrue();
        }

        [Test]
        public void ExecuteRetrieval_NoError_UnbindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            sut.Execute(() => 0);

            sut.SessionWasUnbound.ShouldBeTrue();
        }

        [Test]
        public void Execute_Error_UnbindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.Execute(() => { throw new Exception(); }));

            sut.SessionWasUnbound.ShouldBeTrue();
        }

        [Test]
        public void ExecuteRetrieval_Error_UnbindsSession()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.Execute<int>(() => { throw new Exception(); }));

            sut.SessionWasUnbound.ShouldBeTrue();
        }

        [Test]
        public void BatchExecute_Always_OpensStatelessSession()
        {
            var sessionFactoryMock = _sessionFactoryStub;
            var sut = new TransactionManagerMock(sessionFactoryMock.Object);

            sut.BatchExecute(s => { });

            sessionFactoryMock.Verify(sf => sf.OpenStatelessSession());
        }

        [Test]
        public void BatchExecute_NoError_CommitsTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            sut.BatchExecute(s => { });

            _transactionMock.Verify(tx => tx.Commit());
        }

        [Test]
        public void BatchExecute_Error_DoesNotCommitTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.BatchExecute(s => { throw new Exception(); }));

            _transactionMock.Verify(tx => tx.Commit(), Times.Never());
        }

        [Test]
        public void BatchExecute_Error_RollsBackTransaction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);

            NotLogged(() => sut.BatchExecute(s => { throw new Exception(); }));

            _transactionMock.Verify(tx => tx.Rollback());
        }

        [Test]
        public void BatchExecute_Always_ExecutesAction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            bool actionWasExecuted = false;
            sut.BatchExecute(s => actionWasExecuted = true);

            actionWasExecuted.ShouldBeTrue();
        }

        [Test]
        public void Execute_Always_ExecutesAction()
        {
            var sut = new TransactionManagerMock(_sessionFactoryStub.Object);
            bool actionWasExecuted = false;
            sut.Execute(() => actionWasExecuted = true);

            actionWasExecuted.ShouldBeTrue();
        }

    }

    internal class TransactionManagerMock : TransactionManager
    {
        public TransactionManagerMock(ISessionFactoryManager sessionFactoryManager)
            : base(sessionFactoryManager)
        {
            SessionWasBound = false;
            SessionWasUnbound = false;
        }

        public bool SessionWasBound;
        public bool SessionWasUnbound;

        protected override void OpenAndBindSession()
        {
            SessionWasBound = true;
        }

        protected override void UnbindSession()
        {
            SessionWasUnbound = true;
        }
    }
}
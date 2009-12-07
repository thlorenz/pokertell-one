namespace PokerTell.Repository.Tests.NHibernate
{
    using System;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Repository.NHibernate;

    public class SessionFactoryManagerTests
    {
        IEventAggregator _eventAggregator;

        ISessionFactoryManager _sut;

        Mock<IDataProvider> _connectedDataProviderStub;

        [SetUp]
        public void _Init()
        {
            _eventAggregator = new EventAggregator();
            _connectedDataProviderStub = new Mock<IDataProvider>();
            _connectedDataProviderStub
                .SetupGet(dp => dp.IsConnectedToDatabase)
                .Returns(true);

            _sut = new SessionFactoryManager(_eventAggregator);
        }

        [Test]
        public void DatabaseInUseChangedEvent_DataProviderConnectedToDatabase_CallsBuildSessionFactoryOnDataProvider()
        {
            var dataProviderMock = _connectedDataProviderStub;
            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(dataProviderMock.Object);

            dataProviderMock.Verify(dp => dp.BuildSessionFactory());
        }

        [Test]
        public void Use_DataProviderNotConnectedToDatabase_ThrowsArgumentException()
        {
            var dataProviderMock = _connectedDataProviderStub;
            dataProviderMock
                .SetupGet(dp => dp.IsConnectedToDatabase)
                .Returns(false);

            Assert.Throws<ArgumentException>(() => _sut.Use(dataProviderMock.Object));
        }
    }
}
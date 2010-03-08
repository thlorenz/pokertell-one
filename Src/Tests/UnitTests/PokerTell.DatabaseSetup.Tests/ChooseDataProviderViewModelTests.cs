namespace PokerTell.DatabaseSetup.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Events;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.DatabaseSetup.ViewModels;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ChooseDataProviderViewModelTests
    {
        #region Constants and Fields

        Mock<IDatabaseSettings> _databaseSettingsStub;

        IEventAggregator _aggregator;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _databaseSettingsStub = new Mock<IDatabaseSettings>();
            _aggregator = new EventAggregator();
        }

        [Test]
        public void DetermineSelectedItem_NoAvailableProviders_IsValidIsFalse()
        {
            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(new List<IDataProviderInfo>());

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object);
            sut.DetermineSelectedItem();

            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void DetermineSelectedItem_NoAvailableProviders_PublishesWarning()
        {
            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(new List<IDataProviderInfo>());

            bool errorWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>()
                .Subscribe(arg => errorWasPublished = arg.MessageType.Equals(UserMessageTypes.Warning));

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object)
                .DetermineSelectedItem();

            Assert.That(errorWasPublished, Is.True);
        }

        [Test]
        public void DetermineSelectedItem_OneAvailableProvider_IsValidIsTrue()
        {
            var providerInfo = new SqLiteInfo();

            var availableProviders = new List<IDataProviderInfo>
                {
                    providerInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object);
                sut.DetermineSelectedItem();

            Assert.That(sut.IsValid, Is.True);
        }

        [Test]
        public void DetermineSelectedItem_OneAvailableProvider_FirstAvailableItemIsTheNiceNameOfThatProvider()
        {
            var providerInfo = new SqLiteInfo();

            var availableProviders = new List<IDataProviderInfo>
                {
                    providerInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object)
                .DetermineSelectedItem();

            Assert.That(sut.AvailableItems.First(), Is.EqualTo(providerInfo.NiceName));
        }

        [Test]
        public void DetermineSelectedItem_TwoAvailableProvidersNoCurrentProviderDefinedInSettings_SelectedItemIsTheNiceNameOfFirstProviderSortedAlphabetically()
        {
            var sqLiteInfo = new SqLiteInfo();
            var mySqlInfo = new MySqlInfo();
            
            var availableProviders = new List<IDataProviderInfo>
                {
                    mySqlInfo, sqLiteInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object)
                .DetermineSelectedItem();

            // sqliteInfo NiceName is: "Integrated (SQLite)"
            Assert.That(sut.SelectedItem, Is.EqualTo(sqLiteInfo.NiceName));
        }

        [Test]
        public void DetermineSelectedItem_TwoAvailableProvidersCurrentProviderDefinedInSettings_SelectedItemIsTheNiceNameOfCurrentProvider()
        {
            var sqLiteInfo = new SqLiteInfo();
            var mySqlInfo = new MySqlInfo();

            var availableProviders = new List<IDataProviderInfo>
                {
                    mySqlInfo, sqLiteInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);
            _databaseSettingsStub
                .Setup(ds => ds.GetCurrentDataProvider())
                .Returns(mySqlInfo);

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object)
                .DetermineSelectedItem();

            Assert.That(sut.SelectedItem, Is.EqualTo(mySqlInfo.NiceName));
        }

        [Test]
        public void CommitActionCommandExecute_TwoAvailableProvidersSelectedItemIsMySqlNiceName_CallsSettingsSetCurrentDataProviderToWithMySqlDataProvider()
        {
            var sqLiteInfo = new SqLiteInfo();
            var mySqlInfo = new MySqlInfo();

            var availableProviders = new List<IDataProviderInfo>
                {
                    mySqlInfo, sqLiteInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);
            
            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object)
                { SelectedItem = mySqlInfo.NiceName };

            sut.CommitActionCommand.Execute(null);

            _databaseSettingsStub.Verify(ds => ds.SetCurrentDataProviderTo(mySqlInfo));
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseSelected_PublishesInfoMessage()
        {
            var mySqlInfo = new MySqlInfo();
            var availableProviders = new List<IDataProviderInfo>
                {
                    mySqlInfo
                };
            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);

            bool infoWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>().Subscribe(
                arg => infoWasPublished = arg.MessageType == UserMessageTypes.Info);

            var sut = new ChooseDataProviderViewModel(_aggregator, _databaseSettingsStub.Object) { SelectedItem = mySqlInfo.NiceName };

            sut.CommitActionCommand.Execute(null);

            Assert.That(infoWasPublished, Is.True);
        }
       
        #endregion
    }
}
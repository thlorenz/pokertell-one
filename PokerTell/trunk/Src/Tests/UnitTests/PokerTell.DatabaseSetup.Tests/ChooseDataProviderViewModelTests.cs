namespace PokerTell.DatabaseSetup.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PokerTell.DatabaseSetup.ViewModels;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ChooseDataProviderViewModelTests
    {
        #region Constants and Fields

        Mock<IDatabaseSettings> _databaseSettingsStub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _databaseSettingsStub = new Mock<IDatabaseSettings>();
        }

        [Test]
        public void Construct_NoAvailableProviders_SetsAvailableProvidersToEmpty()
        {
            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(new List<IDataProviderInfo>());

            var sut = new ChooseDataProviderViewModel(_databaseSettingsStub.Object);

            Assert.That(sut.AvailableProviders.Count, Is.EqualTo(0));
        }

        [Test]
        public void Construct_OneAvailableProviders_SetsAbailableToAvailableProviders()
        {
            var providerInfo = new SqLiteInfo();

            var availableProviders = new List<IDataProviderInfo>
                {
                    providerInfo
                };

            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProviders);

            var sut = new ChooseDataProviderViewModel(_databaseSettingsStub.Object);

            Assert.That(sut.AvailableProviders.First(), Is.EqualTo(providerInfo));
        }

        #endregion
    }
}
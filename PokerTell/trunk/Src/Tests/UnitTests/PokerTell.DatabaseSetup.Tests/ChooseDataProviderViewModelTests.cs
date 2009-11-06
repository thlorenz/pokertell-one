namespace PokerTell.DatabaseSetup.Tests
{
    using System.Linq;

    using Interfaces;

    using System.Collections.Generic;
    using Moq;

    using NUnit.Framework;

    using ViewModels;

    public class ChooseDataProviderViewModelTests
    {
        Mock<IDatabaseSettings> _databaseSettingsStub;

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

            var availableProvides = new List<IDataProviderInfo>
                {
                    providerInfo
                };
            
            _databaseSettingsStub
                .Setup(ds => ds.GetAvailableProviders())
                .Returns(availableProvides);

            var sut = new ChooseDataProviderViewModel(_databaseSettingsStub.Object);

            Assert.That(sut.AvailableProviders.First(), Is.EqualTo(providerInfo));
        }
    }
}
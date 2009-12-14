namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetSummary
{
    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.ViewModels.StatisticsSetSummary;

    using Tools.GenericUtilities;

    using UnitTests.Tools;

    public class StatisticsSetSummaryRowViewModelTests
    {
        StatisticsSetSummaryRowViewModel _sut;

        Mock<IBarGraphViewModel> _barGraphMock;

        [SetUp]
        public void _Init()
        {
            _barGraphMock = new Mock<IBarGraphViewModel>();
            _sut = new StatisticsSetSummaryRowViewModel(ActionSequences.HeroX, _barGraphMock.Object);
        }

        [Test]
        public void Construct_WithHeroX_ActionNameIsCheck()
        {
            _sut.ActionLetter.IsEqualTo("X");
        }

        [Test]
        public void UpdateWith_Percentage50_PercentageIsSet()
        {
            _sut
                .UpdateWith(50, null)
                .Percentage.IsEqualTo("50%");
        }

        [Test]
        public void UpdateWith_Always_RaisesPropertyChangedForPercentage()
        {
            string propertyName = Reflect.GetProperty(() => _sut.Percentage).Name;
          
            bool propertyChangedWasRaisedForPercentage = false;
            _sut.PropertyChanged +=
                (s, e) => propertyChangedWasRaisedForPercentage = e.PropertyName.Equals(propertyName);

            _sut.UpdateWith(0, null);

            propertyChangedWasRaisedForPercentage.IsTrue();
        }

        [Test]
        public void UpdateWith_Always_CreatesUpdatesBarGraphViewModelWithByColumnPercentages()
        {
            var byColumnPercentage = new[] { 1, 2 };

            _sut.UpdateWith(0, byColumnPercentage);

            _barGraphMock.Verify(bg => bg.UpdateWith(byColumnPercentage));
        }
    }
}
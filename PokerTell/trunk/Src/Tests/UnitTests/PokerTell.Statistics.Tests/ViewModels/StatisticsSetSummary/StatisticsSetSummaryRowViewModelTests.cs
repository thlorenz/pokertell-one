namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetSummary
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

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
            _sut.ActionLetter.ShouldBeEqualTo("X");
        }

        [Test]
        public void UpdateWith_Percentage50_PercentageIsSet()
        {
            _sut
                .UpdateWith(50, null)
                .Percentage.ShouldBeEqualTo("50%");
        }

        [Test]
        public void UpdateWith_Always_RaisesPropertyChangedForPercentage()
        {
            string propertyName = Reflect.GetProperty(() => _sut.Percentage).Name;
          
            bool propertyChangedWasRaisedForPercentage = false;
            _sut.PropertyChanged +=
                (s, e) => propertyChangedWasRaisedForPercentage = e.PropertyName.Equals(propertyName);

            _sut.UpdateWith(0, null);

            propertyChangedWasRaisedForPercentage.ShouldBeTrue();
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
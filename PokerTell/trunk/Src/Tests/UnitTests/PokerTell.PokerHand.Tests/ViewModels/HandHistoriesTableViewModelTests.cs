namespace PokerTell.PokerHand.Tests.ViewModels
{
    using System;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.ViewModels;

    using Tools.GenericUtilities;

    using UnitTests;

    [TestFixture]
    public class HandHistoriesTableViewModelTests : TestWithLog
    {
        #region Constants and Fields

        private HandHistoriesTableViewModel _handHistoriesTable;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _handHistoriesTable = new HandHistoriesTableViewModel();

            for (int i = 0; i < 10; i++)
            {
                _handHistoriesTable.AddHand(new ConvertedPokerHand("PS", (ulong)i, DateTime.Now, 200, 100, 6));
            }
        }

        [Test]
        public void AddingHand_UpdatesHandCountProperty_AndRaisesCountPropertyChanged()
        {
            int lastHandIndexBeforeAdd = _handHistoriesTable.LastHandIndex;
            bool wasRaised = false;
            _handHistoriesTable.PropertyChanged +=
                (sender, e) =>
                wasRaised = wasRaised || Reflect.GetProperty(() => _handHistoriesTable.LastHandIndex).ToString().EndsWith(e.PropertyName);

            _handHistoriesTable.AddHand(new ConvertedPokerHand());
            Assert.That(_handHistoriesTable.LastHandIndex, Is.EqualTo(lastHandIndexBeforeAdd + 1));
            Assert.That(wasRaised);
        }

        [Test]
        public void ChangingSelectedIndex_SelectsHandHistoryAtIndexAndRaisesCurrentHandPropertyChanged()
        {
            const int newIndex = 2;
            bool wasRaised = false;
           
            _handHistoriesTable.PropertyChanged +=
                (sender, e) =>
                wasRaised = wasRaised || Reflect.GetProperty(() => _handHistoriesTable.CurrentHandHistory).ToString().EndsWith(e.PropertyName);

            _handHistoriesTable.SelectedIndex = newIndex;
            Assert.That(_handHistoriesTable.CurrentHandHistory.GameId, Is.EqualTo(newIndex.ToString()));
            Assert.That(wasRaised);
        }

        #endregion
    }
}
namespace PokerTell.Statistics.Tests.ViewModels
{
    using System;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels;

    using Statistics.Filters;

    using UnitTests.Tools;

    public class AnalyzablePokerPlayersFilterAdjustmentViewModelTests
    {
        #region Constants and Fields

        const string playerName = "somePLayer";

        IAnalyzablePokerPlayersFilter _filterStub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _filterStub = AnalyzablePokerPlayersFilter.InactiveFilter;
        }

        [Test]
        public void Constructor_WithFilter_SetsFilterToThatFilter()
        {
            _filterStub.MFilter.IsActive = true;
            
            var sut = new AnalyzablePokerPlayersFilterAdjustmentViewModel().InitializeWith(playerName, _filterStub, delegate { }, delegate { });

            sut.Filter.CurrentFilter.ShouldBeEqualTo(_filterStub);
        }

        [Test]
        public void ApplyFilterToPlayerCommand_Execute_InvokesApplyFilterWithCorrectPlayerName()
        {
            bool wasInvokedWithCorrectName = false;
            Action<string, IAnalyzablePokerPlayersFilter> applyTo =
                (name, filter) => wasInvokedWithCorrectName = name == playerName;

            var sut = new AnalyzablePokerPlayersFilterAdjustmentViewModel().InitializeWith(playerName, _filterStub, applyTo, delegate { });

            sut.ApplyFilterToPlayerCommand.Execute(null);

            wasInvokedWithCorrectName.ShouldBeTrue();
        }

        [Test]
        public void ApplyFilterToPlayerCommand_Execute_InvokesApplyFilterWithCorrectFilter()
        {
            bool wasInvokedWithCorrectFilter = false;
            Action<string, IAnalyzablePokerPlayersFilter> applyTo =
                (name, filter) => wasInvokedWithCorrectFilter = filter.Equals(_filterStub);

            _filterStub.MFilter.IsActive = true;
            var sut = new AnalyzablePokerPlayersFilterAdjustmentViewModel().InitializeWith(playerName, _filterStub, applyTo, delegate { });
            
            sut.ApplyFilterToPlayerCommand.Execute(null);

            wasInvokedWithCorrectFilter.ShouldBeTrue();
        }

        [Test]
        public void ApplyFilterToAllCommand_Execute_InvokesApplyFilterToAllWithCorrectFilter()
        {
            bool wasInvokedWithCorrectFilter = false;
            Action<IAnalyzablePokerPlayersFilter> applyToAll =
                filter => wasInvokedWithCorrectFilter = filter.Equals(_filterStub);

            _filterStub.MFilter.IsActive = true;
            var sut = new AnalyzablePokerPlayersFilterAdjustmentViewModel().InitializeWith(playerName, _filterStub, delegate { }, applyToAll);

            sut.ApplyFilterToAllCommand.Execute(null);

            wasInvokedWithCorrectFilter.ShouldBeTrue();
        }

        #endregion
    }
}
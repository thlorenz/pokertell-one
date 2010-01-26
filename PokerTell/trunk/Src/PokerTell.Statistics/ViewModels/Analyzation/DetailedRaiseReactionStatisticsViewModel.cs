namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;

    using Statistics.Analyzation;

    using StatisticsSetDetails;

    using Tools.Interfaces;

    public class DetailedRaiseReactionStatisticsViewModel : DetailedStatisticsViewModel
    {
        IRaiseReactionStatistics _raiseReactionStatistics;

        #region Constructors and Destructors

        public DetailedRaiseReactionStatisticsViewModel(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            string playerName,
            Streets street,
            ActionSequences actionSequence,
            ITuple<double, double> selectedBetSizeSpan)
            : base("Raise Size")
        {
            _raiseReactionStatistics = new RaiseReactionStatistics()
                .InitializeWith(analyzablePokerPlayers, street, actionSequence);
        }

        protected DetailedRaiseReactionStatisticsViewModel()
            : base("Raise Size")
        {
        }

        #endregion

        #region Methods

        protected override IDetailedStatisticsViewModel CreateTableFor(IActionSequenceStatisticsSet statisticsSet)
        {
            return this;
        }

        #endregion
    }
}
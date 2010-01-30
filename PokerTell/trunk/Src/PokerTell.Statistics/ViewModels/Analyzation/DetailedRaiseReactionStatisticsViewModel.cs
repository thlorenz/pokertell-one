namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;

    using Statistics.Analyzation;

    using StatisticsSetDetails;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class DetailedRaiseReactionStatisticsViewModel : DetailedStatisticsViewModel
    {
        readonly IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        readonly IRaiseReactionStatistics _raiseReactionStatistics;

        readonly IRaiseReactionsAnalyzer _raiseReactionsAnalyzer;

        readonly IConstructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

        readonly ITuple<double, double> _selectedBetSizeSpan;

        #region Constructors and Destructors

        public DetailedRaiseReactionStatisticsViewModel(
            IRaiseReactionStatistics raiseReactionStatistics,
            IRaiseReactionsAnalyzer raiseReactionsAnalyzer,
            IConstructor<IRaiseReactionAnalyzer> raiseReactionAnalyzerMake,
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            IActionSequenceStatisticsSet actionSequenceStatisticsSet,
            ITuple<double, double> selectedBetSizeSpan)
            : base("Raise Size")
        {
            _selectedBetSizeSpan = selectedBetSizeSpan;
            _raiseReactionAnalyzerMake = raiseReactionAnalyzerMake;
            _raiseReactionsAnalyzer = raiseReactionsAnalyzer;
            _raiseReactionStatistics = raiseReactionStatistics;
            _analyzablePokerPlayers = analyzablePokerPlayers;

            if (analyzablePokerPlayers.Count() < 1)
            {
                throw new ArgumentException("need at least one analyzable Player");
            }

            InitializeWith(actionSequenceStatisticsSet);
        }

        protected DetailedRaiseReactionStatisticsViewModel()
            : base("Raise Size")
        {
        }

        #endregion

        #region Methods

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet)
        {
            CreateDescriptionFrom(statisticsSet);

            AnalyzeStatisticsUsingStreetAndActionSequenceFrom(statisticsSet);

            Rows = new List<IDetailedStatisticsRowViewModel> {
                    new DetailedStatisticsRowViewModel("Fold", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.F].Values, "%"),
                    new DetailedStatisticsRowViewModel("Call", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.C].Values, "%"),
                    new DetailedStatisticsRowViewModel("Raise", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.R].Values, "%"),
                    new DetailedStatisticsRowViewModel("Count", _raiseReactionStatistics.TotalCountsByColumnDictionary.Values, string.Empty)
                };

            return this;
        }

        void AnalyzeStatisticsUsingStreetAndActionSequenceFrom(IActionSequenceStatisticsSet statisticsSet)
        {
            _analyzablePokerPlayers.ForEach(
                analyzablePlayer => _raiseReactionsAnalyzer
                                        .AnalyzeAndAdd(_raiseReactionAnalyzerMake.New, analyzablePlayer,statisticsSet.Street,statisticsSet.ActionSequence));

            _raiseReactionStatistics.InitializeWith(_raiseReactionsAnalyzer);
        }

        void CreateDescriptionFrom(IActionSequenceStatisticsSet statisticsSet)
        {
            DetailedStatisticsDescription =
                string.Format(
                    "{0} {1} {2} of the pot {3} on the {4} and was raised",
                    statisticsSet.PlayerName,
                    ActionSequencesUtility.NameLastActionInSequence(statisticsSet.ActionSequence).ToLower(),
                    (_selectedBetSizeSpan.First == _selectedBetSizeSpan.Second) ? _selectedBetSizeSpan.First.ToString() : _selectedBetSizeSpan.First + " to " + _selectedBetSizeSpan.Second,                    
                    statisticsSet.InPosition ? "in position" : "out of position",
                    statisticsSet.Street.ToString().ToLower()
                    );
        }

        #endregion
    }
}
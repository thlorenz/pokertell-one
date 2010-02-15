namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Base;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class PreFlopRaisingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, IPreFlopRaisingHandStrengthStatisticsViewModel
    {
        public PreFlopRaisingHandStrengthStatisticsViewModel(IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, IPreFlopRaisingHandStrengthDescriber preFlopRaisingHandStrengthDescriber)
            : base(preFlopHandStrengthStatistics, preFlopRaisingHandStrengthDescriber, "Raise Sizes")
        {
        }
    }

    public class PreFlopRaisedPotCallingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel
    {
        public PreFlopRaisedPotCallingHandStrengthStatisticsViewModel(IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, IPreFlopRaisedPotCallingHandStrengthDescriber preFlopRaisedPotCallingHandStrengthDescriber)
            : base(preFlopHandStrengthStatistics, preFlopRaisedPotCallingHandStrengthDescriber, "Pot Odds")
        {
        }
    }

    public class PreFlopUnraisedPotCallingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel
    {
        public PreFlopUnraisedPotCallingHandStrengthStatisticsViewModel(IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, IPreFlopUnraisedPotCallingHandStrengthDescriber preFlopUnraisedPotCallingHandStrengthDescriber)
            : base(preFlopHandStrengthStatistics, preFlopUnraisedPotCallingHandStrengthDescriber, "Pot Odds")
        {
        }
    }

    public class PreFlopHandStrengthStatisticsViewModel : StatisticsTableViewModel, IPreFlopHandStrengthStatisticsViewModel
    {
        readonly IPreFlopHandStrengthStatistics _preFlopHandStrengthStatistics;

        static readonly double[] UnraisedPotCallingRatios = ApplicationProperties.UnraisedPotCallingRatios;

        static readonly double[] RaisedPotCallingRatios = ApplicationProperties.RaisedPotCallingRatios;

        static readonly double[] RaiseSizeKeys = ApplicationProperties.RaiseSizeKeys;

        public PreFlopHandStrengthStatisticsViewModel(IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, IPreFlopHandStrengthDescriber handStrengthDescriber, string columnHeaderTitle)
            : base(columnHeaderTitle)
        {
            _preFlopHandStrengthStatistics = preFlopHandStrengthStatistics;
            _preFlopHandStrengthStatistics.InitializeWith(UnraisedPotCallingRatios, RaisedPotCallingRatios, RaiseSizeKeys);
        }

        public void InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, ActionSequences actionSequence)
        {
            _preFlopHandStrengthStatistics.BuildStatisticsFor(analyzablePokerPlayers, actionSequence);
            var kownCards = _preFlopHandStrengthStatistics.KnownCards.Select(c => c.Count());

            Rows = new List<IStatisticsTableRowViewModel>
               {
                   new StatisticsTableRowViewModel("ChenValue", _preFlopHandStrengthStatistics.AverageChenValues, "(avg)"), 
                   new StatisticsTableRowViewModel("SM Grouping", _preFlopHandStrengthStatistics.AverageSklanskyMalmuthGroupings, "(avg)"), 
                   new StatisticsTableRowViewModel("Known Cards", kownCards, string.Empty), 
               };
        }
    }
}
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

        readonly IPreFlopHandStrengthDescriber _handStrengthDescriber;

        public PreFlopHandStrengthStatisticsViewModel(IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, IPreFlopHandStrengthDescriber handStrengthDescriber, string columnHeaderTitle)
            : base(columnHeaderTitle)
        {
            _handStrengthDescriber = handStrengthDescriber;
            _preFlopHandStrengthStatistics = preFlopHandStrengthStatistics;
            _preFlopHandStrengthStatistics.InitializeWith(UnraisedPotCallingRatios, RaisedPotCallingRatios, RaiseSizeKeys);
        }

        public IPreFlopHandStrengthStatisticsViewModel InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, string playerName, ActionSequences actionSequence)
        {
            _preFlopHandStrengthStatistics.BuildStatisticsFor(analyzablePokerPlayers, actionSequence);
            var kownCardsCount = _preFlopHandStrengthStatistics.KnownCards.Select(c => c.Count());

            Rows = new List<IStatisticsTableRowViewModel>
               {
                   new StatisticsTableRowViewModel("Chen", _preFlopHandStrengthStatistics.AverageChenValues, "(avg)"), 
                   new StatisticsTableRowViewModel("S&M", _preFlopHandStrengthStatistics.AverageSklanskyMalmuthGroupings, "(avg)"), 
                   new StatisticsTableRowViewModel("Counts", kownCardsCount, string.Empty), 
               };
            
            StatisticsDescription = _handStrengthDescriber.Describe(playerName, actionSequence);
            StatisticsHint = _handStrengthDescriber.Hint(playerName);
            return this;
        }
    }
}
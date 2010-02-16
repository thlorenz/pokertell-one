namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IPreFlopHandStrengthStatistics
    {
        IEnumerable<IAnalyzablePokerPlayer> AnalyzablePlayersWithKnownCards { get; }

        IEnumerable<IAnalyzablePokerPlayer>[] SortedAnalyzablePokerPlayersWithKnownCards { get; }

        double[] RatiosUsed { get; }

        IEnumerable<IValuedHoleCards>[] KnownCards { get; }

        string[] AverageChenValues { get; }

        string[] AverageSklanskyMalmuthGroupings { get; }

        IPreFlopHandStrengthStatistics InitializeWith(
            double[] unraisedPotCallingRatios, double[] raisedPotCallingRatios, double[] raiseSizeKeys);

        IPreFlopHandStrengthStatistics BuildStatisticsFor(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, ActionSequences actionSequence);
    }
}
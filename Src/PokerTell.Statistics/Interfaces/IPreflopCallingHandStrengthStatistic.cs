namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Analyzation;

    using Infrastructure.Interfaces.PokerHand;

    public interface IPreflopCallingHandStrengthStatistic
    {
        ICollection<IAnalyzablePokerPlayer> AnalyzablePokerPlayers { get; }

        IValuedHoleCardsAverage AverageHandStrength { get; }

        ICollection<IValuedHoleCards> KnownCards { get; }

        void Add(IPreflopCallingAnalyzer analyzer);

        void CalculateAverageHandStrength();
    }
}
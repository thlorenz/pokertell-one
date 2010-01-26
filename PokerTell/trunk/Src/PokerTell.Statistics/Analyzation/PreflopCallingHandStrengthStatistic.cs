namespace PokerTell.Statistics.Analyzation
{
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    public class PreflopCallingHandStrengthStatistic : IPreflopCallingHandStrengthStatistic
    {
        public ICollection<IAnalyzablePokerPlayer> AnalyzablePokerPlayers { get; private set; }

        public ValuedHoleCardsAverage AverageHandStrength { get; private set; }

        public ICollection<IValuedHoleCards> KnownCards { get; private set; }


        public PreflopCallingHandStrengthStatistic()
        {
            AnalyzablePokerPlayers = new List<IAnalyzablePokerPlayer>();
            KnownCards = new List<IValuedHoleCards>();
        }
        
        public void Add(IPreflopCallingAnalyzer analyzer)
        {
            if (analyzer.AnalyzablePokerPlayer != null)
            {
                AnalyzablePokerPlayers.Add(analyzer.AnalyzablePokerPlayer);
            }

            var valuedHoleCards = new ValuedHoleCards(analyzer.HeroHoleCards);
            if (valuedHoleCards.IsValid)
            {
                KnownCards.Add(valuedHoleCards);
            }
        }

        public void CalculateAverageHandStrength()
        {
            AverageHandStrength = new ValuedHoleCardsAverage(KnownCards);
        }
    }
}
namespace PokerTell.Statistics.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools;

    public class PreflopCallingAnalyzer : IPreflopCallingAnalyzer
    {
        #region Constructors and Destructors

        public PreflopCallingAnalyzer(IReactionAnalyzationPreparer analyzationPreparer, bool raisedPot)
        {
            ConvertedHand = analyzationPreparer.ConvertedHand;

            var callingRatios = GetCallingRatios(raisedPot);

            DetermineRatio(analyzationPreparer, callingRatios);

            DetermineHeroHoleCards(analyzationPreparer);
        }

        static double[] GetCallingRatios(bool raisedPot)
        {
            return raisedPot ? ApplicationProperties.RaisedPotCallingRatios : ApplicationProperties.UnraisedPotCallingRatios;
        }

        void DetermineRatio(IReactionAnalyzationPreparer analyzationPreparer, double[] callingRatios)
        {
            Ratio = Normalizer.NormalizeToKeyValues(
                callingRatios, analyzationPreparer.Sequence[analyzationPreparer.HeroIndex].Ratio);
        }

        void DetermineHeroHoleCards(IReactionAnalyzationPreparer analyzationPreparer)
        {
            IEnumerable<string> foundHeroCards = from IConvertedPokerPlayer player in analyzationPreparer.ConvertedHand
                                                 where player.Name.Equals(analyzationPreparer.HeroName)
                                                 select player.Holecards;
            
            HeroHoleCards = foundHeroCards.Count() > 0 ? foundHeroCards.First() : string.Empty;
        }

        #endregion

        #region Properties

        public string HeroHoleCards { get; private set; }

        public double Ratio { get; private set; }

        public IConvertedPokerHand ConvertedHand { get; private set; }

        #endregion
    }
}
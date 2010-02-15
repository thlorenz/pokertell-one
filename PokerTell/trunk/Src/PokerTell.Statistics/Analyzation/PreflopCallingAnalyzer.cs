namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools;

    public class PreflopCallingAnalyzer : IPreflopCallingAnalyzer
    {
        #region Constructors and Destructors

        public PreflopCallingAnalyzer(IAnalyzablePokerPlayer analyzablePokerPlayer, IReactionAnalyzationPreparer analyzationPreparer, bool raisedPot)
        {
            AnalyzablePokerPlayer = analyzablePokerPlayer;
            
            var callingRatios = GetCallingRatios(raisedPot);

            DetermineRatio(analyzationPreparer, callingRatios);

            DetermineHeroHoleCards(analyzablePokerPlayer);
        }

        static double[] GetCallingRatios(bool raisedPot)
        {
            return raisedPot ? ApplicationProperties.RaisedPotCallingRatios : ApplicationProperties.UnraisedPotCallingRatios;
        }

        void DetermineRatio(IReactionAnalyzationPreparer analyzationPreparer, double[] callingRatios)
        {
            // Error here, we need to determine Hero's calling action index instead of just using his position
            
            Ratio = Normalizer.NormalizeToKeyValues(
                callingRatios, analyzationPreparer.Sequence[analyzationPreparer.HeroPosition].Ratio);
        }

        void DetermineHeroHoleCards(IAnalyzablePokerPlayer analyzablePokerPlayer)
        {
            HeroHoleCards = analyzablePokerPlayer.Holecards;
        }

        #endregion

        #region Properties

        public string HeroHoleCards { get; private set; }

        public double Ratio { get; private set; }

        public IAnalyzablePokerPlayer AnalyzablePokerPlayer { get; protected set; }
       
        #endregion
    }
}
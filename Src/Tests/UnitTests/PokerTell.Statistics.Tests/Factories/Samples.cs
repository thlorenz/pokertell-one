namespace PokerTell.Statistics.Tests.Factories
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using PokerHand.Analyzation;

    internal static class Samples
    {
        internal static IAnalyzablePokerPlayer AnalyzablePokerPlayerWith(long id, ActionSequences actionSequence, Streets street, bool inPosition, int betSizeIndex)
        {
            IAnalyzablePokerPlayer analyzablePokerPlayer = new AnalyzablePokerPlayer { Id = id };

            analyzablePokerPlayer.ActionSequences[(int)street] = actionSequence;
            analyzablePokerPlayer.InPosition[(int)street] = inPosition;
            analyzablePokerPlayer.BetSizeIndexes[(int)street] = betSizeIndex;

            return analyzablePokerPlayer;
        }

        public static IAnalyzablePokerPlayer AnalyzablePokerPlayerWith(long id, ActionSequences actionSequence, Streets street, bool inPosition)
        {
            return AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition, 0);
        }

        public static IAnalyzablePokerPlayer AnalyzablePokerPlayerWith(long id, ActionSequences actionSequence, Streets street, StrategicPositions strategicPosition)
        {
            var analyzablePokerPlayer = (AnalyzablePokerPlayer) AnalyzablePokerPlayerWith(id, actionSequence, street, false);
            analyzablePokerPlayer.StrategicPosition = strategicPosition;
            return analyzablePokerPlayer;
        }
    }
}
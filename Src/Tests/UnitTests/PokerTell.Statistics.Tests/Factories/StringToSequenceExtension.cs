namespace PokerTell.Statistics.Tests.Factories
{
    using Infrastructure.Interfaces.PokerHand;

    using PokerHand.Analyzation;

    internal static class StringToSequenceExtension
    {
        internal static IConvertedPokerRound ToConvertedPokerRoundWithIds(this string sequenceString)
        {
            return new PokerHandStringConverter().ConvertedRoundFrom(sequenceString);
        }
    }
}
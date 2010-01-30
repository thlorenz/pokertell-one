namespace PokerTell.Statistics.Interfaces
{
    using Analyzation;

    using Tools.Interfaces;

    public interface IValuedHoleCards
    {
        int ChenValue { get; }

        int SklanskyMalmuthGrouping { get; }

        bool IsValid { get; }

        ITuple<ValuedCard, ValuedCard> ValuedCards { get; }

        bool AreSuited { get; }
    }
}
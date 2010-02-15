namespace PokerTell.Statistics.Interfaces
{
    using Analyzation;

    using Tools.Interfaces;

    public interface IValuedHoleCards
    {
        int ChenValue { get; }

        int SklanskyMalmuthGrouping { get; }

        bool AreValid { get; }

        ITuple<IValuedCard, IValuedCard> ValuedCards { get; }

        bool AreSuited { get; }

        IValuedHoleCards InitializeWith(string strCards);
    }
}
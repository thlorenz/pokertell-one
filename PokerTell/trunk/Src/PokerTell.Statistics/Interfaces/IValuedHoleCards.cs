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

        /// <summary>
        /// Format depends on the suitedness of cards.
        /// </summary>
        /// <returns>"Rank1Rank2" for unsuited and "Rank1Rank2s" for suited cards</returns>
        string Name { get; }

        IValuedHoleCards InitializeWith(string strCards);
    }
}
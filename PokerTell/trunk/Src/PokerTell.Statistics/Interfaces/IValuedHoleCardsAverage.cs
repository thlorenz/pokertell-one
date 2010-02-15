namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Analyzation;

    public interface IValuedHoleCardsAverage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValuedHoleCardsAverage"/> class. 
        /// Creates a Holdem Hand and that represents average for given Collection of Holdem Cards
        /// Calculates the average holdem hand for a Collection of holdem hands
        /// </summary>
        /// <para>
        /// I calculated the average values for all possible starting hands
        /// Average Chen Value: 4, Average Sklansky Malmuth Grouping: 7
        /// </para>
        /// <param name="cardsCollection">
        /// HoldemCards
        /// </param>
        IValuedHoleCardsAverage InitializeWith(IEnumerable<IValuedHoleCards> cardsCollection);

        int ChenValue { get; }

        bool IsValid { get; }

        int SklanskyMalmuthGrouping { get; }
    }
}
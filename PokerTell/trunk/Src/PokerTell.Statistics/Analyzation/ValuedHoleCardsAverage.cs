namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Statistics.Interfaces;

    using Tools.Extensions;

    public class ValuedHoleCardsAverage : IValuedHoleCards
    {
        #region Constructors and Destructors

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
        public ValuedHoleCardsAverage(IEnumerable<IValuedHoleCards> cardsCollection)
        {
            if (cardsCollection.Count() > 0)
            {
                double sumChen = 0;
                double sumSM = 0;

                foreach (IValuedHoleCards valuedCards in cardsCollection)
                {
                    sumChen += valuedCards.ChenValue;
                    sumSM += valuedCards.SklanskyMalmuthGrouping;
                }

                ChenValue = (sumChen / cardsCollection.Count()).ToInt();
                SklanskyMalmuthGrouping = (sumSM / cardsCollection.Count()).ToInt();

                IsValid = true;
            }
        }

        #endregion

        #region Properties

        public int ChenValue { get; private set; }

        public bool IsValid { get; private set; }

        public int SklanskyMalmuthGrouping { get; private set; }

        #endregion
    }
}
namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using PokerTell.Statistics.Interfaces;

    using log4net;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    /// <summary>
    /// Orders and values two given HoleCards. and determines if they are suited.
    /// ChenValue and Sklansky Malmuth Grouping are assigned. If everything went good AreValid is set to true.
    /// AreSuited will indicate suitedness and the Cards are stored in ValuedCards Property for later.
    /// </summary>
    public class ValuedHoleCards : IValuedHoleCards
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ITuple<IValuedCard, IValuedCard> ValuedCards { get; private set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuedHoleCards"/> class. 
        /// Create the Holdem cards for given two cards and orders them by rank
        /// Calculates Gap, ChenValue,Sklansky Malmuth ranking and determines suitedness
        /// </summary>
        /// <param name="card1">
        /// First card
        /// </param>
        /// <param name="card2">
        /// Second card
        /// </param>
        public ValuedHoleCards(string card1, string card2)
        {
            DetermineValidityAndValueCards(card1, card2);
        }

        public ValuedHoleCards(string strCards)
        {
            InitializeWith(strCards);
        }

        public ValuedHoleCards()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuedHoleCards"/> class. 
        /// Constructs Cards from a given string.
        /// If Identification fails, construction is aborted and the Cards
        /// property of this remains null. To identify this error, the Chen Value will be set to -1 and AreValid to false;
        /// This will most likely happen, when the cards were not known and we
        /// are passed "?? ??"
        /// So after this we need to check AreValid
        /// </summary>
        /// <param name="strCards">
        /// Cards to identify "?? ??" if unknown
        /// </param>
        public IValuedHoleCards InitializeWith(string strCards)
        {
            const string patCards = "(?<Card1>([A,K,Q,J,T]|[0-9])[schd]) *" + "(?<Card2>([A,K,Q,J,T]|[0-9])[schd])";
            
            // Deal with "AsKs" and "As Ks"
            try
            {
                Match m = Regex.Match(strCards, patCards);

                // if we can't identify the cards don't construct them
                if (m.Success)
                {
                    string card1 = m.Groups["Card1"].Value;
                    string card2 = m.Groups["Card2"].Value;

                    DetermineValidityAndValueCards(card1, card2);
                }
            }
            catch (Exception excep)  
            {
                excep.Data.Add("strCards", strCards);
                Log.Error("Unexpected", excep);
            }
            return this;
        }

        #endregion

        #region Properties

        public int ChenValue { get; private set; }

        public int SklanskyMalmuthGrouping { get; private set; }

        public bool AreValid { get; private set;
        }

        public bool AreSuited { get; private set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Format depends on the suitedness of cards.
        /// </summary>
        /// <returns>"Rank1Rank2" for unsuited and "Rank1Rank2s" for suited cards</returns>
        public override string ToString()
        {
            var suitedIndicator = ValuedCards.First.Suit == ValuedCards.Second.Suit ? "s" : string.Empty;
            return ValuedCards.First + ValuedCards.Second.ToString() + suitedIndicator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// # For a gap of 0 subtract 0.
        /// # For a gap of 1 subtract 1.
        /// # For a gap of 2 subtract 2.
        /// # For a gap of 3 subtract 4.
        /// # For a gap of 4 or more subtract 5 (includes A2,A3,A4, A5)
        /// </summary>
        private static int GapDevaluation(int gapBetweenCards)
        {
            if (gapBetweenCards < 3)
            {
                return gapBetweenCards;
            }

            if (gapBetweenCards < 4)
            {
                return 4;
            }
            
            return 5;
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Chen_formula#Chen_formula
        /// </summary>
        /// <returns>The Chen Value of the hand</returns>
        private void CalculateChenValue()
        {
            // Pair
            if (ValuedCards.First.Value == ValuedCards.Second.Value)
            {
                // Minimum of 5 (Round up for any cards whose value is <=2)
                ChenValue = ((int)ValuedCards.First.Value) <= 2 ? 5 : (int)(ValuedCards.First.Value * 2.0);
            }
            else
            {
                // Unpaired
                // if the cards are not paired then calculate the gap for the lower card and subtract a gap penalty.
                // The gap is the number of cards required to complete the sequence,
                // for example, a 9 and 6 have a gap of 2, needing an 8 and 7 to complete the sequence
                var gap = ValuedCards.First.Rank - (ValuedCards.Second.Rank + 1);
                int gapPenalty = GapDevaluation(gap);

                // If the cards are of the same suit apply a flush bonus of +2 pts.
                int suitedBonus = AreSuited ? 2 : 0;

                int straightBonus = 0;

                // If the cards have a gap of 0 or 1 and the top card is a J or lower apply a +1 straight bonus
                if (gap <= 1 && ValuedCards.First.Rank <= CardRank.J)
                {
                    straightBonus = 1;
                }

                ChenValue = ((int)Math.Ceiling(ValuedCards.First.Value)) - gapPenalty + suitedBonus + straightBonus;
            }
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Texas_hold_%27em_starting_hands
        /// </summary>
        private void DetermineSklanskyMalmuthGrouping()
        {
            switch (ChenValue)
            {
                case 20:
                case 19:
                case 18:
                case 17:
                case 16:
                case 15:
                case 14:
                case 13:
                case 12:
                    SklanskyMalmuthGrouping = 1;
                    break;
                case 11:
                case 10:
                    SklanskyMalmuthGrouping = 2;
                    break;
                case 9:
                    SklanskyMalmuthGrouping = 3;
                    break;
                case 8:
                    SklanskyMalmuthGrouping = 4;
                    break;
                case 7:
                case 6:
                    SklanskyMalmuthGrouping = 5;
                    break;
                case 5:
                    SklanskyMalmuthGrouping = 7;
                    break;
                default:
                    SklanskyMalmuthGrouping = 9;
                    break;
            }

            // Correct Mapping errors
            // The following hands are the exceptions (off by 1): 55, AQs, A9, AX, 96s, 32s, 98, 97, 76
            // Actually there are much more which we take also care of below
            string strAddOne;
            string strSubtractOne;

            if (AreSuited)
            {
                strAddOne = "J8s,96s,86s,75s,65s,54s,32s";
                strSubtractOne = "ATs,J7s,85s,74s,42s";
            }
            else
            {
                strAddOne = "AT,A9,KT,QT,Q9,T8,87,76";
                strSubtractOne = "65,55,54";
            }

            if (strAddOne.Contains(ToString()))
            {
                SklanskyMalmuthGrouping++;
            }
            else if (strSubtractOne.Contains(ToString()))
            {
                SklanskyMalmuthGrouping--;
            }
            else
            {
                // Deal with very special cases by hand
                if (AreSuited)
                {
                    if (ToString() == "K9s")
                    {
                        SklanskyMalmuthGrouping = 7;
                    }
                }
                else
                {
                    const string str9 = "A8,A7,A6,A5,A4,A3,A2,97";
                    const string str8 = "K9,J8";
                    const string str7 = "J9,T9,98";

                    if (str9.Contains(ToString()))
                    {
                        SklanskyMalmuthGrouping = 9;
                    }
                    else if (str8.Contains(ToString()))
                    {
                        SklanskyMalmuthGrouping = 8;
                    }
                    else if (str7.Contains(ToString()))
                    {
                        SklanskyMalmuthGrouping = 7;
                    }
                }
            }
        }

        private void SortCards()
        {
            if (ValuedCards.First.Rank < ValuedCards.Second.Rank)
            {
                ValuedCards = Tuple.New(ValuedCards.Second, ValuedCards.First);
            }
        }

        private void DetermineValidityAndValueCards(string card1, string card2)
        {
            AreValid = ExtractValuedCards(card1, card2);

            if (AreValid)
            {
                SortCards();
                DetermineSuitedness();

                CalculateChenValue();
                DetermineSklanskyMalmuthGrouping();
            }
        }

        void DetermineSuitedness()
        {
            AreSuited = ValuedCards.First.Suit == ValuedCards.Second.Suit;
        }

        bool ExtractValuedCards(string card1, string card2)
        {
            ValuedCards = Tuple.New((IValuedCard) new ValuedCard(card1), (IValuedCard) new ValuedCard(card2));

            return ValuedCards.First.Rank != CardRank.Unknown && ValuedCards.Second.Rank != CardRank.Unknown;
        }

        #endregion
    }
}
namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using PokerTell.Statistics.Interfaces;

    using log4net;

    public class ValuedHoleCards : IValuedHoleCards
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ValuedCard[] _valuedCards;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuedHoleCards"/> class. 
        /// Create the Holdem cards for given two cards and orders them by rank
        /// Calculates Gap, ChenValue,Sklansky Malmuth ranking and determines suitedness
        /// </summary>
        /// <param name="card1">
        /// First _valuedCard
        /// </param>
        /// <param name="card2">
        /// Second _valuedCard
        /// </param>
        public ValuedHoleCards(string card1, string card2)
        {
            DetermineValidityAndValueCards(card1, card2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuedHoleCards"/> class. 
        /// Constructs Cards from a given string.
        /// If Identification fails, construction is aborted and the Cards
        /// property of this remains null. To identify this error, the Chen Value will be set to -1;
        /// This will most likely happen, when the cards were not known and we
        /// are passed "?? ??"
        /// So after this we need to check this.ChenValue for -1 to see if it failed.
        /// </summary>
        /// <param name="strCards">
        /// Cards to identify "?? ??" if unknown
        /// </param>
        public ValuedHoleCards(string strCards)
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
        }

        #endregion

        #region Properties

        public int ChenValue { get; private set; }

        public int SklanskyMalmuthGrouping { get; private set; }

        public bool IsValid { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            string s = _valuedCards[0].Suit == _valuedCards[1].Suit ? "s" : string.Empty;
            return _valuedCards[0] + _valuedCards[1].ToString() + s;
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
            if (_valuedCards[0].Value == _valuedCards[1].Value)
            {
                // Minimum of 5 (Round up for any cards whose value is <=2)
                ChenValue = ((int)_valuedCards[0].Value) <= 2 ? 5 : (int)(_valuedCards[0].Value * 2.0);
            }
            else
            {
                // Unpaired
                // if the cards are not paired then calculate the gap for the lower card and subtract a gap penalty.
                // The gap is the number of cards required to complete the sequence,
                // for example, a 9 and 6 have a gap of 2, needing an 8 and 7 to complete the sequence
                var gap = _valuedCards[0].Rank - (_valuedCards[1].Rank + 1);
                int gapPenalty = GapDevaluation(gap);

                // If the cards are of the same suit apply a flush bonus of +2 pts.
                int suitedBonus = _valuedCards[0].Suit == _valuedCards[1].Suit ? 2 : 0;

                int straightBonus = 0;

                // If the cards have a gap of 0 or 1 and the top card is a J or lower apply a +1 straight bonus
                if (gap <= 1 && _valuedCards[0].Rank <= CardRank.J)
                {
                    straightBonus = 1;
                }

                ChenValue = ((int)Math.Ceiling(_valuedCards[0].Value)) - gapPenalty + suitedBonus + straightBonus;
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
            // Actually there are much more
            string strAddOne;
            string strSubtractOne;

            var suited = _valuedCards[0].Suit == _valuedCards[1].Suit;
            if (suited)
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
                if (suited)
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
            if (_valuedCards[0].Rank < _valuedCards[1].Rank)
            {
                ValuedCard tmp = _valuedCards[0];
                _valuedCards[0] = _valuedCards[1];
                _valuedCards[1] = tmp;
            }
        }

        private void DetermineValidityAndValueCards(string card1, string card2)
        {
            IsValid = ExtractValuedCards(card1, card2);

            if (IsValid)
            {
                SortCards();

                CalculateChenValue();
                DetermineSklanskyMalmuthGrouping();
            }
        }

        bool ExtractValuedCards(string card1, string card2)
        {
            _valuedCards = new ValuedCard[2];
            _valuedCards[0] = new ValuedCard(card1);
            _valuedCards[1] = new ValuedCard(card2);

            return _valuedCards[0].Rank != CardRank.Unknown && _valuedCards[1].Rank != CardRank.Unknown;
        }

        #endregion
    }
}
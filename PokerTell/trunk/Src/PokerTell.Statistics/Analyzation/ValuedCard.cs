namespace PokerTell.Statistics.Analyzation
{
    using System;

    using Interfaces;

    /// <summary>
    /// Determines the Rank, SUit and value(according to rank) of a card given as a string in the form: "RankSuit" e.g. "Ac"
    /// </summary>
    public class ValuedCard : IValuedCard
    {
        #region Constructors and Destructors

        public ValuedCard(string card)
        {
            if (card != null && card.Length >= 2)
            {
                SetRankOfCard(card.ToCharArray()[0]);
                SetValueOfCard();
                Suit = card.ToCharArray()[1];
            }
            else
            {
                Rank = CardRank.Unknown;
            }
        }

        #endregion

        #region Properties

        public CardRank Rank { get; private set; }

        public char Suit { get; private set; }

        public double Value { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (Rank >= CardRank.T)
            {
                return Rank.ToString();
            }
            
            return ((int)Rank).ToString();
        }

        #endregion

        #region Methods

        private void SetRankOfCard(char chaRank)
        {
            switch (chaRank)
            {
                case 'A':
                    Rank = CardRank.A;
                    break;
                case 'K':
                    Rank = CardRank.K;
                    break;
                case 'Q':
                    Rank = CardRank.Q;
                    break;
                case 'J':
                    Rank = CardRank.J;
                    break;
                case 'T':
                    Rank = CardRank.T;
                    break;
                case '9':
                    Rank = CardRank.Nine;
                    break;
                case '8':
                    Rank = CardRank.Eight;
                    break;
                case '7':
                    Rank = CardRank.Seven;
                    break;
                case '6':
                    Rank = CardRank.Six;
                    break;
                case '5':
                    Rank = CardRank.Five;
                    break;
                case '4':
                    Rank = CardRank.Four;
                    break;
                case '3':
                    Rank = CardRank.Three;
                    break;
                case '2':
                    Rank = CardRank.Two;
                    break;
                default:
                    {
                        Console.WriteLine("Unknown cards");
                        break;
                    }
            }
        }

        private void SetValueOfCard()
        {
            // Is it a number?
            if (Rank <= CardRank.T)
            {
                Value = (double)Rank / 2.0;
            }
            else
            {
                switch (Rank)
                {
                    case CardRank.A:
                        Value = 10.0;
                        break;
                    case CardRank.K:
                        Value = 8.0;
                        break;
                    case CardRank.Q:
                        Value = 7.0;
                        break;
                    case CardRank.J:
                        Value = 6.0;
                        break;
                    default:
                        {
                            Value = -1;
                            break;
                        }
                }
            }
        }

        #endregion
    }

    public enum CardRank
    {
        A = 14,

        K = 13,

        Q = 12,

        J = 11,

        T = 10,

        Nine = 9,

        Eight = 8,

        Seven = 7,

        Six = 6,

        Five = 5,

        Four = 4,

        Three = 3,

        Two = 2,

        Unknown = -1
    }
}



namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;
    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public class BoardViewModel : NotifyPropertyChanged, IBoardViewModel
    {
        const string PatBoardCard = @"(?<rank>" + PatRank + ")(?<suit>" + PatSuit + ")";

        const string PatRank = @"[2-9TJQKA]";

        const string PatSuit = @"[cdhs]";

        string _rank1;

        string _rank2;

        string _rank3;

        string _rank4;

        string _rank5;

        Image _suit1;

        Image _suit2;

        Image _suit3;

        Image _suit4;

        Image _suit5;

        public BoardViewModel()
        {
            CreateEmptyBoard();
            
            Visible = false;
        }

        #region Properties

        bool _visible;
        public bool Visible
        {
            get { return _visible; }

            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }

        public string Rank1
        {
            get { return _rank1; }

            set
            {
                _rank1 = value;
                RaisePropertyChanged(() => Rank1);
            }
        }

        public string Rank2
        {
            get { return _rank2; }

            set
            {
                _rank2 = value;
                RaisePropertyChanged(() => Rank2);
            }
        }

        public string Rank3
        {
            get { return _rank3; }

            set
            {
                _rank3 = value;
                RaisePropertyChanged(() => Rank3);
            }
        }

        public string Rank4
        {
            get { return _rank4; }

            set
            {
                _rank4 = value;
                RaisePropertyChanged(() => Rank4);
            }
        }

        public string Rank5
        {
            get { return _rank5; }

            set
            {
                _rank5 = value;
                RaisePropertyChanged(() => Rank5);
            }
        }

        public Image Suit1
        {
            get { return _suit1; }

            set
            {
                _suit1 = value;
                RaisePropertyChanged(() => Suit1);
            }
        }

        public Image Suit2
        {
            get { return _suit2; }

            set
            {
                _suit2 = value;
                RaisePropertyChanged(() => Suit2);
            }
        }

        public Image Suit3
        {
            get { return _suit3; }

            set
            {
                _suit3 = value;
                RaisePropertyChanged(() => Suit3);
            }
        }

        public Image Suit4
        {
            get { return _suit4; }

            set
            {
                _suit4 = value;
                RaisePropertyChanged(() => Suit4);
            }
        }

        public Image Suit5
        {
            get { return _suit5; }

            set
            {
                _suit5 = value;
                RaisePropertyChanged(() => Suit5);
            }
        }

        public IBoardViewModel HideBoardAfter(int seconds)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods

        public bool Equals(BoardViewModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetHashCode().Equals(other.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(BoardViewModel))
            {
                return false;
            }

            return Equals((BoardViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _rank1 != null ? _rank1.GetHashCode() : 0;
                result = (result * 397) ^ (_rank2 != null ? _rank2.GetHashCode() : 0);
                result = (result * 397) ^ (_rank3 != null ? _rank3.GetHashCode() : 0);
                result = (result * 397) ^ (_rank4 != null ? _rank4.GetHashCode() : 0);
                result = (result * 397) ^ (_rank5 != null ? _rank5.GetHashCode() : 0);
                result = (result * 397) ^ (_suit1 != null ? _suit1.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (_suit2 != null ? _suit2.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (_suit3 != null ? _suit3.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (_suit4 != null ? _suit4.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (_suit5 != null ? _suit5.Source.ToString().GetHashCode() : 0);
                return result;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Member.State(() => _rank1));
            sb.AppendLine(Member.State(() => _suit1.Source));

            sb.AppendLine(Member.State(() => _rank2));
            sb.AppendLine(Member.State(() => _suit2.Source));

            sb.AppendLine(Member.State(() => _rank3));
            sb.AppendLine(Member.State(() => _suit3.Source));

            sb.AppendLine(Member.State(() => _rank4));
            sb.AppendLine(Member.State(() => _suit4.Source));

            sb.AppendLine(Member.State(() => _rank5));
            sb.AppendLine(Member.State(() => _suit5.Source));
            return sb.ToString();
        }

        #endregion

        #region Implemented Interfaces

        #region IPositionedViewModel<string>

        public void UpdateWith(string boardString)
        {
            Visible = string.IsNullOrEmpty(boardString) ? false : true;

            CreateEmptyBoard();

            if (boardString != null)
            {
                MatchCollection matches = Regex.Matches(boardString, PatBoardCard);
                int index = 0;

                // Create Ranks and Suits for found cards
                foreach (Match m in matches)
                {
                    switch (index)
                    {
                        case 0:
                            {
                                Rank1 = m.Groups["rank"].ToString();
                                Suit1 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 1:
                            {
                                Rank2 = m.Groups["rank"].ToString();
                                Suit2 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 2:
                            {
                                Rank3 = m.Groups["rank"].ToString();
                                Suit3 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 3:
                            {
                                Rank4 = m.Groups["rank"].ToString();
                                Suit4 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 4:
                            {
                                Rank5 = m.Groups["rank"].ToString();
                                Suit5 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }
                    }

                    index++;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        void CreateEmptyBoard()
        {
            Suit1 = Suits.Unknown;
            Suit2 = Suits.Unknown;
            Suit3 = Suits.Unknown;
            Suit4 = Suits.Unknown;
            Suit5 = Suits.Unknown;

            Rank1 = string.Empty;
            Rank2 = string.Empty;
            Rank3 = string.Empty;
            Rank4 = string.Empty;
            Rank5 = string.Empty;
        }

        #endregion
    }
}
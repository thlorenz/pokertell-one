namespace PokerTell.PokerHand.ViewModels
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;
    using Tools.WPF.ViewModels;

    public class BoardViewModel : PositionedViewModel<string>, IBoardViewModel
    {
        #region Constants and Fields

        private const string PatBoardCard = @"(?<rank>" + PatRank + ")(?<suit>" + PatSuit + ")";

        private const string PatRank = @"[2-9TJQKA]";

        private const string PatSuit = @"[cdhs]";

        private string _rank1;

        private string _rank2;

        private string _rank3;

        private string _rank4;

        private string _rank5;

        private Image _suit1;

        private Image _suit2;

        private Image _suit3;

        private Image _suit4;

        private Image _suit5;

        #endregion

        #region Constructors and Destructors

        public BoardViewModel(Point location)
        {
            this.Left = location.X;
            this.Top = location.Y;

            this.CreateEmptyBoard();

            this.Visible = false;
        }

        public BoardViewModel()
            : this(new Point(0, 0))
        {
        }

        #endregion

        #region Properties

        public string Rank1
        {
            get
            {
                return this._rank1;
            }

            set
            {
                this._rank1 = value;
                this.RaisePropertyChanged(() => this.Rank1);
            }
        }

        public string Rank2
        {
            get
            {
                return this._rank2;
            }

            set
            {
                this._rank2 = value;
                this.RaisePropertyChanged(() => this.Rank2);
            }
        }

        public string Rank3
        {
            get
            {
                return this._rank3;
            }

            set
            {
                this._rank3 = value;
                this.RaisePropertyChanged(() => this.Rank3);
            }
        }

        public string Rank4
        {
            get
            {
                return this._rank4;
            }

            set
            {
                this._rank4 = value;
                this.RaisePropertyChanged(() => this.Rank4);
            }
        }

        public string Rank5
        {
            get
            {
                return this._rank5;
            }

            set
            {
                this._rank5 = value;
                this.RaisePropertyChanged(() => this.Rank5);
            }
        }

        public Image Suit1
        {
            get
            {
                return this._suit1;
            }

            set
            {
                this._suit1 = value;
                this.RaisePropertyChanged(() => this.Suit1);
            }
        }

        public Image Suit2
        {
            get
            {
                return this._suit2;
            }

            set
            {
                this._suit2 = value;
                this.RaisePropertyChanged(() => this.Suit2);
            }
        }

        public Image Suit3
        {
            get
            {
                return this._suit3;
            }

            set
            {
                this._suit3 = value;
                this.RaisePropertyChanged(() => this.Suit3);
            }
        }

        public Image Suit4
        {
            get
            {
                return this._suit4;
            }

            set
            {
                this._suit4 = value;
                this.RaisePropertyChanged(() => this.Suit4);
            }
        }

        public Image Suit5
        {
            get
            {
                return this._suit5;
            }

            set
            {
                this._suit5 = value;
                this.RaisePropertyChanged(() => this.Suit5);
            }
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

            return this.GetHashCode().Equals(other.GetHashCode());
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

            return this.Equals((BoardViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this._rank1 != null ? this._rank1.GetHashCode() : 0;
                result = (result * 397) ^ (this._rank2 != null ? this._rank2.GetHashCode() : 0);
                result = (result * 397) ^ (this._rank3 != null ? this._rank3.GetHashCode() : 0);
                result = (result * 397) ^ (this._rank4 != null ? this._rank4.GetHashCode() : 0);
                result = (result * 397) ^ (this._rank5 != null ? this._rank5.GetHashCode() : 0);
                result = (result * 397) ^ (this._suit1 != null ? this._suit1.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (this._suit2 != null ? this._suit2.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (this._suit3 != null ? this._suit3.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (this._suit4 != null ? this._suit4.Source.ToString().GetHashCode() : 0);
                result = (result * 397) ^ (this._suit5 != null ? this._suit5.Source.ToString().GetHashCode() : 0);
                return result;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Member.State(() => this._rank1));
            sb.AppendLine(Member.State(() => this._suit1.Source));

            sb.AppendLine(Member.State(() => this._rank2));
            sb.AppendLine(Member.State(() => this._suit2.Source));

            sb.AppendLine(Member.State(() => this._rank3));
            sb.AppendLine(Member.State(() => this._suit3.Source));

            sb.AppendLine(Member.State(() => this._rank4));
            sb.AppendLine(Member.State(() => this._suit4.Source));

            sb.AppendLine(Member.State(() => this._rank5));
            sb.AppendLine(Member.State(() => this._suit5.Source));
            return sb.ToString();
        }

        public override void UpdateWith(string boardString)
        {
            this.Visible = string.IsNullOrEmpty(boardString) ? false : true;

            this.CreateEmptyBoard();

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
                                this.Rank1 = m.Groups["rank"].ToString();
                                this.Suit1 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 1:
                            {
                                this.Rank2 = m.Groups["rank"].ToString();
                                this.Suit2 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 2:
                            {
                                this.Rank3 = m.Groups["rank"].ToString();
                                this.Suit3 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 3:
                            {
                                this.Rank4 = m.Groups["rank"].ToString();
                                this.Suit4 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }

                        case 4:
                            {
                                this.Rank5 = m.Groups["rank"].ToString();
                                this.Suit5 = Suits.GetSuitImageFrom(m.Groups["suit"].ToString());
                                break;
                            }
                    }

                    index++;
                }
            }
        }

        #endregion

        #region Methods

        private void CreateEmptyBoard()
        {
            this.Suit1 = Suits.Unknown;
            this.Suit2 = Suits.Unknown;
            this.Suit3 = Suits.Unknown;
            this.Suit4 = Suits.Unknown;
            this.Suit5 = Suits.Unknown;

            this.Rank1 = string.Empty;
            this.Rank2 = string.Empty;
            this.Rank3 = string.Empty;
            this.Rank4 = string.Empty;
            this.Rank5 = string.Empty;
        }

        #endregion
    }
}
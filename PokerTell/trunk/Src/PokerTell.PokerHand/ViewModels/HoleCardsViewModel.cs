namespace PokerTell.PokerHand.ViewModels
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class HoleCardsViewModel : NotifyPropertyChanged, IHoleCardsViewModel
    {
        #region Constants and Fields

        const string PatHoleCards =
            @"((?<hr1>" + PatRank + ")(?<hs1>" + PatSuit + "))*" + " *((?<hr2>" + PatRank + ")(?<hs2>" + PatSuit + "))";

        const string PatRank = @"[2-9TJQKA]";

        const string PatSuit = @"[cdhs]";

        string _rank1;

        string _rank2;

        Image _suit1;

        Image _suit2;

        #endregion

        #region Constructors and Destructors
        public HoleCardsViewModel()
        {
            Suit1 = Suits.Unknown;
            Suit2 = Suits.Unknown;

            Rank1 = string.Empty;
            Rank2 = string.Empty;

            Visible = false;
        }

        #endregion

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

        #endregion

        #region Public Methods

        public void UpdateWith(string holeCardsString)
        {
            if (holeCardsString != null)
            {
                Match m = Regex.Match(holeCardsString, PatHoleCards);

                Rank1 = m.Groups["hr1"].ToString();
                Rank2 = m.Groups["hr2"].ToString();

                Suit1 = Suits.GetSuitImageFrom(m.Groups["hs1"].ToString());
                Suit2 = Suits.GetSuitImageFrom(m.Groups["hs2"].ToString());
            }

            Visible = !string.IsNullOrEmpty(holeCardsString) && !holeCardsString.Contains("?");
        }

        #endregion
    }
}
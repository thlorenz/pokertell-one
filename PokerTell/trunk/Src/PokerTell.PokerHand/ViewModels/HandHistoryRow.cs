namespace PokerTell.PokerHand.ViewModels
{
    using System.Text;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;

    public class HandHistoryRow
    {
        #region Constants and Fields

        private const string Indent = "___     ";

        private readonly IConvertedPokerPlayer _pokerPlayer;

        #endregion

        #region Constructors and Destructors

        public HandHistoryRow(IConvertedPokerPlayer pokerPlayer)
        {
            this._pokerPlayer = pokerPlayer;
        }

        #endregion

        #region Properties

        public string Flop
        {
            get
            {
                return this.GetActionsFor(Streets.Flop);
            }
        }

        public HoleCardsViewModel HoleCards
        {
            get
            {
                var hcvm = new HoleCardsViewModel();
                hcvm.UpdateWith(this._pokerPlayer.Holecards);
                return hcvm;
            }
        }

        public string M
        {
            get
            {
                return this._pokerPlayer.MBefore.ToString();
            }
        }

        public string PlayerName
        {
            get
            {
                return this._pokerPlayer.Name;
            }
        }

        public string Position
        {
            get
            {
                return this._pokerPlayer.StrategicPosition.ToString();
            }
        }

        public string Preflop
        {
            get
            {
                return this._pokerPlayer.StrategicPosition == StrategicPositions.SB ||
                       this._pokerPlayer.StrategicPosition == StrategicPositions.BB
                           ? Indent + this.GetActionsFor(Streets.PreFlop)
                           : this.GetActionsFor(Streets.PreFlop);
            }
        }

        public string River
        {
            get
            {
                return this.GetActionsFor(Streets.River);
            }
        }

        public string Turn
        {
            get
            {
                return this.GetActionsFor(Streets.Turn);
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Member.State(() => this.Position) + "  :  ");
            sb.Append(Member.State(() => this.PlayerName) + "  :  ");
            sb.Append(Member.State(() => this.Preflop) + "  :  ");
            sb.Append(Member.State(() => this.Flop) + "  :  ");
            sb.Append(Member.State(() => this.Turn) + "  :  ");
            sb.Append(Member.State(() => this.River) + "  :  ");
            return sb.ToString();
        }

        #endregion

        #region Methods

        private string GetActionsFor(Streets street)
        {
            return this._pokerPlayer.Count > (int)street && this._pokerPlayer[street] != null
                       ? this._pokerPlayer[street].ToString()
                       : string.Empty;
        }

        #endregion
    }
}
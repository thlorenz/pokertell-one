namespace PokerTell.PokerHand.Aquisition
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// PokerAction used during parsing or importing from other databases like PokerOffice
    /// </summary>
    public class AquiredPokerAction : PokerAction, IAquiredPokerAction
    {
        #region Constructors and Destructors

        public AquiredPokerAction()
        {
        }

        public AquiredPokerAction(ActionTypes what, double ratio)
        {
            InitializeWith(what, ratio);
        }

        #endregion

        #region Properties

        public double ChipsGained
        {
            get { return CalculateChipsGain(); }
        }

        #endregion

        #region Methods

        private double CalculateChipsGain()
        {
            switch (What)
            {
                case ActionTypes.A:
                case ActionTypes.F:
                case ActionTypes.X:
                    return 0;
                case ActionTypes.B:
                case ActionTypes.C:
                case ActionTypes.P:
                case ActionTypes.R:
                    return -Ratio;
                case ActionTypes.U:
                case ActionTypes.W:
                    return Ratio;
                default:
                    return 0;
            }
        }

        #endregion

        public IAquiredPokerAction InitializeWith(ActionTypes what, double ratio)
        {
            What = what;
            Ratio = ratio;
            return this;
        }
    }
}
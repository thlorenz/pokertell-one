namespace PokerTell.PokerHandParsers
{
    public abstract class StreetsParser
    {
        #region Properties

        public string Preflop { get; protected set; }

        public string Flop { get; protected set; }

        public string Turn { get; protected set; }

        public string River { get; protected set; }

        public bool HasFlop { get; protected set; }

        public bool HasTurn { get; protected set; }

        public bool HasRiver { get; protected set; }

        public bool IsValid { get; protected set; }

        #endregion

        #region Public Methods

        public abstract void Parse(string handHistory);

        #endregion

        public override string ToString()
        {
            return string.Format("Preflop: {0}, Flop: {1}, Turn: {2}, River: {3}, HasFlop: {4}, HasTurn: {5}, HasRiver: {6}, IsValid: {7}", Preflop, Flop, Turn, River, HasFlop, HasTurn, HasRiver, IsValid);
        }
    }
}
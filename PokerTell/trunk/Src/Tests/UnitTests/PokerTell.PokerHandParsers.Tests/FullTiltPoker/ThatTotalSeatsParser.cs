namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class ThatTotalSeatsParser : Tests.ThatTotalSeatsParser
    {
        #region Public Methods

       public override void Parse_HandHistoryWithoutValidTotalSeats_IsValidIsFalse()
        {
            // String without a seat indication for FullTilt means 9-players
        }

        #endregion

        #region Methods

        protected override TotalSeatsParser GetTotalSeatsParser()
        {
            return new PokerHandParsers.FullTiltPoker.TotalSeatsParser();
        }

        protected override string ValidTotalSeats(int totalSeats)
        {
            switch (totalSeats)
            {
                case 2:
                    return HeadsUp();
                case 9:
                    return NinePlayers();
                default:
                    // Table Mascot (6 max) 
                    return string.Format("Table Mascot ({0} max) ", totalSeats);
            }
        }

        static string HeadsUp()
        {
            // Table Flash (heads up) 
            return string.Format("Table Flash (heads up) ");
        }

        static string NinePlayers()
        {
            return "Table Flash";
        }

        #endregion
    }
}
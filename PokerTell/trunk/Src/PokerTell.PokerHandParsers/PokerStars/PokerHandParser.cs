namespace PokerTell.PokerHandParsers.PokerStars
{
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public class PokerHandParser : PokerHandParsers.PokerHandParser
    {
        #region Constructors and Destructors

        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake,
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake,
            IConstructor<IAquiredPokerRound> aquiredRoundMake,
            IConstructor<IAquiredPokerAction> aquiredActionMake)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = "PokerStars";
            
            AnteParser = new AnteParser();
            BlindsParser = new BlindsParser();
            BoardParser = new BoardParser();
            HandHeaderParser = new HandHeaderParser();
            HoleCardsParser = new HoleCardsParser();
            PlayerActionsParser =new PlayerActionsParser(_aquiredActionMake);
            PlayerSeatsParser= new PlayerSeatsParser();
            SmallBlindSeatNumberParser = new SmallBlindSeatNumberParser();
            StreetsParser = new StreetsParser();
            TableNameParser = new TableNameParser();
            TimeStampParser = new TimeStampParser();
            TotalPotParser = new TotalPotParser();
            TotalSeatsParser = new TotalSeatsParser();
        }

        #endregion
    }
}
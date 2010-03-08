namespace PokerTell.PokerHandParsers.PokerStars
{
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.PokerRooms;

    public class PokerHandParser : Base.PokerHandParser
    {
        #region Constructors and Destructors

        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake,
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake,
            IConstructor<IAquiredPokerRound> aquiredRoundMake,
            IConstructor<IAquiredPokerAction> aquiredActionMake)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = PokerStarsInfo.Site;
            
            AnteParser = new AnteParser();
            BlindsParser = new BlindsParser();
            BoardParser = new BoardParser();
            HandHeaderParser = new HandHeaderParser();
            HoleCardsParser = new HoleCardsParser();
            PlayerActionsParser = new PlayerActionsParser(_aquiredActionMake);
            PlayerSeatsParser = new PlayerSeatsParser();
            SmallBlindPlayerNameParser = new SmallBlindPlayerNameParser();
            StreetsParser = new StreetsParser();
            TableNameParser = new TableNameParser();
            TimeStampParser = new TimeStampParser();
            TotalPotParser = new TotalPotParser();
            TotalSeatsParser = new TotalSeatsParser();
        }

        #endregion
    }
}
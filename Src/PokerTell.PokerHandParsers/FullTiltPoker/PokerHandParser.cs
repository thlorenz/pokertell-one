namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System;
    using System.Collections.Generic;

    using Infrastructure;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public class PokerHandParser : PokerTell.PokerHandParsers.PokerHandParser
    {

        readonly TotalSeatsParser _fullTiltTotalSeatsParser;

        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake,
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake,
            IConstructor<IAquiredPokerRound> aquiredRoundMake,
            IConstructor<IAquiredPokerAction> aquiredActionMake)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = PokerSites.FullTiltPoker;

            AnteParser = new AnteParser();
            BlindsParser = new BlindsParser();
            BoardParser = new BoardParser();
            HandHeaderParser = new HandHeaderParser();
            HeroNameParser = new HeroNameParser();
            HoleCardsParser = new HoleCardsParser();
            PlayerActionsParser = new PlayerActionsParser(_aquiredActionMake);
            PlayerSeatsParser = new PlayerSeatsParser();
            SmallBlindPlayerNameParser = new SmallBlindPlayerNameParser();
            StreetsParser = new StreetsParser();
            TableNameParser = new TableNameParser();
            TimeStampParser = new TimeStampParser();
            TotalPotParser = new TotalPotParser();

            _fullTiltTotalSeatsParser = new TotalSeatsParser();
            TotalSeatsParser = _fullTiltTotalSeatsParser;
            GameTypeParser = new GameTypeParser();
        }

        protected override void ParseTotalSeats()
        {
            if (HandHeaderParser.IsTournament)
            {
                // set record
                // set is tournament
                
                base.ParseTotalSeats();

                // update record
            }
            else
            {
                base.ParseTotalSeats();
            }

        }
    }
}
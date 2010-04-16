namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class PokerHandParser : PokerTell.PokerHandParsers.PokerHandParser
    {
        readonly IFullTiltPokerTotalSeatsParser _fullTiltTotalSeatsParser;

        readonly ITotalSeatsForTournamentsRecordKeeper _totalSeatsForTournamentsRecordKeeper;

        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake, 
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake, 
            IConstructor<IAquiredPokerRound> aquiredRoundMake, 
            IConstructor<IAquiredPokerAction> aquiredActionMake, 
            IFullTiltPokerAnteParser anteParser, 
            IFullTiltPokerBlindsParser blindsParser, 
            IFullTiltPokerBoardParser boardParser, 
            IFullTiltPokerGameTypeParser gameTypeParser, 
            IFullTiltPokerHandHeaderParser handHeaderParser, 
            IFullTiltPokerHeroNameParser heroNameParser, 
            IFullTiltPokerHoleCardsParser holeCardsParser, 
            IFullTiltPokerPlayerActionsParser playerActionsParser, 
            IFullTiltPokerPlayerSeatsParser playerSeatsParser, 
            IFullTiltPokerSmallBlindPlayerNameParser smallBlindPlayerNameParser, 
            IFullTiltPokerStreetsParser streetsParser, 
            IFullTiltPokerTableNameParser tableNameParser, 
            IFullTiltPokerTimeStampParser timeStampParser, 
            IFullTiltPokerTotalPotParser totalPotParser, 
            IFullTiltPokerTotalSeatsParser totalSeatsParser,
            ITotalSeatsForTournamentsRecordKeeper totalSeatsForTournamentsRecordKeeper)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = PokerSites.FullTiltPoker;

            AnteParser = anteParser;
            BlindsParser = blindsParser;
            BoardParser = boardParser;
            HandHeaderParser = handHeaderParser;
            HeroNameParser = heroNameParser;
            HoleCardsParser = holeCardsParser;
            PlayerActionsParser = playerActionsParser;
            PlayerSeatsParser = playerSeatsParser;
            SmallBlindPlayerNameParser = smallBlindPlayerNameParser;
            StreetsParser = streetsParser;
            TableNameParser = tableNameParser;
            TimeStampParser = timeStampParser;
            TotalPotParser = totalPotParser;

            _fullTiltTotalSeatsParser = totalSeatsParser;
            TotalSeatsParser = _fullTiltTotalSeatsParser;
            
            GameTypeParser = gameTypeParser;

            _totalSeatsForTournamentsRecordKeeper = totalSeatsForTournamentsRecordKeeper;
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
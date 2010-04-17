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
            GameTypeParser = gameTypeParser;
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
            
            _totalSeatsForTournamentsRecordKeeper = totalSeatsForTournamentsRecordKeeper;
        }

        /// <summary>
        /// Assumes that the HandHeader was parsed before since it relies on information obtained during that step
        /// </summary>
        protected override void ParseTotalSeats()
        {
            _fullTiltTotalSeatsParser.IsTournament = HandHeaderParser.IsTournament;

            if (HandHeaderParser.IsTournament)
            {
                _fullTiltTotalSeatsParser.TotalSeatsRecord =
                    _totalSeatsForTournamentsRecordKeeper.GetTotalSeatsRecordFor(HandHeaderParser.TournamentId);

                base.ParseTotalSeats();

                if (_fullTiltTotalSeatsParser.IsValid)
                    _totalSeatsForTournamentsRecordKeeper.SetTotalSeatsRecordIfItIsOneFor(HandHeaderParser.TournamentId, _fullTiltTotalSeatsParser.TotalSeats);
            }
            else
            {
                base.ParseTotalSeats();
            }
        }
    }
}
namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class PokerHandParser : PokerTell.PokerHandParsers.PokerHandParser
    {
        public PokerHandParser(
            IConstructor<IAquiredPokerHand> aquiredHandMake, 
            IConstructor<IAquiredPokerPlayer> aquiredPlayerMake, 
            IConstructor<IAquiredPokerRound> aquiredRoundMake, 
            IConstructor<IAquiredPokerAction> aquiredActionMake, 
            IPokerStarsAnteParser anteParser, 
            IPokerStarsBlindsParser blindsParser, 
            IPokerStarsBoardParser boardParser, 
            IPokerStarsGameTypeParser gameTypeParser, 
            IPokerStarsHandHeaderParser handHeaderParser, 
            IPokerStarsHeroNameParser heroNameParser, 
            IPokerStarsHoleCardsParser holeCardsParser, 
            IPokerStarsPlayerActionsParser playerActionsParser, 
            IPokerStarsPlayerSeatsParser playerSeatsParser, 
            IPokerStarsSmallBlindPlayerNameParser smallBlindPlayerNameParser, 
            IPokerStarsStreetsParser streetsParser, 
            IPokerStarsTableNameParser tableNameParser, 
            IPokerStarsTimeStampParser timeStampParser, 
            IPokerStarsTotalPotParser totalPotParser, 
            IPokerStarsTotalSeatsParser totalSeatsParser)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = PokerSites.PokerStars;

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
            TotalSeatsParser = totalSeatsParser;
            GameTypeParser = gameTypeParser;
        }
    }
}
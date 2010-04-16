namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    /// <summary>
    /// HeroName indication for full tilt is currently identical to the one in PokerStars
    /// </summary>
    public class HeroNameParser : PokerStars.HeroNameParser, IFullTiltPokerHeroNameParser 
    {
    }
}
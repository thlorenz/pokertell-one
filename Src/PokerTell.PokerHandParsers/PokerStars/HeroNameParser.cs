namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class HeroNameParser : Base.HeroNameParser, IPokerStarsHeroNameParser
    {
        const string PokerStarsHeroNamePattern = @"Dealt to (?<HeroName>.+) \[" + SharedPatterns.CardPattern;

        protected override string HeroNamePattern
        {
            get { return PokerStarsHeroNamePattern; }
        }
    }
}
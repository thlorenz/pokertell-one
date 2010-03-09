namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;

    public class HeroNameParser : Base.HeroNameParser
    {
        const string PokerStarsHeroNamePattern = @"Dealt to (?<HeroName>.+) \[" + SharedPatterns.CardPattern;

        protected override string HeroNamePattern
        {
            get { return PokerStarsHeroNamePattern; }
        }
    }
}
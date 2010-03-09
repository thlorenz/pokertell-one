namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class HeroNameParserTests : Tests.HeroNameParserTests
    {
        protected override HeroNameParser GetHeroNameParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.HeroNameParser();
        }

        protected override string GetValidHeroNameSnippetFor(string heroName)
        {
            // Dealt to renniweg [Qh Td]
            return string.Format("Dealt to {0} [Qh Td]", heroName);
        }
    }
}
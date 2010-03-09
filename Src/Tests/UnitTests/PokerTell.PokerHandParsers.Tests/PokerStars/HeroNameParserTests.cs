namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    using NUnit.Framework;

    [TestFixture]
    public class HeroNameParserTests : Tests.HeroNameParserTests
    {
        protected override HeroNameParser GetHeroNameParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.HeroNameParser();
        }
    }
}
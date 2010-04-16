namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface IHeroNameParser : IFluentInterface
    {
        string HeroName { get; }

        bool IsValid { get; }

        IHeroNameParser Parse(string handHistory);
    }

    public interface IPokerStarsHeroNameParser : IHeroNameParser
    {
    }

    public interface IFullTiltPokerHeroNameParser : IHeroNameParser
    {
    }

}
namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class AnteParserTests : Base.AnteParserTests
    {
        protected override AnteParser GetAnteParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.AnteParser();
        }

        protected override string ValidTournamentAnte(double ante)
        {
            // Lezli90: posts the ante 50
            return string.Format("posts the ante {0}", ante);
        }
    }
}
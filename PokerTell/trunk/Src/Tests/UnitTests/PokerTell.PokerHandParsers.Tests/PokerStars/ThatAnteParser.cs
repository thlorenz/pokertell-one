namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using Base;

    public class ThatAnteParser : Tests.ThatAnteParser
    {
        protected override AnteParser GetAnteParser()
        {
            return new PokerHandParsers.PokerStars.AnteParser();
        }

        protected override string ValidTournamentAnte(double ante)
        {
            // Lezli90: posts the ante 50
            return string.Format("posts the ante {0}", ante);
        }
    }
}